using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public int powerLvl = 0;
    //public GameObject[] objectsHere;

    private Color normalColor = new Color(0xFF, 0xFF, 0xFF, 0f);
    private Color blockedColor = new Color(0xFF, 0x00, 0x00, 0xD0);
    private Color poweredColor = new Color(0x00, 0xB5, 0xFF, 0xC0);
    private Color powered2Color = new Color(0x60, 0x90, 0xFF, 0xC0);
    private Color powered3Color = new Color(0x00, 0x00, 0x0, 0xC0);
	private Color targetColor = new Color(0x0, 0xFF, 0x00, 0xFD);

    public int x;
    public int y;

    public PowerMarker powerMarker;
    public BuildMarker buildMarker;

    private List<TowerScript.PowerOffset> towerPowerOffsets;

    private Tileset tileset;

    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
        GameObject house = GetHouse();
        if (house != null) {
            HouseScript hs = house.GetComponent<HouseScript>();
            powerMarker.SetRequiredPower(hs.requiredPower);
        } else {
            powerMarker.SetRequiredPower(0);
            powerMarker.SetPower(powerLvl);
        }

        tileset = GetComponentInParent<Tileset>();
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
    
    public void SetAsBuildTarget(List<TowerScript.PowerOffset> powerOffsets)
    {
        if(!CanBuild()) {
            GetComponent<SpriteRenderer>().color = blockedColor;
        } else {
            GetComponent<SpriteRenderer>().color = targetColor;
            if (towerPowerOffsets != null) {
                CancelBuildTarget();
            }
            towerPowerOffsets = powerOffsets;
            if (towerPowerOffsets != null) {
                int bx = tileset.GridX(transform.position.x);
                int by = tileset.GridY(transform.position.y);
                // Debug.Log("Target tile at " + bx + ", " + by);
                // go over all tiles and enable power
                foreach (var offset in powerOffsets) {
                    int tx = bx + offset.x;
                    int ty = by - offset.y;
                    // Debug.Log("Will power at " + tx + ", " + ty);
                    Tile at = tileset.GetTileAt(tx, ty);
                    if (at != null && at.gameObject.activeInHierarchy) {
                        at.buildMarker.StartWillPowerUp();
                    }
                }
            }
        }

        
    }

    public void CancelBuildTarget()
    {
        ResetState();
        if (towerPowerOffsets != null) {
            // go over all tiles and disable power
            int bx = tileset.GridX(transform.position.x);
            int by = tileset.GridY(transform.position.y);
            // go over all tiles and enable power
            foreach (var offset in towerPowerOffsets) {
                int tx = bx + offset.x;
                int ty = by - offset.y;
                Tile at = tileset.GetTileAt(tx, ty);
                if (at != null && at.gameObject.activeInHierarchy) {
                    at.buildMarker.CancelWillPowerUp();
                }
            }
            towerPowerOffsets = null;
        }
    }

    void ResetState()
    {
        GetComponent<SpriteRenderer>().color = normalColor;
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

    public void ShowBuildStatus() {
        buildMarker.StartBuild(CanBuild());
    }

    public void HideBuildStatus() {
        buildMarker.CancelBuild();
    }


    public override string ToString() {
        return "Tile{["+x + "," + y+"], active="+gameObject.activeInHierarchy+"}";
    }
}
