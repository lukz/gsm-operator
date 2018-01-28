using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class TowerSpawner : MonoBehaviour {

	public GameObject destroyIconPrefab;
	GameObject destroyIcon;

	GameObject spawned;
	GameObject towerToSpawn;
	public GameObject towerExplosion;
	public static int currentChosenTower;

	GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

	bool destroy;

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
		Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
		if (destroy) {
			Transform dt = destroyIcon.GetComponent<Transform>();
			dt.position = pos;
			if (Input.GetKeyDown(KeyCode.Mouse0)) { // left
				if (getOverCount() > 0) {
					destroy = false;
					if (destroyIcon) {
						GameObject.Destroy(destroyIcon);
					}	
					return;
				}
				GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
				foreach (GameObject tower in towers)
				{
					Collider2D col = tower.GetComponentInChildren<Collider2D>();
					if (col.OverlapPoint(new Vector2(pos.x, pos.y))) {
						TowerScript ts = tower.GetComponent<TowerScript>();
                        ts.onDestroyed();   
						GameManager.currentTowers[ts.id]++;
						GameManager.UpdateNumbers();
						if (towerExplosion) {
							pos.x = tower.transform.position.x;
							pos.x = tower.transform.position.y - 1;
							GameObject.Instantiate(towerExplosion, pos, Quaternion.identity, gameObject.transform);
						}

                        GameObject.Destroy(tower);
						break;
					}
				}
			} else if (Input.GetKeyDown(KeyCode.Mouse1)) { // right	
				// cancel spawning
				if (destroyIcon) {
					GameObject.Destroy(destroyIcon);
				}
				destroy = false;
			}
			return;
		}

		if (!spawned) return;
		Transform transform = spawned.GetComponent<Transform>();
		transform.position = pos;
		
		if (Input.GetKeyDown(KeyCode.Mouse0)) { // left
			// if we click on gui, ignore it
			if (getOverCount() > 0) {
				if (spawned) {
					GameObject.Destroy(spawned);
				}
				spawned = null;
				towerToSpawn = null;
				return;
			}

			TowerScript ts = spawned.GetComponent<TowerScript>();

			if (!ts.isBuildable) {
				GameManager.instance.deny.Play();
				return;
			}

			GameManager.shakePower += 0.15f;

            
            ts.onBuilded();
			ts.id = currentChosenTower;
			GameManager.currentTowers[currentChosenTower]--;
			GameManager.UpdateNumbers();


            // make it opaque
            //SpriteRenderer sr = spawned.GetComponentInChildren<SpriteRenderer>();
            //sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);

            showForbiddenZones(false);

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
		if (destroyIcon) {
			GameObject.Destroy(destroyIcon);
		}
		destroy = false;

		if (spawned) {
			GameObject.Destroy(spawned);
		}
		if (tower == towerToSpawn) {
			towerToSpawn = null;
			return;
		}
		towerToSpawn = tower;
		GameManager.instance.btnClick.Play();
		Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
		Transform parent = GetComponent<Transform>();
		spawned = GameObject.Instantiate(tower, pos, Quaternion.identity, parent);
		TowerScript ts = spawned.GetComponent<TowerScript>();
		ts.isBuilded = false;
        // make it semi transparent 
        //SpriteRenderer sr = spawned.GetComponentInChildren<SpriteRenderer>();
        //sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        showForbiddenZones(true);
    }

	public void despawn() {
		Debug.Log("destroy stuff?");
		destroy = !destroy;
		if (destroy) {
			GameManager.instance.btnClick.Play();
			Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
			Transform parent = GetComponent<Transform>();
			destroyIcon = GameObject.Instantiate(destroyIconPrefab, pos, Quaternion.identity, parent);
		} else {
			// if (destroyIcon) {
			// 	GameObject.Destroy(destroyIcon);
			// }	
		}
	}

    public void showForbiddenZones(bool show)
    {
        GameObject[] forbiddenZones = GameObject.FindGameObjectsWithTag("Forbidden");
        for (int i = 0; i < forbiddenZones.Length; i++)
        {
            SpriteRenderer sr = forbiddenZones[i].GetComponentInChildren<SpriteRenderer>();

            if(show)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
            }
            else
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
            }
            
        }

    }
}
