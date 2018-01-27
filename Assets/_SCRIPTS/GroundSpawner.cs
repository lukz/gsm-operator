using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour {

	public GameObject tile;
	
	void Start () {
		if (!tile) {
			Debug.LogError("Ground Tile is missing");
			return;
		}
		Transform parent = GetComponent<Transform>();
		for (int x = -5; x < 5; x++)
		{	
			for (int y = -5; y < 5; y++)
			{
				GameObject.Instantiate(tile, new Vector3(x, y, 0), Quaternion.identity, parent);
			}
		}
	}
}
