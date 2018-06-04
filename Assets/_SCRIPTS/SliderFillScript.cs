using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SliderFillScript : MonoBehaviour {

    private Image imageScript;

	// Use this for initialization
	void Start () {
        imageScript = GetComponent<Image>();
        imageScript.material = new Material(imageScript.material);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    int flashMixId = Shader.PropertyToID("_FlashMix");
    float mix;

    public void FlashSlider()
    {
        float flashDuration = .8f;

        DOTween.Sequence()
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    {
                        imageScript.material.SetFloat(flashMixId, mix = v);
                    },
                    0.6f,
                    flashDuration * 0.1f
                ).SetEase(Ease.OutSine)
            )
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    {
                        imageScript.material.SetFloat(flashMixId, mix = v);
                    },
                    0,
                    flashDuration * 0.9f
                ).SetEase(Ease.InSine)
            );
    }
}
