﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TowerSpawnerPro : MonoBehaviour {
	public GameObject towerContainer;
	public GameObject[] towerPrefabs = new GameObject[3];

	public GameObject towerExplosion;

	public GameObject towerDust;

	public GameObject towerPump;

	private GameManager gameManager;


	public Tileset tileset;
	
	public IsOverUI isOverUI;

	[Range(0f, 1f)]
	public float minDragTime = .2f;

	public int towerGuiSorting = 51;
	// offset for tile check at the towers base
	float towerBaseYOffset = -.4f;

	GameObject draggedTowerPrefab;
	GameObject draggedTowerInstance;
	EventTriggerProxy draggedTowerOwner;
	bool dragging;
	int flashMixId = Shader.PropertyToID("_FlashMix");

	Vector2 towerOffset = new Vector2();

	void Start () {
		gameManager = FindObjectOfType<GameManager>();
		for (int id = 0; id < towerPrefabs.Length; id++)
		{
			GameObject tp = towerPrefabs[id];
			if (!tp) {
				Debug.Log("Missing tower prefab at " + id);
				continue;
			}
			TowerScript ts = tp.GetComponent<TowerScript>();
			if (!ts) {
				Debug.Log("Missing TowerScript at " + id);
				continue;
			}
			ts.id = id;
		}
	}

	float dragTime;
    private Tile previouslyDraggedTile;

	void Update () {
		if (!GameManager.canDoActions) return;

		// we are actively dragging the tower
		if (dragging) {
			dragTime += Time.deltaTime;
			Vector3 pos = InputUtils.WorldMousePosition();
			// apply offset
			pos = new Vector2(pos.x + towerOffset.x, pos.y + towerOffset.y);
			draggedTowerInstance.transform.position = pos;

			// check bottom of the tower
            Tile tile = tileset.GetTileAt(new Vector2(pos.x, pos.y + towerBaseYOffset));
            if(previouslyDraggedTile != tile)
            {
                if (previouslyDraggedTile != null) previouslyDraggedTile.CancelBuildTarget();
				// only allow active tiles
                if (tile != null && tile.gameObject.activeInHierarchy) tile.SetAsBuildTarget();

                previouslyDraggedTile = tile;
            }   
        }
	}

	float mix;
	public void PickTower (EventTriggerProxy owner, GameObject towerPrefab) {
		if (draggedTowerPrefab != null) {
			Debug.LogError("Already dragging a tower " + towerPrefab);
			return;
		}
		if (!isValidTowerPrafab(towerPrefab)) {
			Debug.LogError("Invalid tower prefab! " + towerPrefab);
			return;
		}
		Debug.Log("PickTower " + towerPrefab);
		dragTime = 0;
		dragging = true;
		draggedTowerPrefab = towerPrefab;
		draggedTowerOwner = owner;

		Sounds.PlayTowerTake();

		Transform parent = towerContainer.transform;
		Vector3 pos = InputUtils.WorldMousePosition();
		draggedTowerInstance = GameObject.Instantiate(draggedTowerPrefab, pos, Quaternion.identity, parent);
		draggedTowerInstance.transform.Find("Shadow").gameObject.SetActive(false);
		// TowerScript ts = draggedTowerInstance.GetComponent<TowerScript>();

		// buttons are at 50 for some reason...
		ChangeDrawSorting(draggedTowerInstance, "GUI", towerGuiSorting);
		//draggedTowerInstance.transform.DOPunchRotation(new Vector3(0, 0, 30), .5f, 10, 1);
		SpriteRenderer sprite = draggedTowerInstance.transform.Find("Body").GetComponent<SpriteRenderer>();
		
		float flashDuration = .2f;
		int repeats = 1;
	
		DOTween.Sequence()
		.SetLoops(repeats * 2, LoopType.Yoyo)
		.Append(
			DOTween.To(
				() => mix, 
				v => {
					sprite.material.SetFloat(flashMixId, mix = v);		
				}, 
				1, 
				flashDuration/2
			)
		);
		
		// this is in game units
		towerOffset.y = GameManager.IS_MOBILE?1.0f:0.0f;
	}

	void ChangeDrawSorting(GameObject tower, string layer, int order) {
		GameObject body = tower.transform.Find("Body").gameObject;
		SpriteRenderer bodySprite = body.GetComponent<SpriteRenderer>();
		bodySprite.sortingLayerName = layer;
		bodySprite.sortingOrder = order;
	}

	public void PlaceTower (EventTriggerProxy owner, GameObject towerPrefab) {
		if (draggedTowerPrefab == null) {
			Debug.LogError("Nothing to place");
			return;
		}
		if (draggedTowerPrefab != towerPrefab) {
			Debug.LogError("Invalid tower to place " + towerPrefab + ", expected " + draggedTowerPrefab);
			return;
		}
		Debug.Log("PlaceTower");

		dragging = false;

		if (dragTime <= minDragTime) {
			StartCoroutine("PlaceOrReturnTowerLater");
			return;
		} else {
			PlaceOrReturnTower();
		}
	}

	IEnumerator PlaceOrReturnTowerLater () {
		yield return new WaitForSeconds(minDragTime);
		PlaceOrReturnTower();
		yield return null;
	}

    [SerializeField]
    private float towerPlacementSqueezeStrength = 0.75f;

    [SerializeField]
    private Vector2 cameraShakeTimeStrength = new Vector2(0.06f, 0.12f);

	void PlaceOrReturnTower() {
		if (draggedTowerInstance == null) {
			Debug.LogError("Nothing to place");
			return;
		}
		draggedTowerPrefab = null;


		if (previouslyDraggedTile != null) previouslyDraggedTile.CancelBuildTarget();
		previouslyDraggedTile = null;

		// TODO return tower
		if (isOverUI.getOverCount(Input.mousePosition) > 0) {
			ReturnTower();
			return;
		}
		Vector3 pos = InputUtils.WorldMousePosition();
		// drag offset + tower base offset
		pos = new Vector2(pos.x + towerOffset.x, pos.y + towerOffset.y + towerBaseYOffset);

		Tile tile = tileset.GetTileAt(pos);
		if (tile == null || !tile.gameObject.activeInHierarchy) {
			ReturnTower();
		} else if (tile.CanBuild()) {
			ChangeDrawSorting(draggedTowerInstance, "Buildings", 3);
			Vector3 targetPos = tile.transform.position;
			draggedTowerInstance.transform.DOMove(targetPos, .2f)
				.SetEase(Ease.InOutFlash)
				.OnComplete(()=>{
					Sounds.PlayTowerBuild();
					tile.CancelBuildTarget();
					tile.Build(draggedTowerInstance);
					gameManager.TowerBuild(draggedTowerOwner, draggedTowerInstance);
					draggedTowerInstance.transform.Find("Shadow").gameObject.SetActive(true);

					GameObject dust = GameObject.Instantiate(towerDust, targetPos, Quaternion.identity, draggedTowerInstance.transform);
                    dust.transform.localPosition = new Vector3(-0.05f, 0.05f, 0);

                    Tweens.Squeeze(draggedTowerInstance, towerPlacementSqueezeStrength);

                    previouslyDraggedTile = null;
					draggedTowerInstance = null;
					dragging = false;
					draggedTowerOwner = null;


                    gameObject.transform.DOShakePosition(cameraShakeTimeStrength.x, cameraShakeTimeStrength.y);
				}
			);
		} else {
            tile.CancelBuildTarget();
			ReturnTower();
		}

		// GameObject.Destroy(draggedTowerInstance);
		// draggedTowerInstance = null;
	}

	public void ReturnTower() {
		if (draggedTowerInstance == null) return;
		ReturnTower(draggedTowerOwner, draggedTowerInstance);
        previouslyDraggedTile = null;
		draggedTowerInstance = null;
		draggedTowerOwner = null;
	}

	public void ReturnTower(EventTriggerProxy button, GameObject tower, bool lockButton = false) {
		ChangeDrawSorting(tower, "GUI", towerGuiSorting);
		TowerScript ts = tower.GetComponent<TowerScript>();
		bool wasAttached = ts.DetachFromTile();
            
		if (wasAttached) {
			tower.transform.Find("Shadow").gameObject.SetActive(false);
			// lol jank
			Tile tile = tileset.GetTileAt(tower.transform.position);
			if (tile != null) {
				GameObject pump = null;
				for (int i = 0; i < tile.transform.childCount; i++)
        		{
					Transform child = tile.transform.GetChild(i);
					if (child.tag == "Pump")
					{
						pump = child.gameObject;
						break;	
					}
				}
				if (pump != null) {
					
				}
			}

			GameObject.Instantiate(towerExplosion, tower.transform.position, Quaternion.identity, towerContainer.transform);

            Tweens.Squeeze(tower, 2, 1.5f);

            tower.transform.DOMove(button.transform.position, .5f)
				.SetEase(Ease.InSine)
				.OnComplete(()=>{
					button.ReturnTower();
					if (lockButton) {
						button.Lock();
					}
					GameObject.Destroy(tower);	
				}
			);


		} else {
            ts.FlashRed();

            // this api...
            DOTween.Sequence()
			.Append(
				tower.transform.DOPunchRotation(new Vector3(0, 0, 30), .5f, 10, 1)
			)
			.Append(
				tower.transform.DOMove(button.transform.position, .5f)
					.SetEase(Ease.InOutSine)
					.OnComplete(()=>{
						button.ReturnTower();
						if (lockButton) {
							button.Lock();
						}
						GameObject.Destroy(tower);	
					}
				)
			);
		}
	}

	bool isValidTowerPrafab(GameObject gameObject) {
		if (gameObject == null) return false;
		foreach (var item in towerPrefabs){
			if (item == gameObject) {
				return true;
			}
		}
		return false;
	}

	public GameObject GetPumpPrefab() {
		return towerPump;
	}
}
