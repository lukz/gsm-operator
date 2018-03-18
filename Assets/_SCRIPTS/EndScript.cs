using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndScript : MonoBehaviour {


	public SpriteRenderer endText;


	// Use this for initialization
	void Start () {
		if(!GameManager.SOFTLAUNCH)
		Invoke("changeText", 6f);
	}
	

	void changeText()
	{
		Sounds.PlayFinal();
				endText.DOFade(0f, 5f);
		endText.gameObject.transform.DOLocalMove(new Vector3(0, 1, 0), 5f);
	}

}
