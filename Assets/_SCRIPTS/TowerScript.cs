﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour {
    
    public int id;

	public bool playerTower = true;

    public List<PowerOffset> powerOffsets;

    private bool isBuildable;
    public bool IsBuildable {
        get {
            return isBuildable;
        }
        set {
            isBuildable = value;
            if (animator) {
                animator.SetBool("isBuildable", value);
            }
        }
    }

    public bool isBuilded;
    private bool isAddedToTile = false;

    private Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponentInParent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        if (isBuilded && !isAddedToTile)
        {
            AttachToTile();
        }
    }

    public void onBuilded()
    {
        isBuilded = true;
		GameManager.instance.towerBuilt.Play();

        AttachToTile();
    }

    public void OnDestroyed()
    {
        isBuilded = false;

        DetachFromTile();
    }

    public void DetachFromTile()
    {
        isAddedToTile = false;

        GameManager.instance.destroy.Play();
		//for (var i = 0; i < powered.Count; i++)
        ///{
        //    powered[i].GetComponent<HouseScript>().powerDown();
        //}
    }

    public void AttachToTile()
    {
        isAddedToTile = true;

        Tileset tilesetScript = transform.GetComponentInParent<Tileset>();


        Tile tileScript = transform.GetComponentInParent<Tile>();

        tilesetScript.PowerUpTiles(tileScript.gameObject, powerOffsets);
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
