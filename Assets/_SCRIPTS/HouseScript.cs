using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HouseScript : MonoBehaviour {


	public int[] requiredResources;
	public int[] currentResources;

	public bool powered = false;

	Animator animator;

    [SerializeField]
    private SpriteRenderer sprite;

	void Start () {
		animator = GetComponent<Animator>();
	}

	public void UpdateResources()
	{
		//todo check ile jest dobrych elementow w current resources z required resources. porownaj array. jezeli dobrze to powered = true;
	}


	public void PowerChanged() {
        int newPower = transform.GetComponentInParent<Tile>().powerLvl;

		FlashPowered();
	}
	
	void Update () {
		int powerTemp = 0;
		if (powered) powerTemp = 1;
		animator.SetInteger("power", powerTemp);
	}




    private float mix;
    private int flashColorId = Shader.PropertyToID("_FlashColor");
    private int flashMixId = Shader.PropertyToID("_FlashMix");

    public void FlashPowered()
    {
        if (!Application.isPlaying && Application.isEditor) {
            Debug.Log("Not flashing in editor");
            return;
        }
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

        //DOTween.Sequence()
        //    .Append(transform.DOLocalMoveY(0.1f, 0.1f * flashDuration).SetEase(Ease.OutSine))
        //    .Append(transform.DOLocalMoveY(0, 0.9f * flashDuration).SetEase(Ease.InSine));
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

        //DOTween.Sequence()
        //    .Append(transform.DOLocalMoveY(-0.1f, 0.1f * flashDuration).SetEase(Ease.OutSine))
        //    .Append(transform.DOLocalMoveY(0, 0.9f * flashDuration).SetEase(Ease.InSine));
    }



    public bool IsPowered()
    {
        return powered;
    }
}
