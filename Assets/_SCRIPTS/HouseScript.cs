using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour {

    public GameObject restrictedZone;

	// Use this for initialization
	void Start () {
		
	}

	public void powerUp() {
		GetComponent<Animator>().SetBool("powered", true);
	}

	public void powerDown() {
		GetComponent<Animator>().SetBool("powered", false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
