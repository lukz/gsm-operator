using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PowerMarker : MonoBehaviour {
	
	public GameObject markerBorder1;
	public GameObject markerBorder2;
	public GameObject markerBorder3;
	public GameObject markerLow;
	public GameObject markerMed1;
	public GameObject markerMed2;
	public GameObject markerHigh1;
	public GameObject markerHigh2;
	public GameObject markerHigh3;

	public bool showIfNoPower = false;
	
	private Animator overlay;
	
	public int power = 0;

	public int requiredPower = 0;



	void Start()
	{
		overlay = GetComponentInChildren<Animator>();
		
		if (overlay == null) {
			Debug.LogError("Overlay not found " + this);
		}
	}

#if UNITY_EDITOR
	private int _power;
	private int _requiredPower;

	void Update () {
		// if (_power != power) {
		// 	_power = power;
		// 	SetPower(power);
		// }
		// if (_requiredPower != requiredPower) {
		// 	_requiredPower = requiredPower;
		// 	SetRequiredPower(requiredPower);
		// }
	}
#endif

	public void SetPower(int power) {
		this.power = power;
		markerLow.SetActive(false);
		markerMed1.SetActive(false);
		markerMed2.SetActive(false);
		markerHigh1.SetActive(false);
		markerHigh2.SetActive(false);
		markerHigh3.SetActive(false);
		UpdateBorders();
		// no border, just full markers based on power
		if (requiredPower == 0) {
			if (power == 1) {
				markerHigh1.SetActive(true);
				markerBorder1.SetActive(true);
			} else if (power == 2) {
				markerHigh1.SetActive(true);
				markerHigh2.SetActive(true);
				markerBorder2.SetActive(true);
			} else if (power >= 3) {
				markerHigh1.SetActive(true);
				markerHigh2.SetActive(true);
				markerHigh3.SetActive(true);
				markerBorder3.SetActive(true);
			}
		} else if (requiredPower == 1) {
			if (power >= 1) {
				markerHigh1.SetActive(true);
			}
		} else if (requiredPower == 2) {
			if (power == 1) {
				markerLow.SetActive(true);
			} else if (power >= 2) {
				markerHigh1.SetActive(true);
				markerHigh2.SetActive(true);
			}
		} else if (requiredPower >= 3) {
			if (power == 1) {
				markerLow.SetActive(true);
			} else if (power == 2) {
				markerMed1.SetActive(true);
				markerMed2.SetActive(true);
			} else if (power >= 3) {
				markerHigh1.SetActive(true);
				markerHigh2.SetActive(true);
				markerHigh3.SetActive(true);
			}
		}
		// if Start is not called before we run this...
		if (overlay == null) {
			overlay = GetComponentInChildren<Animator>();
			if (overlay == null) {
				Debug.LogError("Overlay not found " + this);
			} else {
				overlay.SetInteger("power", power);
			}
		} else {
			overlay.SetInteger("power", power);
		}
	}

	public void SetRequiredPower(int requiredPower) {
		this.requiredPower = requiredPower;
		UpdateBorders();
		SetPower(power);
	}

	private void UpdateBorders() 
	{
		if (!showIfNoPower && power == 0) {
			markerBorder1.SetActive(false);
			markerBorder2.SetActive(false);
			markerBorder3.SetActive(false);
			return;
		}
		switch(requiredPower) {
			case 0: {
				markerBorder1.SetActive(false);
				markerBorder2.SetActive(false);
				markerBorder3.SetActive(false);
			} break;
			case 1: {
				markerBorder1.SetActive(true);
				markerBorder2.SetActive(false);
				markerBorder3.SetActive(false);
			} break;
			case 2: {
				markerBorder1.SetActive(false);
				markerBorder2.SetActive(true);
				markerBorder3.SetActive(false);
			} break;
			case 3: {
				markerBorder1.SetActive(false);
				markerBorder2.SetActive(false);
				markerBorder3.SetActive(true);
			} break;
		}
	}
}
