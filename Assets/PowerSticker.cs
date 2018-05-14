using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSticker : MonoBehaviour {

	[SerializeField]
	private Sprite powerOffSprite;

	[SerializeField]
	private Sprite powerOnSprite;

	private SpriteRenderer _spriterenderer;



	// Use this for initialization
	void Start () {

		_spriterenderer = GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void PowerUp()
	{
		_spriterenderer.sprite = powerOnSprite;
	}

	public void PowerDown()
	{
		_spriterenderer.sprite = powerOffSprite;
	}

}
