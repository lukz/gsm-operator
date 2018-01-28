using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDone : MonoBehaviour {

	float timer = 0;

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > 2)
		{
			GetComponent<Animator>().enabled = false;
			GameObject.Destroy(gameObject, 0.2f);
		}
	}

	public void destroy() {
		
	}
}
