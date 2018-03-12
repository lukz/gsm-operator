using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HouseScript : MonoBehaviour {

 	[SerializeField]
	private int powered = 0;

    public int requiredPower = 1;
	
	Animator animator;
    // Use this for initialization

    [SerializeField]
    private SpriteRenderer sprite;

	void Start () {
		animator = GetComponent<Animator>();
	}

	public void PowerChanged() {
        int newPower = transform.GetComponentInParent<Tile>().powerLvl;

        if(newPower > powered)
        {
            FlashPowered();
        } else if(newPower < powered)
        {
            FlashPowerDown();
        }

        powered = newPower;
	}
	
	// Update is called once per frame
	void Update () {
		animator.SetInteger("power", powered);
       
	}

    private float mix;
    private int flashColorId = Shader.PropertyToID("_FlashColor");
    private int flashMixId = Shader.PropertyToID("_FlashMix");

    public void FlashPowered()
    {
        sprite.material.SetColor(flashColorId, new Color(1, 1, 1, 0));

        float flashDuration = .4f;

        DOTween.Sequence()
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    {
                        sprite.material.SetFloat(flashMixId, mix = v);
                    },
                    1,
                    flashDuration * 0.1f
                ).SetEase(Ease.OutSine)
            )
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    {
                        sprite.material.SetFloat(flashMixId, mix = v);
                    },
                    0,
                    flashDuration * 0.9f
                ).SetEase(Ease.InSine)
            );

        DOTween.Sequence()
            .Append(transform.DOLocalMoveY(0.1f, 0.1f * flashDuration).SetEase(Ease.OutSine))
            .Append(transform.DOLocalMoveY(0, 0.9f * flashDuration).SetEase(Ease.InSine));
    }

    public void FlashPowerDown()
    {
        sprite.material.SetColor(flashColorId, new Color(0.3f, 0.3f, 0.3f, 0));

        float flashDuration = .4f;

        DOTween.Sequence()
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    {
                        sprite.material.SetFloat(flashMixId, mix = v);
                    },
                    1,
                    flashDuration * 0.1f
                ).SetEase(Ease.OutSine)
            )
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    {
                        sprite.material.SetFloat(flashMixId, mix = v);
                    },
                    0,
                    flashDuration * 0.9f
                ).SetEase(Ease.InSine)
            );
        
        float startY = transform.localPosition.y;

        DOTween.Sequence()
            .Append(transform.DOLocalMoveY(-0.1f, 0.1f * flashDuration).SetEase(Ease.OutSine))
            .Append(transform.DOLocalMoveY(0, 0.9f * flashDuration).SetEase(Ease.InSine));
    }



    public bool IsPowered()
    {
        return powered >= requiredPower;
    }
}
