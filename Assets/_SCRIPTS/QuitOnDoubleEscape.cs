using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnDoubleEscape : MonoBehaviour {
	public bool androidOnly = true;
	
	[Range(0.1f, 1f)]
	public float countResetDelay = .3f;
	int backCount = 0;
	// Update is called once per frame
	void Update () {
		if (!androidOnly || GameManager.IS_ANDROID) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				if (backCount >= 1) {
					Debug.Log("Quitting");
					Application.Quit();
				} else {
					Debug.Log("Starting countdown");
					backCount += 1;
					// TODO toast of some sort?
					StopAllCoroutines();
					StartCoroutine("CancelExit");
				}
			}
		}
	}

	IEnumerator CancelExit() {
		yield return new WaitForSeconds(countResetDelay);
		Debug.Log("Resetting exit count");
		backCount = 0;
		yield return null;
	}
}
