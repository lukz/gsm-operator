using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public int powerLvl = 0;
    //public GameObject[] objectsHere;

    private Color normalColor = new Color(0xFF, 0xFF, 0xFF, 0.3f);
    private Color blockedColor = new Color(0xFF, 0x00, 0x00, 0xD0);
    private Color poweredColor = new Color(0x00, 0xB5, 0xFF, 0xC0);
    private Color targetColor = new Color(0x0, 0xFF, 0x00, 0xFD);

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    bool HasEnergyField()
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
        return false;

    }

    bool IsBlocked()
    {
        return HasRocks() || HasHouse() || HasTower();
    }
    
    public void SetAsBuildTarget()
    {
        if(IsBlocked() || !HasEnergyField()) {
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
            GetComponent<SpriteRenderer>().color = poweredColor;
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

        tower.GetComponent<TowerScript>().OnBuilded();
    }

    public bool CanBuild()
    {
        return !IsBlocked() && HasEnergyField();
    }

    public void PowerChange(int powerChange)
    {
        powerLvl += powerChange;

        OnPowerChange();
    }

    private void OnPowerChange()
    {
        GameObject house = GetHouse();
        if (house != null)
        {
            house.GetComponent<HouseScript>().PowerChanged();
        }

        ResetState();
    }
}
