using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerSpawnerPro : MonoBehaviour {
	public GameObject towerContainer;
	public GameObject[] towerPrefabs = new GameObject[3];

	public GameObject towerExplosion;

	private GameManager gameManager;


	public Tileset tileset;
	
	public IsOverUI isOverUI;

	[Range(0f, 1f)]
	public float minDragTime = .2f;

	GameObject draggedTowerPrefab;
	GameObject draggedTowerInstance;
	EventTriggerProxy draggedTowerOwner;
	bool dragging;

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

		GameObject body = draggedTowerInstance.transform.Find("Body").gameObject;
		SpriteRenderer bodySprite = body.GetComponent<SpriteRenderer>();
		bodySprite.sortingLayerName = "GUI";
		bodySprite.sortingOrder = 3;

		Animator animator = draggedTowerInstance.GetComponent<Animator>();
		animator.SetTrigger("shake");
		// this is in game units
		towerOffset.y = .4f;
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
		Sounds.PlayTowerBuild();

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

		GameObject body = draggedTowerInstance.transform.Find("Body").gameObject;
		SpriteRenderer bodySprite = body.GetComponent<SpriteRenderer>();
		bodySprite.sortingLayerName = "Buildings";
		bodySprite.sortingOrder = 1;

		// TODO return tower
		if (isOverUI.getOverCount(Input.mousePosition) > 0) {
			ReturnTower();
			return;
		}

		Tile tile = tileset.GetTileAt(InputUtils.WorldMousePosition());
		if (tile == null) {
			ReturnTower();
		} else if (tile.CanBuild()) {
            tile.CancelBuildTarget();
			tile.Build(draggedTowerInstance);
			gameManager.TowerBuild(draggedTowerOwner, draggedTowerInstance);
        	previouslyDraggedTile = null;
			draggedTowerInstance = null;
			dragging = false;
			draggedTowerOwner = null;
		} else {
            tile.CancelBuildTarget();
			ReturnTower();
		}

		// GameObject.Destroy(draggedTowerInstance);
		// draggedTowerInstance = null;
	}

	public void ReturnTower() {
		if (previouslyDraggedTile == null) return;
        GameObject.Destroy(draggedTowerInstance);	
        previouslyDraggedTile = null;
		draggedTowerInstance = null;
		draggedTowerOwner.ReturnTower();
		draggedTowerOwner = null;
	}

	public void ReturnTower(EventTriggerProxy button, GameObject tower) {
		tower.GetComponent<TowerScript>().DetachFromTile();
		button.ReturnTower();
        GameObject.Destroy(tower);	
		draggedTowerOwner = null;
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
