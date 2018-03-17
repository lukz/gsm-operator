using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tileset : MonoBehaviour {

    public List<TileRow> tiles;

    void Start () {

	}
	
	Tile lastTile;
	void Update () {
		if (false) {
			Vector3 mp = InputUtils.WorldMousePosition();
			Tile tile = GetTileAt(mp);
			if (tile != lastTile) {
				if (lastTile != null) {
					SpriteRenderer oldSprite = lastTile.gameObject.GetComponent<SpriteRenderer>();
					oldSprite.color = new Color(1, 1, 1, .5f);
				}
				lastTile = tile;
				SpriteRenderer sprite = tile.gameObject.GetComponent<SpriteRenderer>();
				sprite.color = new Color(1, 1, 1, 1);
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

    [System.Serializable]
    public class TileRow
    {
        public List<GameObject> row;
    }
}
