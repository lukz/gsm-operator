﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
	private HouseScript houseScript;
	private TowerScript towerScript;

	private List<MineScript> poweredBy = new List<MineScript>();
	private List<MineScript> powers = new List<MineScript>();

	void Start()
	{
		houseScript = GetComponent<HouseScript>();
		towerScript = GetComponent<TowerScript>();
	}
	public void PowerChange(TowerScript source, int power, int tilePower)
	{
		// Debug.Log("Mine(" + GetInstanceID() + ") Power change " + this.powerLvl + " + " + powerChange);
		// the general idea is to disallow power up loops
		// if we power up a mine, we dont want to be power back by it		
	houseScript.powered = false;
		if (!source)
		{
			if (tilePower != 0)
				houseScript.powered = true;
		}
		else
		{
			MineScript other = source.gameObject.GetComponent<MineScript>();
			if (other != null)
			{
				// other mine wants to power us, check if we power it first
				if (power < 0)
				{
					// Debug.Log("(" + GetInstanceID() + " Power removed " 
					// + this.powerLvl + " -> " + powerChange + " from " + other + "("+other.GetInstanceID()+")");
					poweredBy.Remove(other);
					other.powers.Remove(this);
				}
				else if (power > 0)
				{
					// make sure we dont make a chain
					if (Chained(other))
					{
						return;
					}
					// Debug.Log("(" + GetInstanceID() + " Power added " 
					// + this.powerLvl + " -> " + powerChange + " from " + other + "("+other.GetInstanceID()+")");
					poweredBy.Add(other);
					other.powers.Add(this);
				}
			}
			houseScript.powered = true;
			towerScript.PowerUp();
		}
	}

	private bool Chained(MineScript other)
	{
		if (powers.Contains(other))
		{
			return true;
		}
		foreach (var ms in powers)
		{
			if (ms.Chained(other))
			{
				return true;
			}
		}
		return false;
	}
}
