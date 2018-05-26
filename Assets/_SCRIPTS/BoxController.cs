using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {
	// slot 0 is the end slot
	public Transform[] slots = new Transform[6];

	public GameObject boxPrefab;

	public List<TowerBox> boxes = new List<TowerBox>();

	public GameManager gameManager;
	
	int currentId;

	void Start () {
		
	}

	void InitBoxes() {
		if (boxes.Count == 0) {
			for (int i = 0; i < 5; i++)
			{
				Transform slot = slots[i + 1];
				GameObject inst = GameObject.Instantiate(boxPrefab, this.transform);
				TowerBox box = inst.GetComponent<TowerBox>();
				boxes.Add(box);
			}
		}
		foreach(var box in boxes) {
			box.SetPrefab(null);
		}
	}

	public void Restart () {
		InitBoxes();
		Debug.Log("Start BoxManager");
		

		LVLsettings settings = FindObjectOfType<LVLsettings>();
		List<GameObject> prefabs = new List<GameObject>();

		if (settings.towerPrefab1 != null) prefabs.Add(settings.towerPrefab1);
        if (settings.towerPrefab2 != null) prefabs.Add(settings.towerPrefab2);
        if (settings.towerPrefab3 != null) prefabs.Add(settings.towerPrefab3);
        if (settings.towerPrefab4 != null) prefabs.Add(settings.towerPrefab4);
        if (settings.towerPrefab5 != null) prefabs.Add(settings.towerPrefab5);


		for (int i = 0; i < prefabs.Count; i++)
		{
			Transform slot = slots[i + 1];
			TowerBox box = boxes[i];
			box.Init(this, i + 1);
			box.SetPrefab(prefabs[i]);
		}
	}

	public Vector3 GetSlotPosition(int slotId) {
		return slots[slotId].position;
	}

	public void NextTower() {
		Debug.Log("NextTower");
		foreach(var box in boxes) {
			if (box.SlotId - 1 == 0) {
				box.MoveToSlot(box.SlotId - 1, 0, (tb) => {
					gameManager.towerButton.SetTowerPrefab(box.GetPrefab(), true);
				});
			} else{
				box.MoveToSlot(box.SlotId - 1, 0);
			}
		}
	}
	public void PrevTower() {
		Debug.Log("PrevTower");
		// cant go up if already at last box
		if (boxes[boxes.Count -1].SlotId >= 5){
			return;
		}
		foreach(var box in boxes) {
			box.MoveToSlot(box.SlotId + 1, 0);
		}	
		gameManager.towerButton.SetTowerPrefab(null);
	}

	public void ReturnTower() {
		Debug.Log("ReturnTower");
		foreach(var box in boxes) {
			if (box.SlotId == 0) {
				gameManager.towerButton.SetTowerPrefab(box.GetPrefab());
			}
		}
	}
}
