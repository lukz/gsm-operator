using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerSpawnerPro : MonoBehaviour {
	public GameObject towerContainer;
	public GameObject[] towerPrefabs = new GameObject[3];

	public GameObject towerExplosion;

	public Tileset tileset;
	
	public IsOverUI isOverUI;

	[Range(0f, 1f)]
	public float minDragTime = .2f;

	GameObject draggedTowerPrefab;
	GameObject draggedTowerInstance;
	bool dragging;

	Vector2 towerOffset = new Vector2();

	void Start () {
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

	public void PickTower (GameObject towerPrefab) {
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

		Sounds.PlayTowerTake();

		Transform parent = towerContainer.transform;
		Vector3 pos = InputUtils.WorldMousePosition();
		draggedTowerInstance = GameObject.Instantiate(draggedTowerPrefab, pos, Quaternion.identity, parent);
		TowerScript ts = draggedTowerInstance.GetComponent<TowerScript>();
		ts.isBuilded = false;

		GameObject body = draggedTowerInstance.transform.Find("Body").gameObject;
		SpriteRenderer bodySprite = body.GetComponent<SpriteRenderer>();
		bodySprite.sortingLayerName = "GUI";
		bodySprite.sortingOrder = 3;

		Animator animator = draggedTowerInstance.GetComponent<Animator>();
		animator.SetTrigger("shake");
		// this is in game units
		towerOffset.y = .4f;
	}

	public float PlaceTower (GameObject towerPrefab) {
		if (draggedTowerPrefab == null) {
			Debug.LogError("Nothing to place");
			return .2f;
		}
		if (draggedTowerPrefab != towerPrefab) {
			Debug.LogError("Invalid tower to place " + towerPrefab + ", expected " + draggedTowerPrefab);
			return .2f;
		}
		Debug.Log("PlaceTower");
		Sounds.PlayTowerBuild();

		dragging = false;


		// return -1 for valid spot?
		if (dragTime <= minDragTime) {
			// wait in given spot for a while and do whatever
			StartCoroutine("PlaceOrReturnTowerLater");
			return .2f + minDragTime;
		}
		PlaceOrReturnTower();
		// do whatever
		return .2f;
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
		bodySprite.sortingOrder = 0;

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
        	previouslyDraggedTile = null;
			draggedTowerInstance = null;
			dragging = false;
		} else {
            tile.CancelBuildTarget();
			ReturnTower();
		}

		// GameObject.Destroy(draggedTowerInstance);
		// draggedTowerInstance = null;
	}

	void ReturnTower() {
        GameObject.Destroy(draggedTowerInstance);	
        previouslyDraggedTile = null;
		draggedTowerInstance = null;
		dragging = false;
	}

	bool isValidTowerPrafab(GameObject gameObject) {
		if (!gameObject) return false;
		foreach (var item in towerPrefabs){
			if (item == gameObject) {
				return true;
			}
		}
		return false;
	}

	
	public void DestroyTower(GameObject tower) {
		if (!tower) {
			return;
		}
		TowerScript ts = tower.GetComponent<TowerScript>();
		ts.OnDestroyed();   
		// GameManager.currentTowers[ts.id]++;
		// GameManager.UpdateNumbers();
		if (towerExplosion) {
			Vector3 pos = new Vector3(tower.transform.position.x, tower.transform.position.y, 0);
			GameObject.Instantiate(towerExplosion, pos, Quaternion.identity, gameObject.transform);
		}
		GameObject.Destroy(tower);
	}
}
