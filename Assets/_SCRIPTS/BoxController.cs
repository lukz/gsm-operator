﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {
	// slot 0 is the end slot
	public Transform[] slots = new Transform[6];

	public GameObject boxPrefab;
	public List<TowerBox> allBoxes = new List<TowerBox>();
	public List<TowerBox> boxes = new List<TowerBox>();

	public GameManager gameManager;

	public SpriteRenderer beltRenderer;

	public Sprite[] beltFrames;


	int moving = 0;
	int nextFrame = 0;

	void Start () {
		
	}

	float timer;

	[SerializeField]
	float frameDuration = .1f;
	void Update () {
		if (moving != 0) {
			timer += Time.deltaTime;
			if (timer >= frameDuration) {
				timer -= frameDuration;
				nextFrame += moving;
				if (nextFrame < 0) nextFrame = beltFrames.Length - 1;
				if (nextFrame >= beltFrames.Length) nextFrame = 0;
				beltRenderer.sprite = beltFrames[nextFrame];
			}
		}
	}

	void InitBoxes() {
		boxes.Clear();
		if (allBoxes.Count == 0) {
			for (int i = 0; i < 5; i++)
			{
				Transform slot = slots[i + 1];
				GameObject inst = GameObject.Instantiate(boxPrefab, this.transform);
				TowerBox box = inst.GetComponent<TowerBox>();
				allBoxes.Add(box);
			}
		}
		foreach(var box in allBoxes) {
			box.SetPrefab(null);
		}
	}

	public void Restart (LVLsettings settings) {
		InitBoxes();
		gameManager.towerButton.SetTowerPrefab(null);
		Debug.Log("Start BoxManager");
		

		List<GameObject> prefabs = new List<GameObject>();

		for (int i = 0; i < 5; i++)
		{
			GameObject tp = settings.TowerPrefab(i);
			if (tp != null) {
				prefabs.Add(tp);
			}
		}


		for (int i = 0; i < prefabs.Count; i++)
		{
			Transform slot = slots[i + 1];
			TowerBox box = allBoxes[i];
			box.Init(this, i + 1);
			box.SetPrefab(prefabs[i]);
			boxes.Add(box);
		}
	}

	public Vector3 GetSlotPosition(int slotId) {
		return slots[slotId].position;
	}

	private void StopMoving(TowerBox tb) {
		moving = 0;
	}

	public void NextTower() {
		Debug.Log("NextTower");
		bool anyMoved = false;
		foreach(var box in boxes) {
			if (box.SlotId -1 >= 0) {
				anyMoved = true;
			}
			if (box.SlotId - 1 == 0) {
				box.MoveToSlot(box.SlotId - 1, 0, (tb) => {
					gameManager.towerButton.SetTowerPrefab(tb.GetPrefab(), true);
                    gameManager.towerButton.Unlock();
                    moving = 0;
				});
			} else {
				box.MoveToSlot(box.SlotId - 1, 0, StopMoving);
			}
		}
		if (anyMoved) {
			moving = 1;
		}
	}
	public void PrevTower() {
		Debug.Log("PrevTower");
		// cant go up if already at last box
		if (boxes[boxes.Count -1].SlotId >= boxes.Count){
			return;
		}
		moving = -1;
		foreach(var box in boxes) {
			box.MoveToSlot(box.SlotId + 1, 0, StopMoving);
		}	
		gameManager.towerButton.SetTowerPrefab(null);
	}

	public void ReturnTower() {
		Debug.Log("ReturnTower");
		foreach(var box in boxes) {
			if (box.SlotId == 0) {
				gameManager.towerButton.SetTowerPrefab(box.GetPrefab());
				gameManager.towerButton.Unlock();
			}
		}

	}
}
