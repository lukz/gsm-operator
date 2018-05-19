using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerSticker : MonoBehaviour {

	[SerializeField]
	private Sprite powerOffSprite;

	[SerializeField]
	private Sprite powerOnSprite;

	private SpriteRenderer _spriterenderer;

    public bool isPowered = false;


	// Use this for initialization
	void Start () {

		_spriterenderer = GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void PowerUp()
	{
		if (_spriterenderer)
		{
			_spriterenderer.sprite = powerOnSprite;
			isPowered = true;

			Vector3 initialScale = transform.localScale;

			DOTween.Sequence()
				.Append(transform.DOScale(new Vector3(initialScale.x * 1.5f, initialScale.y * 1.5f, 1), 0.02f)).SetEase(Ease.OutSine)
				.Append(transform.DOScale(new Vector3(initialScale.x, initialScale.y, 1f), 0.6f)).SetEase(Ease.InSine);
		}
    }

	public void PowerDown()
	{
		if (_spriterenderer)
		{
			_spriterenderer.sprite = powerOffSprite;
			isPowered = false;
		}

    }

}
