using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour {

    public GameObject restrictedZone;
	public bool Powered { get { return powered; } private set { powered = value;} }

 	[SerializeField]
	private bool powered;
	
	Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}

	public void powerUp() {
		Powered = true;
	}

	public void powerDown() {
		Powered = false;
	}
	
	// Update is called once per frame
	void Update () {
		animator.SetBool("powered", powered);
	}
}
