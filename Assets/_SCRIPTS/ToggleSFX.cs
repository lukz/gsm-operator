using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSFX : MonoBehaviour {
	public Sounds sounds;

	public Sprite on;
	public Sprite onPressed;
	public Sprite off;
	public Sprite offPressed;

	public Button button;
	public Image buttonBackground;
	
	void Start()
	{
		UpdateStyle();
	}

	public void Toggle() 
	{
		sounds.ToggleSound();
		UpdateStyle();
	}

	private void UpdateStyle() 
	{
		// glorious native crap
		SpriteState spriteState = new SpriteState();
		spriteState = button.spriteState;
		if (sounds.IsSoundOn()) {
			buttonBackground.sprite = on;
			spriteState.pressedSprite = onPressed;
		} else {
			buttonBackground.sprite = off;
			spriteState.pressedSprite = offPressed;
		}
		button.spriteState = spriteState;
	}
}
