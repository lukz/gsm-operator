using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tileset : MonoBehaviour {

	public List<TileRow> tiles;



    private ToggleRocks[] allRocks;

    private const int MAP_HEIGHT = 4;
    private const int MAP_WIDTH = 5;
    // x, y
    private Tile[,] map = new Tile[MAP_WIDTH, MAP_HEIGHT];
    float tileSize = 1.2f;
    bool initialized = false;

    void Start () {
        // figure out where each tile is on a grid
		foreach (var row in tiles) {
            foreach (var tile in row.row) {
                Vector3 tp = tile.transform.position;
                Tile t = tile.GetComponent<Tile>();
                t.Init(this, GridX(tp.x), GridY(tp.y));
                map[t.x, t.y] = t;
            }
		}
        if (initialized) return;
        initialized = true;
        // init all tiles before doing this
        for(int x = 0; x < MAP_WIDTH; x++) 
        {
            for(int y = 0; y < MAP_HEIGHT; y++) 
            {
				// if there is a tower at the start, its not players
				Tile tile = GetTileAt(x, y);
                GameObject tower = tile.GetTower();
                if (tower != null)
                {
                    tower.GetComponent<TowerScript>().InitAsNotPlayers();
                }
            }
        }

        allRocks = transform.GetComponentsInChildren<ToggleRocks>();
    }

    public int GridX (float x) 
    {
        return Mathf.FloorToInt((2.9f + x)/tileSize);
    }

    public int GridY (float y) 
    {
        // return MAP_HEIGHT - 1 - Mathf.FloorToInt((2.3f +y)/tileSize);
        return Mathf.FloorToInt((2.5f + y)/tileSize);
    }
	
    private List<Tile> willPowerTiles = new List<Tile>();
	public List<GameObject> WillPowerRocks(List<GameObject> rocks, Tile startTile, TowerScript tower) {
        willPowerTiles.Clear();
        WillPowerRocksInternal(rocks, startTile, tower);
		return rocks;
	}

    private List<GameObject> WillPowerRocksInternal(List<GameObject> rocks, Tile startTile, TowerScript tower) {
        List<TowerScript.PowerOffset> offsets = tower.powerOffsets;
        for (int i = 0; i < offsets.Count; i++)
        {
            int xPos = startTile.x + offsets[i].x;
            // fliped y
            int yPos = startTile.y - offsets[i].y;
            
            Tile t = GetTileAt(xPos, yPos);
            if (t != null && t.gameObject.activeInHierarchy) {
                // dont process same tile more then once
                if (willPowerTiles.Contains(t)) {
                    continue;
                }
                willPowerTiles.Add(t);
                GameObject rock = t.GetEnemy();
                if (rock != null) {
                    // Debug.Log("Got rocks " + t.x + ", " + t.y);
                    rocks.Add(rock);
                }
                MineScript ms = t.GetMine();
                if (ms != null) {
                    //  Debug.Log("Got mine " + t.x + ", " + t.y);
                     WillPowerRocksInternal(rocks, t, t.GetTower().GetComponent<TowerScript>());
                }
            }
        }
        return rocks;
    }

    public void ChangeTilesPower(TowerScript tower, Tile startTile, int powerChange, List<TowerScript.PowerOffset> offsets)
    {
        List<Tile> validTiles = new List<Tile>();

        for (int i = 0; i < offsets.Count; i++)
        {
            int xPos = startTile.x + offsets[i].x;
            int yPos = startTile.y - offsets[i].y; // fliped y

            Tile t = GetTileAt(xPos, yPos);
            if (t != null && t.gameObject.activeInHierarchy) {
                validTiles.Add(t);
            }
        }
        // can be null when adding stuff in editor
        GameManager gm = GameManager.instance;
        if (gm == null) {
            for (int i = 0; i < validTiles.Count; i++)
            {
                validTiles[i].PowerChange(tower, powerChange);
            }
        } else {
            float fullTime = validTiles.Count < 5 ? gm.delayPowerFx : gm.delayPowerLargeFx;

            for (int i = 0; i < validTiles.Count; i++)
            {
                float delay = 0;

                delay = (fullTime / validTiles.Count) * i;

                if (powerChange <= 0) delay = 0;

                validTiles[i].PowerChangeDelayed(tower, powerChange, delay);
            }

            if(powerChange > 0)
            {
                gm.lockRestartFor(fullTime);
            }
        }
    }



    public void StartBuilding () 
    {
        foreach (var tile in map) {
            tile.StartBuilding();
        }
    }

    public void CancelBuilding () 
    {
        foreach (var tile in map) {
            tile.CancelBuilding();
        }
    }
   
	public Tile GetTileAt(Vector3 at) {
		return GetTileAt(at.x, at.y);
	}

    public Tile GetTileAt(float x, float y) {
        return GetTileAt(GridX(x), GridY(y));
    }

    public Tile GetTileAt(int x, int y) {
        if (!initialized) {
            Start();
        }
        if (x < 0 || x >= MAP_WIDTH || y < 0 || y >= MAP_HEIGHT) return null;
        // return map[x, MAP_HEIGHT - 1 - y];
        return map[x, y];
    }

    [System.Serializable]
    public class TileRow
    {
        public List<GameObject> row;
    }

    // Tile lastTile;
    // void Update () {
    // 	if (false) {
    // 		Vector3 mp = InputUtils.WorldMousePosition();
    // 		Tile tile = GetTileAt(mp);
    // 		if (tile != lastTile) {
    // 			if (lastTile != null) {
    // 				SpriteRenderer oldSprite = lastTile.gameObject.GetComponent<SpriteRenderer>();
    // 				oldSprite.color = new Color(1, 1, 1, .5f);
    // 			}
    // 			lastTile = tile;
    //             if (tile != null) {
    // 			    SpriteRenderer sprite = tile.gameObject.GetComponent<SpriteRenderer>();
    // 			    sprite.color = new Color(1, 1, 1, 1);
    //             }
    // 			Debug.Log("Tile at " + mp + ": " + tile);
    // 		}
    // 	}
    // }

    public void ToggleRocks()
    {
        for (int i = 0; i < allRocks.Length; i++)
        {
            allRocks[i].ToggleState();
			allRocks[i].gameObject.transform.parent.GetComponent<Tile>().ChangeHooks();
        }
    }
}
