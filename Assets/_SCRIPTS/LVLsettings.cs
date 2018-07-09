using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVLsettings : MonoBehaviour {
	public int level;
	public string levelName;

	[Range(-1, 2)]
	public int towerPrefab1 = -1;

	[Range(-1, 2)]
	public int towerPrefab2 = -1;

	[Range(-1, 2)]
	public int towerPrefab3 = -1;

	[Range(-1, 2)]
	public int towerPrefab4 = -1;

	[Range(-1, 2)]
	public int towerPrefab5 = -1;

	
	public GameObject[] towerPrefabs;

	public List<Tile> tiles;

	[HideInInspector]
    public int allTowers = 0;

	void Awake () {

		EventTriggerProxy[] list = GameObject.FindGameObjectWithTag("MainCamera").GetComponentsInChildren<EventTriggerProxy>();

		// list[0].SetTowerPrefab(towerPrefab1);
		// list[1].SetTowerPrefab(towerPrefab2);
		// list[2].SetTowerPrefab(towerPrefab3);
		// list[3].SetTowerPrefab(towerPrefab4);
		// list[4].SetTowerPrefab(towerPrefab5);

        allTowers = 0;

        if (towerPrefab1 != -1) allTowers++;
        if (towerPrefab2 != -1) allTowers++;
        if (towerPrefab3 != -1) allTowers++;
        if (towerPrefab4 != -1) allTowers++;
        if (towerPrefab5 != -1) allTowers++;
	}

	public GameObject TowerPrefab(int id) {
		switch (id)
		{
			case 0: return towerPrefab1 != -1? towerPrefabs[towerPrefab1]:null;
			case 1: return towerPrefab2 != -1? towerPrefabs[towerPrefab2]:null;
			case 2: return towerPrefab3 != -1? towerPrefabs[towerPrefab3]:null;
			case 3: return towerPrefab4 != -1? towerPrefabs[towerPrefab4]:null;
			case 4: return towerPrefab5 != -1? towerPrefabs[towerPrefab5]:null;
			default:
			return null;
		}
	}

	
}

