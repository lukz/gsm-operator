using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FlareScript : MonoBehaviour {

    public bool isEnabled = false;
    public float timeBetween = 1f;
    
    private bool isImage = false;

    [SerializeField]
    private float moveTime = 0.7f;

    [SerializeField]
    private float moveYBy = -0.7f;

    private float timeToMove;

    private Material targetMaterial;

    [SerializeField]
    private Image flash;

	// Use this for initialization
	void Start () {
        timeToMove = timeBetween;
        
        if (GetComponentInParent<SpriteRenderer>() != null)
        {
            isImage = false;
            targetMaterial = GetComponentInParent<SpriteRenderer>().material;
        }
        else
        {
            isImage = true;

            Image image = GetComponentInParent<Image>();
            image.material = new Material(image.material);
            targetMaterial = image.material;

            if (flash != null) flash.DOFade(0, 0.00001f);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!isEnabled) return;

        timeToMove -= Time.deltaTime;
        if(timeToMove < 0)
        {
            timeToMove = timeBetween;
            MoveFlare();
        }
	}

    int flashMixId = Shader.PropertyToID("_FlashMix");
    float mix;

    public void MoveFlare()
    {
        float startPos = transform.localPosition.y;

        float flashDuration = .4f;


        if(isImage && flash != null)
        {
            DOTween.Sequence()
                .Append(transform.DOLocalMoveY(startPos + moveYBy, moveTime).SetEase(Ease.InCubic))
                .Append(transform.DOLocalMoveY(startPos, 0.001f))
                .Append(flash.DOFade(1, flashDuration * 0.1f).SetEase(Ease.OutSine))
                .Append(flash.DOFade(0, flashDuration * 0.9f).SetEase(Ease.InSine))
                
                ;
        }
        else
        {
            DOTween.Sequence()
                .Append(transform.DOLocalMoveY(startPos + moveYBy, moveTime).SetEase(Ease.InCubic))
                .Append(transform.DOLocalMoveY(startPos, 0.001f))
                .Append(
                    DOTween.To(
                        () => mix,
                        v =>
                        {
                            targetMaterial.SetFloat(flashMixId, mix = v);
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
                            targetMaterial.SetFloat(flashMixId, mix = v);
                        },
                        0,
                        flashDuration * 0.9f
                    ).SetEase(Ease.InSine)
                );
        }



    }
}
