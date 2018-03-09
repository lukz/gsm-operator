using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tweens {


    public static void Squeeze(GameObject squeezedObject)
    {
        SpriteRenderer sr = squeezedObject.GetComponentInChildren<SpriteRenderer>();

        Sequence xScale = DOTween.Sequence()
            .Append(squeezedObject.transform.DOScaleX(1 + 1 * 0.2f, 0.05f).SetEase(Ease.InOutQuad))
            .Append(squeezedObject.transform.DOScaleX(1f, 0.6f).SetEase(Ease.OutElastic))
            ;

        Sequence yScale = DOTween.Sequence()
            .Append(squeezedObject.transform.DOScaleY(1 + 1 * 0.2f, 0.05f).SetEase(Ease.InOutQuad))
            .Append(squeezedObject.transform.DOScaleY(1, 0.6f).SetEase(Ease.OutElastic))
            .PrependInterval(0.05f);
    }

}
