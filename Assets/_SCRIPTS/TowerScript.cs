using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour {
    
    public int id;

	public bool playerTower = true;

    public List<PowerOffset> powerOffsets;

    private bool isAttachedToTile = false;

    // Use this for initialization
    void Start () {
        // hack to disable mine with id == 3
        if (!playerTower && id < 3) {
            AttachToTile();
        }
    }

    public bool DetachFromTile()
    {

        if (!isAttachedToTile) {
            Debug.Log("Not attached to tile");
			Sounds.PlayDeny();
            return false;
        }
        isAttachedToTile = false;

        Sounds.PlayDestroy();
        //for (var i = 0; i < powered.Count; i++)
        ///{
        //    powered[i].GetComponent<HouseScript>().powerDown();
        //}
        PowerDown();
        return true;
    }

    public bool AttachToTile()
    {

        if (isAttachedToTile) {
            Debug.Log("Already attached to tile");
            return false;
        }
        isAttachedToTile = true;

        PowerUp();

		if(playerTower) Sounds.PlayTowerBuild();
        //for (var i = 0; i < powered.Count; i++)
        //{
        //    powered[i].GetComponent<HouseScript>().powerUp();
        //}
        return true;
    }

    public void PowerDown() 
    {
        Tileset tilesetScript = transform.GetComponentInParent<Tileset>();
        Tile tileScript = transform.GetComponentInParent<Tile>();
        tilesetScript.ChangeTilesPower(tileScript.gameObject, -1, powerOffsets);
    }

    public void PowerUp() 
    {
        Tileset tilesetScript = transform.GetComponentInParent<Tileset>();
        Tile tileScript = transform.GetComponentInParent<Tile>();
        tilesetScript.ChangeTilesPower(tileScript.gameObject, 1, powerOffsets);
    }
    
    [System.Serializable]
    public class PowerOffset
    {
        public int x;
        public int y;
    }
}
