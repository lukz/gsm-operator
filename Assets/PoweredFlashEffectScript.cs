using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PoweredFlashEffectScript : MonoBehaviour {

    private SpriteRenderer sprite;

    private float delay = 0;

    // Use this for initialization
    void Start () {
        sprite = transform.GetComponent<SpriteRenderer>();
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);

        DOTween.Sequence()
            .Append(sprite.DOFade(1, 0.05f)).SetEase(Ease.OutSine)
            .Append(sprite.DOFade(0, 0.6f)).SetEase(Ease.InSine)
            .PrependInterval(delay)
            .OnComplete(() => {
                GameObject.Destroy(gameObject);
            });

    }

    public void SetDelay(float delay)
    {
        this.delay = delay;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
