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
        // toggling it makes it show up on the first run, for whatever reason
        textMesh.enabled = false;
        textMesh.Text = "YEAR  " + year;

        DOTween.Sequence()
        .Append(
            DOTween.Sequence()
                .AppendInterval(1 / 30f)
                .OnComplete(() => {
                    textMesh.enabled = true;
                })
        )
        .AppendInterval(1)
        .Append(
            spriteRenderer.DOFade(0f, 2.3f).SetEase(Ease.InSine)
        )
        .AppendInterval(0.1f)
        .OnComplete(() => {
            Sounds.PlayMusic();
            Sounds.RestoreMusic();
            Destroy(gameObject);
        });
    }
}
