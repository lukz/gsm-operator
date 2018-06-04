using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVLsettings : MonoBehaviour {
	public int level;
	public string levelName;

	public GameObject towerPrefab1;
	public GameObject towerPrefab2;
	public GameObject towerPrefab3;
	public GameObject towerPrefab4;
	public GameObject towerPrefab5;

	public List<Tile> tiles;

	[HideInInspector]
    public int allTowers = 0;

	void Awake () {


		// trzeba patrzec co ginie na zmiane sceny, nigdy nie robi sie referencji do stalego elementu, 
		// od elementow ktore dostajesz od nowej sceny, bo referencje ZAWSZE sa nullami wtedy.
		// Albo wszystko dontDelete albo nic. Jak pomiedzy to trzeba szukac po tagach.
		EventTriggerProxy[] list = GameObject.FindGameObjectWithTag("MainCamera").GetComponentsInChildren<EventTriggerProxy>();

		// list[0].SetTowerPrefab(towerPrefab1);
		// list[1].SetTowerPrefab(towerPrefab2);
		// list[2].SetTowerPrefab(towerPrefab3);
		// list[3].SetTowerPrefab(towerPrefab4);
		// list[4].SetTowerPrefab(towerPrefab5);

        allTowers = 0;

        if (towerPrefab1 != null) allTowers++;
        if (towerPrefab2 != null) allTowers++;
        if (towerPrefab3 != null) allTowers++;
        if (towerPrefab4 != null) allTowers++;
        if (towerPrefab5 != null) allTowers++;
    }

	
}

