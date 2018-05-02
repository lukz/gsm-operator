using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public int powerLvl = 0;

    //public GameObject[] objectsHere;

    // private Color normalColor = new Color(0xFF, 0xFF, 0xFF, 0f);
    // private Color blockedColor = new Color(0xFF, 0x00, 0x00, 0xD0);
    // private Color poweredColor = new Color(0x00, 0xB5, 0xFF, 0xC0);
    // private Color powered2Color = new Color(0x60, 0x90, 0xFF, 0xC0);
    // private Color powered3Color = new Color(0x00, 0x00, 0x0, 0xC0);
	// private Color targetColor = new Color(0x0, 0xFF, 0x00, 0xFD);

    public int x;
    public int y;

    public PowerMarker powerMarker;
    public BuildMarker buildMarker;


	[SerializeField]
	private Sprite hookLvl0;

	[SerializeField]
	private Sprite hookLvl1;

	[SerializeField]
	private Sprite hookLvl2;

	[SerializeField]
	private Sprite hookLvl3;

	[SerializeField]
	private Sprite hookBad;

	private List<TowerScript.PowerOffset> towerPowerOffsets;

    private Tileset tileset;

    public Tileset Tileset {
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
    }

	private void Awake()
	{
		_spriterenderer = GetComponent<SpriteRenderer>();
		_spriterenderer.enabled = true;
	}

	void Start () {
		ChangeHooks();

		// GetComponent<SpriteRenderer>().enabled = false;
		GameObject house = GetHouse();
        if (house != null) {
            HouseScript hs = house.GetComponent<HouseScript>();
            powerMarker.SetRequiredPower(hs.requiredPower);
			_spriterenderer.enabled = false;

		} else {
			if (HasRocks()) _spriterenderer.enabled = false;
			if(HasTower()) _spriterenderer.enabled = false;
			
			powerMarker.SetRequiredPower(0);
           // powerMarker.SetPower(powerLvl);
        }
	}

    public bool HasEnergyField()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).transform.tag == "EnergyField") return true;
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
            if (transform.GetChild(i).transform.tag == "Blocker") return true;
        }

        return false;
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

    private delegate void DoTile (Tile parameter);
    public void SetAsBuildTarget(List<TowerScript.PowerOffset> powerOffsets)
    {
        buildMarker.OverBuild(CanBuild());
        if (towerPowerOffsets != null) {
            CancelBuildTarget();
        }
        towerPowerOffsets = powerOffsets;
        if (towerPowerOffsets != null) {
            DoOverOffsetTiles(t => {
                t.buildMarker.StartWillPowerUp(!CanBuild());
            });
        }
    }
    public void CancelBuildTarget()
    {
        buildMarker.CancelOverBuild(CanBuild());
    
        if (towerPowerOffsets != null) {
            DoOverOffsetTiles(t => {
                t.buildMarker.CancelWillPowerUp();
            });
            towerPowerOffsets = null;
        }
    }

    private void DoOverOffsetTiles (DoTile doTile) 
    {
        if (towerPowerOffsets != null) {
            int bx = tileset.GridX(transform.position.x);
            int by = tileset.GridY(transform.position.y);
            foreach (var offset in towerPowerOffsets) {
                int tx = bx + offset.x;
                int ty = by - offset.y;
                Tile at = tileset.GetTileAt(tx, ty);
                if (at != null && at.gameObject.activeInHierarchy) {
                    doTile(at);
                }
            }
        }
    }
    public void Build(GameObject tower)
    {
        tower.transform.parent = transform;
        tower.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        tower.GetComponent<TowerScript>().AttachToTile(this);
		buildMarker.CancelBuild();
		_spriterenderer.enabled = false;
	}

    public bool CanBuild()
    {
        return !IsBlocked() && (HasEnergyField() || powerLvl > 0);
    }

    public void PowerChange(TowerScript source, int powerChange)
    {
		// Debug.Log("Tile#[" + x + ", " + y + "] power change " + source.GetInstanceID());
        powerLvl += powerChange;
        if (powerLvl < 0) {
            Debug.Log("This sohuld not happen " + powerLvl + " -> " + source);
            powerLvl = 0;
        }
        powerMarker.SetPower(powerLvl);
        OnPowerChange(source, powerChange);

		ChangeHooks();
    }

	private void ChangeHooks()
	{
		switch (powerLvl)
		{
			case 0:
				_spriterenderer.sprite = hookBad;
				if (HasEnergyField()) _spriterenderer.sprite = hookLvl0;
				break;
			case 1:
				_spriterenderer.sprite = hookLvl1;
				break;
			case 2:
				_spriterenderer.sprite = hookLvl2;
				break;
			case 3:
				_spriterenderer.sprite = hookLvl3;
				break;
		}
	}

    private void OnPowerChange(TowerScript source, int powerChange)
    {
        HouseScript[] hss = GetComponentsInChildren<HouseScript>();
        foreach (var hs in hss) 
        {
            hs.PowerChanged();
            MineScript ts = hs.gameObject.GetComponent<MineScript>();
            if (ts != null) {
                ts.PowerChange(source, powerChange);
            }
        }
    }

    public override string ToString() {
        return "Tile{["+x + "," + y+"], active="+gameObject.activeInHierarchy+"}";
    }
}
