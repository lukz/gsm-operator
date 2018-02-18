using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVLsettings : MonoBehaviour {
	public int level;
	public string levelName;
	public List<TowerSet> tiers;

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
