using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class TowerSpawner : MonoBehaviour {

	GameObject spawned;
	GameObject towerToSpawn;
	public static int currentChosenTower;

	GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

	// Use this for initialization
	void Start () {
		//Fetch the Raycaster from the GameObject (the Canvas)
		Canvas canvas = FindObjectOfType<Canvas>();
        raycaster = canvas.GetComponent<GraphicRaycaster>();
		Debug.Log("raycaster " + raycaster);
        //Fetch the Event System from the Scene
        eventSystem = FindObjectOfType<EventSystem>();
		Debug.Log("events " + eventSystem);
	}
	
	// Update is called once per frame
	void Update () {
		if (!spawned) return;
		Transform transform = spawned.GetComponent<Transform>();
		Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
		transform.position = pos;
		
		if (Input.GetKeyDown(KeyCode.Mouse0)) { // left
			// if we click on gui, ignore it
			if (getOverCount() > 0) return;
			GameManager.currentTowers[currentChosenTower]--;
			GameManager.UpdateNumbers();


			// make it opaque
			SpriteRenderer sr = spawned.GetComponentInChildren<SpriteRenderer>();
			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
			spawned = null;
			towerToSpawn = null;
			// TODO perhaps we need to enable the thing or something
		} else if (Input.GetKeyDown(KeyCode.Mouse1)) { // right	
			// cancel spawning
			if (spawned) {
				GameObject.Destroy(spawned);
			}
			spawned = null;
			towerToSpawn = null;
		}
	}

	int getOverCount() {
		//Set up the new Pointer Event
		pointerEventData = new PointerEventData(eventSystem);
		//Set the Pointer Event Position to that of the mouse position
		pointerEventData.position = Input.mousePosition;

		//Create a list of Raycast Results
		List<RaycastResult> results = new List<RaycastResult>();

		//Raycast using the Graphics Raycaster and mouse click position
		raycaster.Raycast(pointerEventData, results);

		//For every result returned, output the name of the GameObject on the Canvas hit by the Ray
		// foreach (RaycastResult result in results)
		// {
		// 	Debug.Log("Hit " + result.gameObject.name);
		// }
		return results.Count;
	}

	public void spawn(GameObject tower) {
		Debug.Log("Spawn stuff maybe?");

		if (spawned) {
			GameObject.Destroy(spawned);
		}
		if (tower == towerToSpawn) {
			towerToSpawn = null;
			return;
		}
		towerToSpawn = tower;

		Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
		Transform parent = GetComponent<Transform>();
		spawned = GameObject.Instantiate(tower, pos, Quaternion.identity, parent);
		// make it semi transparent 
		SpriteRenderer sr = spawned.GetComponentInChildren<SpriteRenderer>();
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, .5f);
	}
}
