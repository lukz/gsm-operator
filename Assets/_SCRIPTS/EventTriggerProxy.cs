using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventTriggerProxy : MonoBehaviour {
	public GameObject towerPrefab;
	private TowerSpawnerPro towerSpawnerPro;
	public Image towerImage;

	
	private float enableImageDelay = .2f;
	void Start()
    {
		if (!towerPrefab) {
			Debug.LogError("TowerPrefab missing!");
		}
		towerSpawnerPro = FindObjectOfType<TowerSpawnerPro>();
		if (!towerSpawnerPro) {
			Debug.LogError("TowerSpawnerPro missing!");
		}

		if (!towerImage) {
			Debug.LogError("Image missing!");
		}

        EventTrigger trigger = GetComponent<EventTrigger>();
        {
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback.AddListener((data) => { 
				PickTower((PointerEventData)data); 
				PlaceTower((PointerEventData)data);
			});
			trigger.triggers.Add(entry);
		}
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.BeginDrag;
			entry.callback.AddListener((data) => { 
				PickTower((PointerEventData)data);
			});
			trigger.triggers.Add(entry);
		}
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.EndDrag;
			entry.callback.AddListener((data) => { 
				PlaceTower((PointerEventData)data);
			});
			trigger.triggers.Add(entry);
		}
    }

    void PickTower(PointerEventData data)
    {
        Debug.Log("PickTower called " + towerPrefab);
		if (towerSpawnerPro && towerPrefab) {
			towerSpawnerPro.PickTower(towerPrefab);
		}
		if (towerImage) {
			towerImage.enabled = false;
		}
    }

	void PlaceTower(PointerEventData data)
    {
        Debug.Log("PlaceTower called "  + towerPrefab);
		if (towerSpawnerPro && towerPrefab) {
			enableImageDelay = towerSpawnerPro.PlaceTower(towerPrefab);
		}
		StartCoroutine("EnableTowerImage");
		
    }

	IEnumerator EnableTowerImage () {
		yield return new WaitForSeconds(enableImageDelay);
		if (towerImage) {
			towerImage.enabled = true;
		}
		yield return null;
	}
}
