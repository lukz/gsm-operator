using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerSpawnerPro : MonoBehaviour {
	public GameObject towerContainer;
	public GameObject[] towerPrefabs = new GameObject[3];
	
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
	void Update () {
		if (!GameManager.canDoActions) return;

		Vector3 pos = InputUtils.WorldMousePosition();
		// we are actively dragging the tower
		if (dragging) {
			dragTime += Time.deltaTime;
			draggedTowerInstance.transform.position = new Vector2(pos.x + towerOffset.x, pos.y + towerOffset.y);
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

		GameManager.instance.takeTower.Play();

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
		GameManager.instance.towerBuilt.Play();

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

		if (isOverUI.getOverCount(Input.mousePosition) > 0) {
			GameObject.Destroy(draggedTowerInstance);
			draggedTowerInstance = null;
			return;
		}

		// GameObject.Destroy(draggedTowerInstance);
		// draggedTowerInstance = null;
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
}
