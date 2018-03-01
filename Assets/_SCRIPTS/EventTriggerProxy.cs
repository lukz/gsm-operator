using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventTriggerProxy : MonoBehaviour {
	private GameObject towerPrefab;
	private TowerSpawnerPro towerSpawnerPro;
	public Image towerImage;
	public SpriteRenderer gateSprite;
	bool locked = true;

	private float enableImageDelay = .2f;
	void Start()
    {
		towerSpawnerPro = FindObjectOfType<TowerSpawnerPro>();
		if (!towerSpawnerPro) {
			Debug.LogError("TowerSpawnerPro missing!");
		}

		if (!towerImage) {
			Debug.LogError("Image missing!");
		}
		if (!gateSprite) {
			Debug.LogError("Gate missing!");
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

	public void SetTowerPrefab(GameObject prefab) {
		towerPrefab = prefab;
		Button button = GetComponent<Button>();
		if (prefab != null) {
			GameObject body = prefab.transform.Find("Body").gameObject;
			SpriteRenderer spriteRenderer = body.GetComponent<SpriteRenderer>();
			towerImage.sprite = spriteRenderer.sprite;
			towerImage.enabled = true;
			button.enabled = true;
		} else {
			towerImage.enabled = false;
			button.enabled = false;
		}
	}

	private float lockTimer;
	void Update() {
		if (locked) {
			if (lockTimer > 0) {
				lockTimer -= Time.deltaTime;
				float a = 1- Mathf.Clamp01(lockTimer/.5f);
				gateSprite.color = new Color(1, 1, 1, a);
			}
		} else {
			if (lockTimer > 0) {
				lockTimer -= Time.deltaTime;
				float a = Mathf.Clamp01(lockTimer/.5f);
				gateSprite.color = new Color(1, 1, 1, a);
			}
		}
	}

    void PickTower(PointerEventData data)
    {
		if (locked) return;
		Button button = GetComponent<Button>();
		if (!button.enabled) return;
        Debug.Log("PickTower called " + towerPrefab);
		if (towerSpawnerPro != null && towerPrefab != null) {
			towerSpawnerPro.PickTower(this, towerPrefab);
			button.enabled = false;
		}
		if (towerImage != null ) {
			towerImage.enabled = false;
		}
    }

	void PlaceTower(PointerEventData data)
    {
        Debug.Log("PlaceTower called "  + towerPrefab);
		if (towerSpawnerPro != null && towerPrefab != null) {
			towerSpawnerPro.PlaceTower(this, towerPrefab);
		}
    }

	public void ReturnTower() {
		if (towerImage != null) {
			towerImage.enabled = true;
		}
		Button button = GetComponent<Button>();
		button.enabled = true;
	}

	public void Unlock() {
		if (towerPrefab == null) return;
		if (!locked) return;
		Debug.Log("Unlock");
		locked = false;
		lockTimer = .5f;
		// TODO hide the graphic
	}

	public void Lock() {
		if (towerPrefab == null) return;
		if (locked) return;
		Debug.Log("Lock");
		locked = true;
		lockTimer = .5f;
		// TODO show the graphic
	}

	public void Reset () {
		ReturnTower();
		Lock();
	}
}
