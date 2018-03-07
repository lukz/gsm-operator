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

    public SpriteRenderer gateSprite;
    private Vector3 gateSpritePos;

	private float enableImageDelay = .2f;
	void Start()
    {
		towerSpawnerPro = FindObjectOfType<TowerSpawnerPro>();
		if (!towerSpawnerPro) {
			Debug.LogError("TowerSpawnerPro missing!");
		}

		if (!towerImage) {
			Debug.LogError("Image missing!");
		}
		if (!gateSprite) {
			Debug.LogError("Gate missing!");
		}
        gateSpritePos = gateSprite.transform.position;

        EventTrigger trigger = GetComponent<EventTrigger>();
        {
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback.AddListener((data) => { 
				PickTower((PointerEventData)data); 
				PlaceTower((PointerEventData)data);
			});
			trigger.triggers.Add(entry);
		}
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.BeginDrag;
			entry.callback.AddListener((data) => { 
				PickTower((PointerEventData)data);
			});
			trigger.triggers.Add(entry);
		}
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.EndDrag;
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
		} else {
			towerImage.enabled = false;
			button.enabled = false;
		}
	}

	//private float lockTimer;
	void Update() {
        //if (locked)
        //{
        //    if (lockTimer > 0)
        //    {
        //        lockTimer -= Time.deltaTime;
        //        float a = 1 - Mathf.Clamp01(lockTimer / .5f);
        //        gateSprite.color = new Color(1, 1, 1, a);
        //    }
        //}
        //else
        //{
        //    if (lockTimer > 0)
        //    {
        //        lockTimer -= Time.deltaTime;
        //        float a = Mathf.Clamp01(lockTimer / .5f);
        //        gateSprite.color = new Color(1, 1, 1, a);
        //    }
        //}
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
                        gateSprite.material.SetFloat(flashMixId, mix = v);
                    },
                    1,
                    flashDuration / 2
                ).SetEase(Ease.OutSine)
            )
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    {
                        gateSprite.material.SetFloat(flashMixId, mix = v);
                    },
                    0,
                    flashDuration / 2
                ).SetEase(Ease.InSine)
            );

        Sequence mySequence = DOTween.Sequence()
            .SetId("PokrywaMove")

            //.Append(flashSequence)
            .Append(gateSprite.transform.DOMoveY(gateSpritePos.y + 0.5f, 0.15f).SetEase(Ease.OutSine))
            .Append(gateSprite.transform.DOMoveY(gateSpritePos.y + -3, 0.8f).SetEase(Ease.InSine))

            .Insert(0, gateSprite.transform.DORotate(new Vector3(0, 0, 25), 0.5f).SetEase(Ease.InOutSine))
            .Insert(0, gateSprite.transform.DOMoveX(gateSpritePos.x - 1.5f, 1.5f).SetEase(Ease.OutSine))
            .Insert(0, gateSprite.DOFade(0, 0.9f).SetEase(Ease.InSine))

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

        gateSprite.transform.DOKill();
        gateSprite.material.DOKill();

        DOTween.Kill("PokrywaFlash");
        DOTween.Kill("PokrywaMove");

        gateSprite.transform.localPosition = new Vector3(0, 0, 0);
        gateSprite.transform.rotation = Quaternion.EulerAngles(new Vector3(0, 0, 0));
        gateSprite.DOFade(1, 1f).SetEase(Ease.InSine);

    }

	public void Reset () {
		ReturnTower();
		Lock();
	}
}
