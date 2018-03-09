using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour {

 	[SerializeField]
	private int powered = 0;

    public int requiredPower = 1;
	
	Animator animator;
    // Use this for initialization

	void Start () {
		animator = GetComponent<Animator>();
	}

	public void PowerChanged() {
        powered = transform.GetComponentInParent<Tile>().powerLvl;
	}
	
	// Update is called once per frame
	void Update () {
		animator.SetInteger("power", powered);
       
	}



    public bool IsPowered()
    {
        return powered >= requiredPower;
    }
}
