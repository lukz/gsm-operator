using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrystal : MonoBehaviour {

	public GameObject enemyCrystalFX;
	
	public void SpawnFX() {
		GameObject.Instantiate(enemyCrystalFX, transform);
	}
}
