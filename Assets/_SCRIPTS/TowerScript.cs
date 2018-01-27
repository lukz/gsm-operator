using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour {

    public GameObject powerZone;
    public List<GameObject> powered;
    public int id;

    public bool isBuildable;

    public bool isBuilded;

    // Use this for initialization
    void Start () {
        powered = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onBuilded()
    {
        isBuilded = true;

        setPowerUps();
    }

    public void onDestroyed()
    {
        isBuilded = false;

        for (var i = 0; i < powered.Count; i++)
        {
            powered[i].GetComponent<HouseScript>().powerDown();
        }
    }

    public void setPowerUps()
    {
        for (var i = 0; i < powered.Count; i++)
        {
            powered[i].GetComponent<HouseScript>().powerUp();
        }
    }

}
