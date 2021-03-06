﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

	public int powerLvl = 0;


	public int x;
	public int y;

	public BuildMarker buildMarker;



	private GameObject resource;
	public GameObject resourceFlash;

	private List<TowerScript.PowerOffset> towerPowerOffsets;

	private Tileset tileset;

	public Tileset Tileset
	{
		get { return tileset; }
		set { tileset = value; }
	}

	private SpriteRenderer _spriterenderer;

	// Use this for initialization

	public void Init(Tileset tileset, int x, int y)
	{
		Tileset = tileset;
		this.x = x;
		this.y = y;
		powerLvl = 0;
		_spriterenderer = GetComponent<SpriteRenderer>();
		_spriterenderer.enabled = true;

		if (HasEnergyField()) PowerChange(null, 1);
		if (HasWaterField()) PowerChange(null, -1);
		if (HasHouse()) _spriterenderer.enabled = false;
		if (HasRocks()) _spriterenderer.enabled = false;
		if (HasTower()) _spriterenderer.enabled = false;
	}

	private void Awake()
	{
		_spriterenderer = GetComponent<SpriteRenderer>();
		_spriterenderer.enabled = true;
		if (HasHouse()) _spriterenderer.enabled = false;
		if (HasRocks()) _spriterenderer.enabled = false;
		if (HasTower()) _spriterenderer.enabled = false;


	}

	void Start()
	{

		GameObject house = GetHouse();
		if (house != null)
		{
			HouseScript hs = house.GetComponent<HouseScript>();
			_spriterenderer.enabled = false;

		}
		else
		{
			if (HasRocks()) _spriterenderer.enabled = false;
			if (HasTower()) _spriterenderer.enabled = false;
		}
	}

	public bool HasEnergyField()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject temp = transform.GetChild(i).gameObject;
			if (temp.transform.tag == "EnergyField")
			{
				resource = temp;
				return true;
			}
		}

		return false;
	}
	public bool HasWaterField()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject temp = transform.GetChild(i).gameObject;
			if (temp.transform.tag == "WaterField")
			{
				resource = temp;
				return true;
			}
		}

		return false;
	}

	bool HasTower()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).transform.tag == "Tower") return true;
		}

		return false;
	}

	public GameObject GetTower()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).transform.tag == "Tower") return transform.GetChild(i).gameObject;
		}

		return null;
	}

	public MineScript GetMine()
	{
		GameObject mine = GetTower();
		if (mine != null)
		{
			return mine.GetComponent<MineScript>();
		}
		return null;
	}

	bool HasHouse()
	{
		if (GetHouse() != null) return true;
		return false;
	}

	public GameObject GetHouse()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).transform.tag == "House") return transform.GetChild(i).gameObject;
		}

		return null;
	}

	bool HasRocks()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).transform.tag == "Blocker")
			{
				return true;
			}
		}

		return false;
	}
	public GameObject GetEnemy()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform t = transform.GetChild(i);
			if (t.transform.tag == "Blocker")
			{
				// check if they are on or something?
				EnemyCrystal rocks = t.gameObject.GetComponent<EnemyCrystal>();
				if (rocks != null)
				{
					return t.gameObject;
				}
			}
		}
		return null;
	}

	bool IsBlocked()
	{
		return HasRocks() || HasHouse() || HasTower();
	}

	public void StartBuilding()
	{
		if (gameObject.activeInHierarchy)
		{
			buildMarker.StartBuild(CanBuild());
		}
	}

	public void CancelBuilding()
	{
		if (gameObject.activeInHierarchy)
		{
			buildMarker.CancelBuild();
		}
	}

	private delegate void DoTile(Tile parameter);
	public void SetAsBuildTarget(List<TowerScript.PowerOffset> powerOffsets)
	{
		buildMarker.OverBuild(CanBuild());
		if (towerPowerOffsets != null)
		{
			CancelBuildTarget();
		}
		towerPowerOffsets = powerOffsets;
		if (towerPowerOffsets != null)
		{
			DoOverOffsetTiles(t =>
			{
				t.buildMarker.StartWillPowerUp(!CanBuild());
			});
		}
	}
	public void CancelBuildTarget()
	{
		buildMarker.CancelOverBuild(CanBuild());

		if (towerPowerOffsets != null)
		{
			DoOverOffsetTiles(t =>
			{
				t.buildMarker.CancelWillPowerUp();
			});
			towerPowerOffsets = null;
		}
	}

	private void DoOverOffsetTiles(DoTile doTile)
	{
		if (towerPowerOffsets != null)
		{
			int bx = tileset.GridX(transform.position.x);
			int by = tileset.GridY(transform.position.y);
			foreach (var offset in towerPowerOffsets)
			{
				int tx = bx + offset.x;
				int ty = by - offset.y;
				Tile at = tileset.GetTileAt(tx, ty);
				if (at != null && at.gameObject.activeInHierarchy)
				{
					doTile(at);
				}
			}
		}
	}
	public float Build(GameObject tower)
	{
		tower.transform.parent = transform;
		tower.transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);

		float delay = tower.GetComponent<TowerScript>().AttachToTile(this);
		buildMarker.CancelBuild();
		_spriterenderer.enabled = false;

        return delay;
	}

	public bool CanBuild()
	{
		return (!IsBlocked() && powerLvl != 0);
	}

	public void PowerChange(TowerScript source, int powerChange, bool fromHistoryNoFX = false)
	{
		if (!Application.isPlaying) return;
		// Debug.Log("Tile#[" + x + ", " + y + "] power change " + source.GetInstanceID());

		if (!HasRocks())
		{
			powerLvl = powerChange;


			if (resource)
			{
				//TODO flash white
				Destroy(resource);
			}
			GameObject temp;
			if (!HasHouse())
			{
				if (powerLvl > 0)
				{
					temp = GameObject.Instantiate(GameManager.instance.CrystalPrefab, transform);
					resource = temp;
				}
				else
				{
					if (powerLvl < 0)
					{
						temp = GameObject.Instantiate(GameManager.instance.WaterPrefab, transform);
						resource = temp;
					}
				}
			}
		}



		OnPowerChange(source, powerChange);
		if (!fromHistoryNoFX)
		{
			if (resourceFlash == null)
			{
				if (powerChange > 0)
				{

					// dont play sound when we just started and spawning stuff
					if (GameManager.instance.timeOnLevel >= 0.1f)
					{
						GameObject powerUpFx = GameObject.Instantiate(GameManager.instance.powerUpFlashEffect);
						powerUpFx.transform.parent = transform;
						powerUpFx.transform.localPosition = new Vector3(0, 0, 0);
						Sounds.PlayPowerUp();
						resourceFlash = powerUpFx;
					}
				}
				else
				{
					if (powerChange < 0)
					{

						// dont play sound when we just started and spawning stuff

						//TODO drugi zasob ikonka animacji FLASH, inny dzwiek?
						if (GameManager.instance.timeOnLevel >= 0.1f)
						{
							GameObject powerUpFx = GameObject.Instantiate(GameManager.instance.waterUpFlashEffect);
							powerUpFx.transform.parent = transform;
							powerUpFx.transform.localPosition = new Vector3(0, 0, 0);
							Sounds.PlayPowerUp();
							resourceFlash = powerUpFx;
						}
					}
				}
			}
			else
			{
				resourceFlash.GetComponent<DestroyOnDone>().time = 1;
				resourceFlash.GetComponent<Animator>().Play("PowerUpTile", 0, 0f);
			}
		}
		else
		{
			HouseScript[] hss = GetComponentsInChildren<HouseScript>();
			foreach (var hs in hss)
			{
				MineScript ts = hs.gameObject.GetComponent<MineScript>();
				if (ts != null)
				{
					ts.PowerChange(null, 0, powerLvl);
				}
			}
		}
	}

	public void PowerChangeDelayed(TowerScript source, int powerChange, float delay)
	{
		StartCoroutine(PowerChangeCorutine(source, powerChange, delay));
	}

	IEnumerator PowerChangeCorutine(TowerScript source, int powerChange, float delay)
	{
		yield return new WaitForSeconds(delay);
		PowerChange(source, powerChange);
	}


	private void OnPowerChange(TowerScript source, int powerChange)
	{
		HouseScript[] hss = GetComponentsInChildren<HouseScript>();
		foreach (var hs in hss)
		{
			MineScript ts = hs.gameObject.GetComponent<MineScript>();
			if (ts != null)
			{
				ts.PowerChange(source, powerChange,powerLvl);
			}
			else
			{
				hs.UpdateResources(powerChange);
			}

		}
	}

	public override string ToString()
	{
		return "Tile{[" + x + "," + y + "], active=" + gameObject.activeInHierarchy + "}";
	}
}
