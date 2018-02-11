using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour {

	public GameObject[] tiles;
	public GameObject[] decals;
	public int decalCount = 50;
	public GameObject particle;
	[Range(0, 1)]
	public float particleSpawnChance = .4f;

	[FloatRange(.1f, 3)]
	public FloatRange spawnFrequency;
	void Start () {
		SpawnGround();
		SpawnParticleMaybe();
	}

	public void SpawnGround() {
		// clear existing children if any
		while (transform.childCount > 0)
		{
			// we use DestroyImmediate so this works in the editor
			GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
		}

		float cx = Camera.main.transform.position.x;
		float cy = Camera.main.transform.position.y;
		// float height = Camera.main.orthographicSize * 2;
		// target ortho size
		float height = 6;
		float width = height * Camera.main.aspect;

		if (tiles.Length > 0) {
			// size of the single tile, ie pixel width/cam pixels per unit
			const float size = 0.56f;
			float hw = (width + size)/2;
			float hh = (height + size)/2;
			for (float x = -hw; x <= hw; x += size)
			{	
				for (float y = -hh; y <= hh; y += size)
				{
					int which = Random.Range(0, tiles.Length);
					GameObject go = tiles[which];
					if (!go) {
						Debug.LogError("Ground: Tile at " + which + " missing");
						continue;
					}
					Vector3 pos = new Vector3(cx + x -1, cy + y, 0);
					GameObject ob =  GameObject.Instantiate(tiles[which], pos, Quaternion.identity, transform);
					float val = 1 - ((Mathf.Abs(x) + Mathf.Abs(y))/12f * 0.4f) - Random.Range(0.02f,0.05f);
					if (Mathf.Abs(x) + Mathf.Abs(y) < 2)
					{
						val = 1f;
					}
					val *= 0.9f;

					ob.GetComponent<SpriteRenderer>().color = new Color(val, val, val*1.3f);
				}
			}
		} else {
			Debug.LogError("Ground: Ground tiles are missing");
		}

		if (decals.Length > 0) {
			float hw = width/2;
			float hh = height/2;
			for (int i = 0; i < decalCount; i++)
			{
				int which = Random.Range(0, decals.Length);
				GameObject go = decals[which];
				if (!go) {
					Debug.LogError("Ground: Decal at " + which + " missing");
					continue;
				} 
				Vector3 pos = new Vector3(cx + Random.Range(-hw, hw)-1, cx + Random.Range(-hh, hh), 0);
				go = GameObject.Instantiate(go, pos, Quaternion.identity, transform);

				float val = 1 - ((Mathf.Abs(go.transform.position.x) + Mathf.Abs(go.transform.position.y)) / 12f * 0.2f) - Random.Range(0.02f, 0.05f);
				if (Mathf.Abs(go.transform.position.x) + Mathf.Abs(go.transform.position.y) < 2)
				{
					val = 1f;
				}
				go.GetComponent<SpriteRenderer>().color = new Color(val, val, val * 1.2f);

			}
		} else {
			Debug.LogError("Ground: Decals are missing");
		}
	}


	public void SpawnParticleMaybe() {
		if (Random.Range(0.0f, 1f) <= particleSpawnChance) {
			SpawnParticle();
		}
		Invoke("SpawnParticleMaybe", Random.Range(spawnFrequency.min, spawnFrequency.max));
	}
	public void SpawnParticle() {
		if (!particle) {
			Debug.LogError("Ground: Missing particle game object!");
			return;
		}
		float cx = Camera.main.transform.position.x;
		float cy = Camera.main.transform.position.y;
		// target ortho size
		float hh = 3; // Camera.main.orthographicSize;
		float hw = hh * Camera.main.aspect;

		Vector3 pos = new Vector3(cx + Random.Range(-hw, hw), cy + Random.Range(-hh, hh), 0);
		GameObject inst = GameObject.Instantiate(particle, pos, Quaternion.identity, gameObject.transform);
		Animator animator = inst.GetComponent<Animator>();
		animator.SetInteger("idleid", Random.Range(0, 2));
	}
}
