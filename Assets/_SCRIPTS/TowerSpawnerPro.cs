using System.Collections;
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

	private GameManager gameManager;


	public Tileset tileset;
	
	public IsOverUI isOverUI;

	[Range(0f, 1f)]
	public float minDragTime = .2f;

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

		Vector3 pos = InputUtils.WorldMousePosition();
		// we are actively dragging the tower
		if (dragging) {
			dragTime += Time.deltaTime;
			draggedTowerInstance.transform.position = new Vector2(pos.x + towerOffset.x, pos.y + towerOffset.y);

            Tile tile = tileset.GetTileAt(pos);
            if(previouslyDraggedTile != tile)
            {
                if (previouslyDraggedTile != null) previouslyDraggedTile.CancelBuildTarget();
                if (tile != null) tile.SetAsBuildTarget();

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
		// TowerScript ts = draggedTowerInstance.GetComponent<TowerScript>();

		ChangeDrawSorting(draggedTowerInstance, "GUI", 3);
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
		towerOffset.y = 1.0f;
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

	void PlaceOrReturnTower() {
		if (draggedTowerInstance == null) {
			Debug.LogError("Nothing to place");
			return;
		}
		draggedTowerPrefab = null;


		// TODO return tower
		if (isOverUI.getOverCount(Input.mousePosition) > 0) {
			ReturnTower();
			return;
		}

		Tile tile = tileset.GetTileAt(InputUtils.WorldMousePosition());
		if (tile == null) {
			ReturnTower();
		} else if (tile.CanBuild()) {
			ChangeDrawSorting(draggedTowerInstance, "Buildings", 1);
			draggedTowerInstance.transform.DOMove(tile.transform.position, .2f)
				.SetEase(Ease.InOutFlash)
				.OnComplete(()=>{
					Sounds.PlayTowerBuild();
					tile.CancelBuildTarget();
					tile.Build(draggedTowerInstance);
					gameManager.TowerBuild(draggedTowerOwner, draggedTowerInstance);
					GameObject dust = GameObject.Instantiate(towerDust, draggedTowerInstance.transform.position, Quaternion.identity, draggedTowerInstance.transform);
                    dust.transform.localPosition = new Vector3(0, -0.1f, 0);

					previouslyDraggedTile = null;
					draggedTowerInstance = null;
					dragging = false;
					draggedTowerOwner = null;		
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
		ChangeDrawSorting(tower, "GUI", 3);
		TowerScript ts = tower.GetComponent<TowerScript>();
		bool wasAttached = ts.DetachFromTile();
		if (wasAttached) {
			GameObject.Instantiate(towerExplosion, tower.transform.position, Quaternion.identity, towerContainer.transform);
				tower.transform.DOMove(button.transform.position, .5f)
				.SetEase(Ease.InOutSine)
				.OnComplete(()=>{
					button.ReturnTower();
					if (lockButton) {
						button.Lock();
					}
					GameObject.Destroy(tower);	
				}
			);
		} else {
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
}
