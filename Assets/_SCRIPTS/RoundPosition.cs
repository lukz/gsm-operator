using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPosition : MonoBehaviour {

	public float ile=1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

		Vector3 newPosition = transform.position;

		newPosition.x = (Mathf.Round(newPosition.x * ile) / ile);
		newPosition.y = (Mathf.Round(newPosition.y * ile) / ile);

		transform.position = newPosition;

	}
}
