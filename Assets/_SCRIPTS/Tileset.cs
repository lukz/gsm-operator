using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tileset : MonoBehaviour {

    public List<TileRow> tiles;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PowerUpTiles(GameObject startTile, List<TowerScript.PowerOffset> offsets)
    {
        Vector2 tilePos = GetTilePos(startTile);

        for (int i = 0; i < offsets.Count; i++)
        {
            int xPos = (int)tilePos.x + offsets[i].x;
            int yPos = (int)tilePos.y + offsets[i].y;

            // Check if not out of bounds
            if (xPos < 0 || xPos > tiles[0].row.Count) continue;
            if (yPos < 0 || yPos > tiles.Count) continue;

            tiles[yPos].row[xPos].GetComponent<Tile>().PowerUp();
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

    [System.Serializable]
    public class TileRow
    {
        public List<GameObject> row;
    }
}
