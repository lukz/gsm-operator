using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FullScreenTintScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void fadeIn(float delay)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0);

        Sequence mySequence = DOTween.Sequence()

            //.Append(flashSequence)
            .Append(sr.DOFade(1, 2f).SetEase(Ease.InSine))
            .PrependInterval(delay);
        ;

    }

    public void fadeOut(float delay)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 1);

        Sequence mySequence = DOTween.Sequence()

            //.Append(flashSequence)
            .Append(sr.DOFade(0, 2f).SetEase(Ease.OutSine))
            .PrependInterval(delay)
            .OnComplete(() => {
                Destroy(gameObject);
            });


        //.PrependInterval(flashDuration);
        ;

    }
}
