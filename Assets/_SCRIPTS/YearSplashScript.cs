using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class YearSplashScript : MonoBehaviour {
    bool init;
	void Start () {
        if (init) return;
		SuperTextMesh textMesh = GetComponentInChildren<SuperTextMesh>();
        textMesh.Text = "";
	}

    public void ShowSplash(string year)
    {
        init = true;
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        SuperTextMesh textMesh = GetComponentInChildren<SuperTextMesh>();
        textMesh.Text = "YEAR  " + year;

        DOTween.Sequence()
            .SetDelay(1f)
            .Append(
                spriteRenderer.DOFade(0.3f, 2f)
            ).OnComplete(() => {
				//gameObject.SetActive(false);
					Sounds.PlayMusic();
				Sounds.RestoreMusic();
				Destroy(gameObject);
            }).PrependInterval(1f);
    }
}
