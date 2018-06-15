using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerMarker : MonoBehaviour
{
	public Sprite powerOn;
	public Sprite powerOff;

	public bool showIfNoPower = false;

	public int power = 0;

	public int requiredPower = 0;

	private List<GameObject> markers = new List<GameObject>();
	private List<SpriteRenderer> markerRenderers = new List<SpriteRenderer>();

	[SerializeField]
	private Animator animatorPower;

    public static float currentMarkerDelay = 0;
    private float perMarkerDelayAdd = 0.1f;

    public void Awake()
    {
        currentMarkerDelay = 0;
    }

    public void SetPower(int power, int powerChange)
	{
		if (power < 0) power = 0;
		this.power = power;
		// Debug.Log("power = " + power);
		// if we dont need power, create them dynamically
		if (animatorPower) animatorPower.SetInteger("power", power);
		if (requiredPower > 0)
		{
			CreateMarkers(requiredPower);

			foreach (var sr in markerRenderers)
			{
				sr.sprite = powerOff;
			}

			switch (power)
			{
				case 0:
					{
						// do nothing
					}
					break;
				case 1:
					{
						markerRenderers[0].sprite = powerOn;
					}
					break;
				case 2:
					{
						markerRenderers[0].sprite = powerOn;
						if(markerRenderers.Count>1)	markerRenderers[1].sprite = powerOn;
					}
					break;
				default:
					{ // 3+
						markerRenderers[0].sprite = powerOn;
						if (markerRenderers.Count > 1)markerRenderers[1].sprite = powerOn;
						if (markerRenderers.Count > 2) markerRenderers[2].sprite = powerOn;
					}
					break;
			}

            if(powerChange > 0)
            {
                for (int i = power; i < power + powerChange && i <= markerRenderers.Count; i++)
                {
                    DOTween.Sequence()
                        .Append(markerRenderers[i - 1].transform.DOLocalMoveY(0.1f, 0.1f * 0.4f).SetEase(Ease.OutSine).SetRelative(true))
                        .Append(markerRenderers[i - 1].transform.DOLocalMoveY(-0.1f, 0.6f * 0.4f).SetEase(Ease.InSine).SetRelative(true));
                }
            } else {
                for (int i = power + Mathf.Abs(powerChange); i > power && i <= markerRenderers.Count; i--)
                {
                    DOTween.Sequence()
                        .Append(markerRenderers[i - 1].transform.DOLocalMoveY(-0.1f, 0.1f * 0.4f).SetEase(Ease.OutSine).SetRelative(true))
                        .Append(markerRenderers[i - 1].transform.DOLocalMoveY(0.1f, 0.6f * 0.4f).SetEase(Ease.InSine).SetRelative(true));
                }
            }


		}
	}

	public void SetRequiredPower(int requiredPower)
	{
		if (requiredPower < 0) requiredPower = 0;
		if (requiredPower > 3) requiredPower = 3;

		this.requiredPower = requiredPower;
		// Debug.Log("req power = " + requiredPower);

		CreateMarkers(requiredPower);

		if (requiredPower > 0)
		{
			SetPower(power, 0);
		}
	}

	private void CreateMarkers(int count)
	{
		foreach (var go in markers)
		{
			GameObject.Destroy(go);
		}
		markers.Clear();
		markerRenderers.Clear();
		float y = .4f;
		// added in light up order
		switch (count)
		{
			case 1:
				{
					CreateMarker("marker1", 0f, y);
				}
				break;
			case 2:
				{
					CreateMarker("marker1", -.13f, y);
					CreateMarker("marker2", .13f, y);
				}
				break;
			case 3:
				{
					CreateMarker("marker1", -.26f, y);
					CreateMarker("marker2", 0f, y);
					CreateMarker("marker3", .26f, y);
				}
				break;
		}
	}

	private void CreateMarker(string name, float x, float y)
	{
		// Debug.Log("Create marker at " + x + ", " + y);
		GameObject m = new GameObject(name);
		m.transform.parent = transform;
		m.transform.position = new Vector2(transform.position.x + x, transform.position.y + y);
		float scale = 1;
		m.transform.localScale = new Vector3(scale, scale, scale);
		markers.Add(m);

		SpriteRenderer sr = m.AddComponent<SpriteRenderer>();
		sr.sprite = powerOff;
		sr.sortingLayerName = "GUI";
		sr.sortingOrder =99;

		markerRenderers.Add(sr);
	}

    public void MapFinishedTween()
    {
        for (int i = 0; i < markers.Count; i++)
        {
            GameObject marker = markers[i];
            SpriteRenderer mr = marker.GetComponent<SpriteRenderer>();

            DOTween.Sequence()
            .Append(marker.transform.DOScale(1.3f, 0.2f).SetEase(Ease.OutSine))
            .Append(marker.transform.DOScale(0f, 0.2f).SetEase(Ease.InSine))
            .PrependInterval(currentMarkerDelay)
            .OnComplete(() =>
            {
                Destroy(marker.gameObject);
            });

            currentMarkerDelay += perMarkerDelayAdd;
        }

        SpriteRenderer or = animatorPower.GetComponentInChildren<SpriteRenderer>();
        if(or != null)
        {
            or.DOFade(0, 1f).SetEase(Ease.InSine);
        }
    }
}
