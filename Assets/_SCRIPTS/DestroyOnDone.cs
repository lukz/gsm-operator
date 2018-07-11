using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDone : MonoBehaviour {

	public float time = 1;
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
				if(p.tileParent)
				p.tileParent.resourceFlash = null;
			}
			GameObject.Destroy(gameObject);
		}
	}

	public void destroy() {
		
	}
}
