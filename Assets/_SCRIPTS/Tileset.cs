using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tileset : MonoBehaviour {

    public List<TileRow> tiles;

    private const int MAP_HEIGHT = 4;
    private const int MAP_WIDTH = 5;
    // x, y
    private Tile[,] map = new Tile[MAP_WIDTH, MAP_HEIGHT];
    float tileSize = 1.125f;

    void Start () {
        // figure out where each tile is on a grid
        
		foreach (var row in tiles) {
            foreach (var tile in row.row) {
                Vector3 tp = tile.transform.position;
                Tile t = tile.GetComponent<Tile>();
                
                int x = GridX(tp.x);
                int y = GridY(tp.y);
                // Debug.Log("Tile " + t);
                // Debug.Log(tp + " -> [" + x +", " + y + "]");
                map[x, y] = t;
            }
		}
	}

    public int GridX (float x) 
    {
        return Mathf.FloorToInt((3.8f + x)/tileSize);
    }

    public int GridY (float y) 
    {
        // return MAP_HEIGHT - 1 - Mathf.FloorToInt((2.3f +y)/tileSize);
        return Mathf.FloorToInt((2.3f +y)/tileSize);
    }
	
	Tile lastTile;
	void Update () {
		if (false) {
			Vector3 mp = InputUtils.WorldMousePosition();
			Tile tile = GetTileAt(mp);
            Debug.Log("Cursor at x= " + mp.x + ", y= " +mp.y);
            Debug.Log("Cursor at ox= " + (3.8f + mp.x) + ", oy= " + (2.3f + mp.y));
            Debug.Log("Cursor at sx= " + (3.8f + mp.x)/tileSize + ", soy= " + (2.3f + mp.y)/tileSize);
            Debug.Log("Cursor at gx= " + GridX(mp.x) + ", gy= " + GridY(mp.y));
			if (tile != lastTile) {
				if (lastTile != null) {
					SpriteRenderer oldSprite = lastTile.gameObject.GetComponent<SpriteRenderer>();
					oldSprite.color = new Color(1, 1, 1, .5f);
				}
				lastTile = tile;
                if (tile != null) {
				    SpriteRenderer sprite = tile.gameObject.GetComponent<SpriteRenderer>();
				    sprite.color = new Color(1, 1, 1, 1);
                }
				Debug.Log("Tile at " + mp + ": " + tile);
			}
		}
	}

    public void ChangeTilesPower(GameObject startTile, int powerChange, List<TowerScript.PowerOffset> offsets)
    {
        Vector2 tilePos = GetTilePos(startTile);

        for (int i = 0; i < offsets.Count; i++)
        {
            int xPos = (int)tilePos.x + offsets[i].x;
            int yPos = (int)tilePos.y + offsets[i].y;
            
            // Check if not out of bounds
            if (xPos < 0 || xPos >= tiles[0].row.Count) continue;
            if (yPos < 0 || yPos >= tiles.Count) continue;
            GameObject t = tiles[yPos].row[xPos];
            if (t.activeInHierarchy) {
                t.GetComponent<Tile>().PowerChange(powerChange);
            }
        }
    }

    public Vector2 GetTilePos(GameObject tile)
    {
        for (int y = 0; y < tiles.Count; y++)
        {
            TileRow tileRow = tiles[y];

            for (int x = 0; x < tileRow.row.Count; x++)
            {
                GameObject checkedTile = tileRow.row[x];
                if (checkedTile == tile) return new Vector2(x, y);
            }
        }

        return new Vector2(-1, -1);
    }

	public Tile GetTileAt(Vector3 at) {
        // return GetTileAt(GridX(at.x), GridY(at.y));
		Rect tmp = new Rect();
		// tiles are 108 pixels, at 100 p/u, but theres a small gap
		float tileSize = 1.15f;
		foreach (var row in tiles) {
            foreach (var tile in row.row) {
                // apparently this is world pos, so thats nice
                Vector3 tp = tile.transform.position;
                // tiles are centered on the position
                tmp.Set(tp.x - tileSize/2, tp.y - tileSize/2, tileSize, tileSize);
                if (tmp.Contains(at)) {
                    return tile.GetComponent<Tile>();
                }   
            }
		}
		return null;
	}

    public Tile GetTileAt(int x, int y) {
        if (x < 0 || x >= MAP_WIDTH || y < 0 || y >= MAP_HEIGHT) return null;
        // return map[x, MAP_HEIGHT - 1 - y];
        return map[x, y];
    }

    public void ShowTileBuildStatus() {
        foreach (var row in tiles) {
            foreach (var tile in row.row) {
                tile.GetComponent<Tile>().ShowBuildStatus();
            }
		}
    }

    public void HideTileBuildStatus() {
        foreach (var row in tiles) {
            foreach (var tile in row.row) {
                tile.GetComponent<Tile>().HideBuildStatus();
            }
		}
    }

    [System.Serializable]
    public class TileRow
    {
        public List<GameObject> row;
    }
}
