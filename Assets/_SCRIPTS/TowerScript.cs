using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour {

    public GameObject powerZone;
    public List<GameObject> powered;
    public int id;

    public bool isBuildable;

    public bool isBuilded;

    public float PowerRotation {get; private set; }

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
		GameManager.instance.towerBuilt.Play();

        setPowerUps();
    }

    public void onDestroyed()
    {
        isBuilded = false;
		GameManager.instance.destroy.Play();
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

    public void setPowerRotation(float angle)
    {
        PowerRotation = angle - 90;
        powerZone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }
}
