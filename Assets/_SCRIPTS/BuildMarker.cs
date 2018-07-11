using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMarker : MonoBehaviour {
	const int CAN_BUILD_OFF = 0;
	const int CAN_BUILD_ALLOW = 1;
	const int CAN_BUILD_DISALLOW = 2;
	const int CAN_BUILD_PLACE = 3;

	public GameObject willPowerMarker;
	public GameObject willWaterMarker;
	private Animator canBuildAnimator;
	void Init () {
		if (canBuildAnimator == null) {
			canBuildAnimator = GetComponent<Animator>();
		}
	}

	public void StartBuild(bool canBuild) {
		Init();
		if (canBuild) {
			canBuildAnimator.SetInteger("state", CAN_BUILD_ALLOW);
		} else {
			canBuildAnimator.SetInteger("state", CAN_BUILD_OFF);
		}
	}

	public void OverBuild(bool canBuild) {
		Init();
		if (canBuild) {
			canBuildAnimator.SetInteger("state", CAN_BUILD_PLACE);
		} else {
			canBuildAnimator.SetInteger("state", CAN_BUILD_DISALLOW);
		}
	}

	public void CancelOverBuild(bool canBuild) {
		Init();
		StartBuild(canBuild);
	}

	public void CancelBuild() {
		Init();
		canBuildAnimator.SetInteger("state", CAN_BUILD_OFF);
	}

	public void StartWillPowerUp(bool blocked) {
		willPowerMarker.SetActive(true);
		willPowerMarker.GetComponent<Animator>().SetBool("blocked", blocked);
	}

	public void CancelWillPowerUp() {
		willPowerMarker.SetActive(false);
	}
}
