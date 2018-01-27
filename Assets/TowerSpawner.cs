using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour {

	GameObject spawned;
	GameObject towerToSpawn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// Vector3 pos = new Vector3();
		
		// pos.Set();
		
		// Debug.Log(pos);
		if (!spawned) return;
		Transform transform = spawned.GetComponent<Transform>();
		Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
		transform.position = pos;

		if (Input.GetKeyDown(KeyCode.Mouse0)) { // left
			SpriteRenderer sr = spawned.GetComponentInChildren<SpriteRenderer>();
			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
			spawned = null;
			towerToSpawn = null;
		} else if (Input.GetKeyDown(KeyCode.Mouse1)) { // right	
			if (spawned) {
				GameObject.Destroy(spawned);
			}
			spawned = null;
			towerToSpawn = null;
		}
	}

	public void spawn(GameObject tower) {
		Debug.Log("Spawn stuff maybe?");

		if (spawned) {
			GameObject.Destroy(spawned);
		}
		if (tower == towerToSpawn) return;
		towerToSpawn = tower;

		Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
		Transform parent = GetComponent<Transform>();
		spawned = GameObject.Instantiate(tower, pos, Quaternion.identity, parent); 
		SpriteRenderer sr = spawned.GetComponentInChildren<SpriteRenderer>();
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, .5f);
	}
}
