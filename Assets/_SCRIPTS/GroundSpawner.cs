using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour {

	public GameObject[] tiles;
	public GameObject particle;
	[Range(0, 1)]
	public float particleSpawnChance = .04f;
	void Start () {
		if (tiles.Length==0) {
			Debug.LogError("Ground Tile is missing");
			return;
		}
		Transform parent = GetComponent<Transform>();
		for (float x = -6; x < 6; x++)
		{	
			for (float y = -6; y < 6; y++)
			{
				int which = Random.Range(0, tiles.Length);
				GameObject ob =  GameObject.Instantiate(tiles[which], new Vector3(-0.8f+x*0.56f, y*0.56f, 0), Quaternion.identity, parent);
				float val = 1 - ((Mathf.Abs(x) + Mathf.Abs(y))/12f * 0.3f) - Random.Range(0.02f,0.05f);
				if (Mathf.Abs(x) + Mathf.Abs(y) < 2)
				{
					val = 0.96f;
				}


					ob.GetComponent<SpriteRenderer>().color = new Color(val, val, val*1.3f);
				
			}
		}
	}

	void Update() {
		if (!particle) {
			Debug.LogError("Missing particle game object!");
			return;
		}
		if (Random.Range(0.0f, 1f) <= particleSpawnChance) {
			Vector3 pos = new Vector3(Random.Range(-6f, 6f), Random.Range(-6f, 6f), 0);
			GameObject inst = GameObject.Instantiate(particle, pos, Quaternion.identity, gameObject.transform);
			Animator animator = inst.GetComponent<Animator>();
			animator.SetInteger("idleid", Random.Range(0, 2));
		}
	}
}
