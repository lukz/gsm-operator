using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLoadPercentText : MonoBehaviour {
	Text text;
	void Start () {
		text = GetComponent<Text>();	
		if (!text) {
			Debug.LogError("Missing Text component!");
			return;
		}
		ChangeSceneAsync csa = FindObjectOfType<ChangeSceneAsync>();
		if (csa) {
			csa.onProgress += UpdateText;
		} else {
			Debug.LogError("Missing ChangeSceneAsync component!");
		}
	}

	void UpdateText(float value) {
		string loaded = string.Format("LOADED {0}%", (int)(100 * value));
		Debug.Log(loaded);
		if (text) {
			text.text = loaded;
		}
	}
}
