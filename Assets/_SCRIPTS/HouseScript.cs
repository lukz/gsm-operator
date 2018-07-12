using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HouseScript : MonoBehaviour {


	public List<int> requiredResources;
	public List<int> currentResources=new List<int>();


	private List<GameObject> markers = new List<GameObject>();
	private List<SpriteRenderer> markerRenderers = new List<SpriteRenderer>();

	public static float currentMarkerDelay = 0;
	private float perMarkerDelayAdd = 0.1f;


	public bool powered = false;

	private List<int> lastResources = new List<int>();

	Animator animator;

    [SerializeField]
    private SpriteRenderer sprite;

	void Start () {
		currentMarkerDelay = 0;
		animator = GetComponent<Animator>();
		CreateMarkers(requiredResources.Count);
		UpdateResources(0);
	}

	public void UpdateResources(int powerChange)
	{

		if (GetComponent<MineScript>()) return;

		if (powerChange != 0)
		{
			currentResources.Add(powerChange);
			FlashPowered();
			/*for (int i = power; i < power + powerChange && i <= markerRenderers.Count; i++)
			{
				DOTween.Sequence()
					.Append(markerRenderers[i - 1].transform.DOLocalMoveY(0.1f, 0.1f * 0.4f).SetEase(Ease.OutSine).SetRelative(true))
					.Append(markerRenderers[i - 1].transform.DOLocalMoveY(-0.1f, 0.6f * 0.4f).SetEase(Ease.InSine).SetRelative(true));
			}*/
		}
		else
		{
			if (lastResources.Count > currentResources.Count)
			{
				FlashPowerDown();
				/*for (int i = power + Mathf.Abs(powerChange); i > power && i <= markerRenderers.Count; i--)
				{
					DOTween.Sequence()
						.Append(markerRenderers[i - 1].transform.DOLocalMoveY(-0.1f, 0.1f * 0.4f).SetEase(Ease.OutSine).SetRelative(true))
						.Append(markerRenderers[i - 1].transform.DOLocalMoveY(0.1f, 0.6f * 0.4f).SetEase(Ease.InSine).SetRelative(true));
				}*/
			}

		}
		powered = false;
		List<int> tempArray = new List<int>();
		for (int i = 0; i < requiredResources.Count; i++)
		{
			tempArray.Add(requiredResources[i]);

			markerRenderers[i].sprite = GameManager.instance.powerOff;
			if (requiredResources[i] < 0)
				markerRenderers[i].sprite = GameManager.instance.waterOff;
		}
		for (int i = 0; i < currentResources.Count; i++)
		{
			for (int j = 0; j < tempArray.Count; j++)
			{
				if(currentResources[i] == tempArray[j])
				{
					if(tempArray[j] == 1)
					{
						for (int h = 0; h < markerRenderers.Count; h++)
						{
							if (markerRenderers[h].sprite == GameManager.instance.powerOff) markerRenderers[h].sprite = GameManager.instance.powerOn;
						}
					}else
					{
						for (int h = 0; h < markerRenderers.Count; h++)
						{
							if (markerRenderers[h].sprite == GameManager.instance.waterOff) markerRenderers[h].sprite = GameManager.instance.waterOn;
						}
					}
					tempArray.RemoveAt(j);
				}
			}
		}
		if (tempArray.Count == 0)
		{
			powered = true;
		}
		lastResources = currentResources;
	}

	
	void Update () {
		int powerTemp = 0;
		if (powered) powerTemp = 1;
		animator.SetInteger("power", powerTemp);
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
		sr.sprite = GameManager.instance.powerOff;
		sr.sortingLayerName = "GUI";
		sr.sortingOrder = 99;

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

	}





	private float mix;
    private int flashColorId = Shader.PropertyToID("_FlashColor");
    private int flashMixId = Shader.PropertyToID("_FlashMix");

    public void FlashPowered()
    {
        if (!Application.isPlaying && Application.isEditor) {
            Debug.Log("Not flashing in editor");
            return;
        }
        sprite.material.SetColor(flashColorId, new Color(1, 1, 1, 0));

        float flashDuration = .4f;

        DOTween.Sequence()
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    {
                        sprite.material.SetFloat(flashMixId, mix = v);
                    },
                    1,
                    flashDuration * 0.1f
                ).SetEase(Ease.OutSine)
            )
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    {
                        sprite.material.SetFloat(flashMixId, mix = v);
                    },
                    0,
                    flashDuration * 0.9f
                ).SetEase(Ease.InSine)
            );

        //DOTween.Sequence()
        //    .Append(transform.DOLocalMoveY(0.1f, 0.1f * flashDuration).SetEase(Ease.OutSine))
        //    .Append(transform.DOLocalMoveY(0, 0.9f * flashDuration).SetEase(Ease.InSine));
    }

    public void FlashPowerDown()
    {
        sprite.material.SetColor(flashColorId, new Color(0.3f, 0.3f, 0.3f, 0));

        float flashDuration = .4f;

        DOTween.Sequence()
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    {
                        sprite.material.SetFloat(flashMixId, mix = v);
                    },
                    1,
                    flashDuration * 0.1f
                ).SetEase(Ease.OutSine)
            )
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    {
                        sprite.material.SetFloat(flashMixId, mix = v);
                    },
                    0,
                    flashDuration * 0.9f
                ).SetEase(Ease.InSine)
            );
        
        float startY = transform.localPosition.y;

        //DOTween.Sequence()
        //    .Append(transform.DOLocalMoveY(-0.1f, 0.1f * flashDuration).SetEase(Ease.OutSine))
        //    .Append(transform.DOLocalMoveY(0, 0.9f * flashDuration).SetEase(Ease.InSine));
    }



    public bool IsPowered()
    {
        return powered;
    }
}
