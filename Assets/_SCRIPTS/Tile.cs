using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public int powerLvl = 0;
    //public GameObject[] objectsHere;

    private Color normalColor = new Color(0xFF, 0xFF, 0xFF, 0.3f);
    private Color blockedColor = new Color(0xFF, 0x00, 0x00, 0xD0);
    private Color poweredColor = new Color(0x00, 0xB5, 0xFF, 0xC0);
    private Color powered2Color = new Color(0x60, 0x90, 0xFF, 0xC0);
    private Color powered3Color = new Color(0x00, 0x00, 0x0, 0xC0);
	private Color targetColor = new Color(0x0, 0xFF, 0x00, 0xFD);

    public int x;
    public int y;

    public PowerMarker powerMarker;

    // Use this for initialization
    void Start () {
        GameObject house = GetHouse();
        if (house != null) {
            HouseScript hs = house.GetComponent<HouseScript>();
            powerMarker.SetRequiredPower(hs.requiredPower);
        } else {
            powerMarker.SetRequiredPower(0);
            powerMarker.SetPower(powerLvl);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
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
    
    public void SetAsBuildTarget()
    {
        if(!CanBuild()) {
            GetComponent<SpriteRenderer>().color = blockedColor;
        } else {
            GetComponent<SpriteRenderer>().color = targetColor;
        }

        
    }

    public void CancelBuildTarget()
    {
        ResetState();
    }

    void ResetState()
    {
        if (powerLvl > 0)
        {
			switch (powerLvl)
			{
				case 1:
					GetComponent<SpriteRenderer>().color = poweredColor;
					break;
				case 2:
					GetComponent<SpriteRenderer>().color = powered2Color;
					break;
				case 3:
					GetComponent<SpriteRenderer>().color = powered3Color;
					break;
			}

        }
        else
        {
            GetComponent<SpriteRenderer>().color = normalColor;
        }
    }

    public void Build(GameObject tower)
    {
        tower.transform.parent = transform;
        tower.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        tower.GetComponent<TowerScript>().AttachToTile(this);
    }

    public bool CanBuild()
    {
        return !IsBlocked() && (HasEnergyField() || powerLvl > 0);
    }

    public void PowerChange(int powerChange)
    {
        powerLvl += powerChange;
        if (powerLvl < 0) powerLvl = 0;
        powerMarker.SetPower(powerLvl);
        OnPowerChange();
    }

    private void OnPowerChange()
    {
        HouseScript[] hss = GetComponentsInChildren<HouseScript>();
        foreach (var hs in hss) 
        {
            hs.PowerChanged();
            MineScript ts = hs.gameObject.GetComponent<MineScript>();
            if (ts != null) {
                if (powerLvl == 0) {
                    ts.PowerDown();
                } else {
                    ts.PowerUp();
                }
                
            }
        }

        ResetState();
    }

    public override string ToString() {
        return "Tile{["+x + "," + y+"], active="+gameObject.activeInHierarchy+"}";
    }
}
