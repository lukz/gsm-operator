using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

		Vector3 newPosition = transform.position;

		newPosition.x = (Mathf.Round(newPosition.x * 10f) / 10f);
		newPosition.y = (Mathf.Round(newPosition.y * 10f) / 10f);

		transform.position = newPosition;

	}
}
