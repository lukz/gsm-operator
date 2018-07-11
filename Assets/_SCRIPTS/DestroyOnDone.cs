using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDone : MonoBehaviour {

	public float time = 2;
	float timer = 0;

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > time)
		{
			GetComponent<Animator>().enabled = false;

			PoweredFlashEffectScript p = GetComponent<PoweredFlashEffectScript>();
			if (p)
			{
				p.tileParent.resourceFlash = null;
			}
			GameObject.Destroy(gameObject, 0.2f);
		}
	}

	public void destroy() {
		
	}
}
