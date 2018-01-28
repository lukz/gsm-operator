using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDone : MonoBehaviour {
	public void destroy() {
		GameObject.Destroy(gameObject);
	}
}
