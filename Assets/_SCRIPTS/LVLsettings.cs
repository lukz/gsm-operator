﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVLsettings : MonoBehaviour {
	public int level;
	public string levelName;
	public List<TowerSet> tiers;

	public GameObject towerPrefab1;
	public GameObject towerPrefab2;
	public GameObject towerPrefab3;
	public GameObject towerPrefab4;
	public GameObject towerPrefab5;

	public EventTriggerProxy towerButton1;
	public EventTriggerProxy towerButton2;
	public EventTriggerProxy towerButton3;
	public EventTriggerProxy towerButton4;
	public EventTriggerProxy towerButton5;
	public List<Tile> tiles;

	
	void Start () {
		towerButton1.SetTowerPrefab(towerPrefab1);
		towerButton2.SetTowerPrefab(towerPrefab2);
		towerButton3.SetTowerPrefab(towerPrefab3);
		towerButton4.SetTowerPrefab(towerPrefab4);
		towerButton5.SetTowerPrefab(towerPrefab5);
	}

	public int GetSphereCount(int tier) {
		if (tier < 0 || tier >= tiers.Count) return 0;
		return tiers[tier].sphereCount;
	}

	public int GetConeCount(int tier) {
		if (tier < 0 || tier >= tiers.Count) return 0;
		return tiers[tier].coneCount;
	}

	public int GetRayCount(int tier) {
		if (tier < 0 || tier >= tiers.Count) return 0;
		return tiers[tier].rayCount;
	}


}

