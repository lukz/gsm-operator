using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PoweredFlashEffectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {


    }

    private void Awake()
    {
        SpriteRenderer sprite = transform.GetComponent<SpriteRenderer>();
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);

        DOTween.Sequence()
            .Append(sprite.DOFade(1, 0.05f)).SetEase(Ease.OutSine)
            .Append(sprite.DOFade(0, 0.4f)).SetEase(Ease.InSine)
            .OnComplete(() => {
                GameObject.Destroy(gameObject);
            });
    }

    // Update is called once per frame
    void Update () {
		
	}
}
