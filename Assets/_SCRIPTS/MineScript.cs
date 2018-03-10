using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour {
	private HouseScript houseScript;
	private TowerScript towerScript;

	bool powered;
	
	void Start () {
		houseScript = GetComponent<HouseScript>();
		towerScript = GetComponent<TowerScript>();
	}
	
	public void PowerDown() 
    {
		if (!powered) return;
		powered = false;
		towerScript.PowerDown();
    }

    public void PowerUp() 
    {
        if (powered) return;
		powered = true;
		towerScript.PowerUp();
    }
}
