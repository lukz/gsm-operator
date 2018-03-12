using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tweens {


    public static void Squeeze(GameObject squeezedObject, float strengthScale = 1f, float timeScale = 1f, float startScaleX = 1f, float startScaleY = 1f)
    {
        SpriteRenderer sr = squeezedObject.GetComponentInChildren<SpriteRenderer>();

        float strength = 0.2f * strengthScale;

        float startTime = 0.05f * timeScale;
        float backTime = 0.6f * timeScale;

        Sequence xScale = DOTween.Sequence()
            .Append(squeezedObject.transform.DOScaleX(startScaleX + startScaleX * strength, startTime).SetEase(Ease.InOutQuad))
            .Append(squeezedObject.transform.DOScaleX(startScaleX, backTime).SetEase(Ease.OutElastic))
            ;

        Sequence yScale = DOTween.Sequence()
            .Append(squeezedObject.transform.DOScaleY(startScaleY + startScaleY * strength, startTime).SetEase(Ease.InOutQuad))
            .Append(squeezedObject.transform.DOScaleY(startScaleY, backTime).SetEase(Ease.OutElastic))
            .PrependInterval(0.05f * timeScale);
    }

}
