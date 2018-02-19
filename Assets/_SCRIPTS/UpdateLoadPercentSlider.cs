using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLoadPercentSlider : MonoBehaviour {
	Slider slider;	
	void Start () {
		slider = GetComponent<Slider>();	
		if (!slider) {
			Debug.LogError("Missing Slider component!");
			return;
		}
		ChangeSceneAsync csa = FindObjectOfType<ChangeSceneAsync>();
		if (csa) {
			csa.onProgress += UpdateSlider;
		} else {
			Debug.LogError("Missing ChangeSceneAsync component!");
		}
	}

	void UpdateSlider(float value) {
		if (slider) {
			slider.value = value;
		}
	}
}
