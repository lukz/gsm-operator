using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class YearSplashScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowSplash(int year)
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        SuperTextMesh textMesh = GetComponentInChildren<SuperTextMesh>();

        textMesh.Text = "YEAR   " + year;

        DOTween.Sequence()
            .SetDelay(1f)
            .Append(
                spriteRenderer.DOFade(0.3f, 2f)
            ).OnComplete(() => {
                //gameObject.SetActive(false);
                Destroy(gameObject);
            });
    }
}
