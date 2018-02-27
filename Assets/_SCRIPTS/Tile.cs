using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public int powerLvl = 0;
	//public GameObject[] objectsHere;

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
    
    void SetAsBuildTarget()
    {

    }

    void CancelBuildTarget()
    {

    }

    public void Build(GameObject tower)
    {
        tower.transform.parent = transform;
        tower.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    public bool CanBuild()
    {
        return !IsBlocked();
    }

    public void PowerUp()
    {
        powerLvl++;

        OnPowerChange();
    }

    public void PowerDown()
    {
        powerLvl--;

        OnPowerChange();
    }

    private void OnPowerChange()
    {
        GameObject house = GetHouse();
        if (house != null)
        {
            house.GetComponent<HouseScript>().PowerChanged();
        }
    }
}
