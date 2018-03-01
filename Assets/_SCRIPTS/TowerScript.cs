﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour {
    
    public int id;

	public bool playerTower = true;

    public List<PowerOffset> powerOffsets;

    private bool isAttachedToTile = false;

    // Use this for initialization
    void Start () {
        if (!playerTower) {
            AttachToTile();
        }
    }

    public void DetachFromTile()
    {

        if (!isAttachedToTile) {
            Debug.Log("Not attached to tile");
            return;
        }
        isAttachedToTile = false;

        Sounds.PlayDestroy();
        //for (var i = 0; i < powered.Count; i++)
        ///{
        //    powered[i].GetComponent<HouseScript>().powerDown();
        //}

        Tileset tilesetScript = transform.GetComponentInParent<Tileset>();
        Tile tileScript = transform.GetComponentInParent<Tile>();
        tilesetScript.ChangeTilesPower(tileScript.gameObject, -1, powerOffsets);
    }

    public void AttachToTile()
    {

        if (isAttachedToTile) {
            Debug.Log("Already attached to tile");
            return;
        }
        isAttachedToTile = true;

        Tileset tilesetScript = transform.GetComponentInParent<Tileset>();
        Tile tileScript = transform.GetComponentInParent<Tile>();
        tilesetScript.ChangeTilesPower(tileScript.gameObject, 1, powerOffsets);

        Sounds.PlayTowerBuild();
        //for (var i = 0; i < powered.Count; i++)
        //{
        //    powered[i].GetComponent<HouseScript>().powerUp();
        //}
    }
    
    [System.Serializable]
    public class PowerOffset
    {
        public int x;
        public int y;
    }
}
