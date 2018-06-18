using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrystal : MonoBehaviour {

	
	public Animator animator;
	public void SpawnFX() {
		animator.SetTrigger("explode");
	}
}
