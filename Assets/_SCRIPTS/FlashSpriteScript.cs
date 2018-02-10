using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSpriteScript : MonoBehaviour {
	[SerializeField]
	SpriteRenderer[] sprites;

	[SerializeField]
	float onDuration = .5f;
	[SerializeField]
	float offDuration = .5f;

	Color baseColor = new Color(1, 1, 1, 1);

	[SerializeField]
	Color flashColor = new Color(1, 0, 0, 1);
	[SerializeField]
	int times = -1;

	[SerializeField]
	bool useDisable = false;

	
	[SerializeField]
	bool enableOnStart = false;

	private bool flashEnabled = false;
	
	// Use this for initialization
	void Start () {
		if (enableOnStart) {
			enable();
		}
	}

	private IEnumerator running;
	public void enable() {
		if (sprites.Length > 0 && !flashEnabled) {
			if (sprites[0] != null) {
				baseColor = sprites[0].color;
			}
			Debug.Log("Enable FlashSprites");
        	StartCoroutine(running = FlashSprites(sprites, times, onDuration, offDuration, baseColor, flashColor, useDisable));
			flashEnabled = true;
		}
	}

	public void disable() {
		if (flashEnabled) {
			Debug.Log("Disable FlashSprites");
			StopCoroutine(running);
			running = null;
			flashEnabled = false;

			// make sure we end ip in enabled state
			for (int i = 0; i < sprites.Length; i++)
			{
				if (useDisable)
				{
					sprites[i].enabled = true;
				}
				else
				{
					sprites[i].color = baseColor;
				}
			}
		}
		
	}

	IEnumerator FlashSprites(SpriteRenderer[] sprites, int numTimes, float onDuration, float offDuration, Color onColor, Color offColor, bool disable = false)
    {
		if (numTimes == -1) {
			while(true) {
				Debug.Log("FlashSprites on");
				yield return FlashSprites(sprites, offDuration, offColor, false, disable);
				Debug.Log("FlashSprites off");
				yield return FlashSprites(sprites, onDuration, onColor, true, disable);
			}
		} else{
			// number of times to loop
			for (int loop = 0; loop < numTimes; loop++)
			{
				Debug.Log("FlashSprites on");
				yield return FlashSprites(sprites, offDuration, offColor, false, disable);
				Debug.Log("FlashSprites off");
				yield return FlashSprites(sprites, onDuration, onColor, true, disable);
			}
		}
		Debug.Log("FlashSprites exiting");
		yield break;
    }

	WaitForSeconds FlashSprites(SpriteRenderer[] sprites, float delay,  Color color, bool enabled = false, bool useDisable = false)
    {
		for (int i = 0; i < sprites.Length; i++)
		{
			if (useDisable)
			{
				sprites[i].enabled = enabled;
			}
			else
			{
				sprites[i].color = color;
			}
		}
		return new WaitForSeconds(delay);
    }
}
