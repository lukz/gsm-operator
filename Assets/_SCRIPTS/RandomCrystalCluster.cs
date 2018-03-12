using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCrystalCluster : MonoBehaviour {
	public GameObject prefab;
	public bool randomPositions = false;

	[Range(0, 32)]
	public int minCrystalCount = 10;

	[Range(0, 32)]
	public int maxCrystalCount = 14;

	public bool randomSpread = true;
	public float spread = .05f;
	void Start () {
		if (randomSpread) 
		{
			PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
			collider.enabled = false;
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				Vector3 dir = new Vector3(Random.Range(0, spread), 0, 0);
				Vector3 offset = Quaternion.AngleAxis(Random.Range(-180f, 180f), Vector3.up) * dir;
				Debug.Log("Move " +offset);
				child.position += offset;
			}
		}

		if (randomPositions) 
		{
			Debug.Log("welp "  +transform.childCount);
			for (int i = 0; i < transform.childCount; i++)
			{
				Destroy(transform.GetChild(i).gameObject);
			}
			int count = Random.Range(minCrystalCount, maxCrystalCount);	
			// count = 100;
			int added = 0;
			int tries = 0;
			PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
			collider.enabled = true;
			float x = transform.position.x;
			float y = transform.position.y;
			float spread = .5f;
			while (added < count && tries < 100)
			{
				Vector3 pos = new Vector3(x + Random.Range(-spread, spread), y + Random.Range(-spread, spread), 0);
				if (collider.OverlapPoint(pos)) {
					Debug.Log("Point in collider " + pos);
					GameObject.Instantiate(prefab, pos, Quaternion.identity, transform);
					added ++;
				}
				tries++;
			}
			Debug.Log("Added " + added + ", tries " + tries);

			collider.enabled = false;
		}
	}
}
