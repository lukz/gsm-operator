using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour {

	float speed = .2f;
	// Use this for initialization
	void Start () {
		Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		speed = Random.Range(.05f, .3f);
		pos.x = Random.Range(-4.5f, 4.5f);
		pos.y = Random.Range(-3f, 3f);
		pos.x += speed * Time.deltaTime;
		transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		if (pos.x > 3.25f) {
			speed = Random.Range(.05f, .3f);
			pos.x = Random.Range(-10, -4.75f);
			pos.y = Random.Range(-3f, 3f);
		}
		pos.x += speed * Time.deltaTime;
		transform.position = pos;
	}
}
