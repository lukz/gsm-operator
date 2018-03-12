using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class EventTriggerProxy : MonoBehaviour {
	private GameObject towerPrefab;
	private TowerSpawnerPro towerSpawnerPro;
	public Image towerImage;
	bool locked = true;

    public SpriteRenderer gateSpriteFilled;
    public SpriteRenderer gateSpriteEmpty;
    private Vector3 gateSpritePos;

	private float enableImageDelay = .2f;

    private float timeToShake = 3f;
    private float timeToShakeLeft;

    private GameObject flareObject;

    void Start()
    {
        if(GetComponentInChildren<FlareScript>() != null)
        {
            flareObject = GetComponentInChildren<FlareScript>().gameObject;
        }

        timeToShakeLeft = timeToShake;

        towerSpawnerPro = FindObjectOfType<TowerSpawnerPro>();
		if (!towerSpawnerPro) {
			Debug.LogError("TowerSpawnerPro missing!");
		}

		if (!towerImage) {
			Debug.LogError("Image missing!");
		}
		if (!gateSpriteFilled) {
			Debug.LogError("Gate filled missing!");
		}
        gateSpritePos = gateSpriteFilled.transform.position;
		if (!gateSpriteEmpty) {
			Debug.LogError("Gate empty missing!");
		}

        EventTrigger trigger = GetComponent<EventTrigger>();
        {
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { 
				PickTower((PointerEventData)data); 
			});
			trigger.triggers.Add(entry);
		}
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerUp;
			entry.callback.AddListener((data) => { 
				PlaceTower((PointerEventData)data);
			});
			trigger.triggers.Add(entry);
		}
    }

	public void SetTowerPrefab(GameObject prefab) {
		towerPrefab = prefab;
		Button button = GetComponent<Button>();
		if (prefab != null) {
			GameObject body = prefab.transform.Find("Body").gameObject;
			SpriteRenderer spriteRenderer = body.GetComponent<SpriteRenderer>();
			towerImage.sprite = spriteRenderer.sprite;
			towerImage.enabled = true;
			button.enabled = true;
			gateSpriteFilled.enabled = true;
			gateSpriteEmpty.enabled = false;
		} else {
			towerImage.enabled = false;
			button.enabled = false;
			gateSpriteFilled.enabled = false;
			gateSpriteEmpty.enabled = true;
		}
	}

	//private float lockTimer;
	void Update() {
        if(!locked && towerImage != null && towerImage.enabled)
        {
            timeToShakeLeft -= Time.deltaTime;

            if(timeToShakeLeft <= 0)
            {
                timeToShakeLeft = timeToShake;
                towerImage.rectTransform.DOPunchRotation(new Vector3(0, 0, 20), .5f, 10, 1);
            }

            ShowFlare(true);
        } else
        {
            ShowFlare(false);
        }
    }

    void PickTower(PointerEventData data)
    {
		if (locked) return;
		Button button = GetComponent<Button>();
		if (!button.enabled) return;
        // Debug.Log("PickTower called " + towerPrefab);
		if (towerSpawnerPro != null && towerPrefab != null) {
			towerSpawnerPro.PickTower(this, towerPrefab);
			button.enabled = false;
		}
		if (towerImage != null ) {
			towerImage.enabled = false;
		}
    }

	void PlaceTower(PointerEventData data)
    {
        // Debug.Log("PlaceTower called "  + towerPrefab);
		if (towerSpawnerPro != null && towerPrefab != null) {
			towerSpawnerPro.PlaceTower(this, towerPrefab);
		}
    }

	public void ReturnTower() {
		if (towerImage != null) {
			towerImage.enabled = true;
		}
		Button button = GetComponent<Button>();
		button.enabled = true;
	}

    int flashMixId = Shader.PropertyToID("_FlashMix");
    float mix;

    public void Unlock() {
		if (towerPrefab == null) return;
		if (!locked) return;
		Debug.Log("Unlock");
		locked = false;

       

        //lockTimer = .5f;
        // TODO hide the graphic

        float flashDuration = .2f;

        DOTween.Sequence()
            .SetId("PokrywaFlash")
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    {
                        gateSpriteFilled.material.SetFloat(flashMixId, mix = v);
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
                        gateSpriteFilled.material.SetFloat(flashMixId, mix = v);
                    },
                    0,
                    flashDuration * 0.9f
                ).SetEase(Ease.InSine)
            );

        Sequence mySequence = DOTween.Sequence()
            .SetId("PokrywaMove")

            //.Append(flashSequence)
            .Append(gateSpriteFilled.transform.DOMoveY(gateSpritePos.y + 0.5f, 0.15f).SetEase(Ease.OutSine))
            .Append(gateSpriteFilled.transform.DOMoveY(gateSpritePos.y + -5, 0.8f).SetEase(Ease.InSine))

            .Insert(0, gateSpriteFilled.transform.DORotate(new Vector3(0, 0, 25), 0.5f).SetEase(Ease.InOutSine))
            .Insert(0, gateSpriteFilled.transform.DOMoveX(gateSpritePos.x - 1.5f, 1.5f).SetEase(Ease.OutSine))
            .Insert(0, gateSpriteFilled.DOFade(0, 0.7f).SetEase(Ease.InSine))

            .PrependInterval(flashDuration);
            ;

       
    }

	public void Lock() {
		if (towerPrefab == null) return;
		if (locked) return;
		Debug.Log("Lock");
		locked = true;
        
        //lockTimer = .5f;
        // TODO show the graphic

        gateSpriteFilled.transform.DOKill();
        gateSpriteFilled.material.DOKill();

        DOTween.Kill("PokrywaFlash");
        DOTween.Kill("PokrywaMove");

        gateSpriteFilled.transform.localPosition = new Vector3(0, 0, 0);
        gateSpriteFilled.transform.rotation = Quaternion.EulerAngles(new Vector3(0, 0, 0));

        gateSpriteFilled.DOFade(1, 0f).SetEase(Ease.InSine);
        Tweens.Squeeze(gateSpriteFilled.gameObject, 1f, 0.5f, gateSpriteFilled.transform.localScale.x, gateSpriteFilled.transform.localScale.y);

    }

	public void Reset () {
		ReturnTower();
		Lock();
	}

    public void ShowFlare(bool show)
    {
        if (flareObject == null || flareObject.activeSelf == show) return;

        flareObject.SetActive(show);
    }
}
