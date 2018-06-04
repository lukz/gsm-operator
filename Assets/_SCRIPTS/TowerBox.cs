using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerBox : MonoBehaviour {

	public Sprite towerArc_0;

	public Sprite towerUp_1;
	
	public Sprite towerSide_2;

	BoxController controller;

	string tweenId;

	public void Init(BoxController controller, int slotId) {
		this.controller = controller;
		SlotId = slotId;
		tweenId = "TowerBoxTween"+slotId;
		Vector3 pos = controller.GetSlotPosition(slotId);
		transform.position = pos;
	}

	int type = -1;
	public int SlotId {get; private set;}
	GameObject prefab;
	Tweener tween;

	public void SetPrefab (GameObject prefab) {
		this.prefab = prefab;
		if (prefab == null) {
			SetType(-1);
			if (tween != null) {
				tween.Kill();
			}
			tween = null;
		} else {
			TowerScript ts = prefab.GetComponent<TowerScript>();
			SetType(ts.id);
		}
	}

	public GameObject GetPrefab() {
		return prefab;
	}

	void SetType(int type) {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		switch(type) {
			case 0: {
				sr.enabled = true;
				sr.sprite = towerArc_0;
			} break;
			case 1: {
				sr.enabled = true;
				sr.sprite = towerUp_1;
			} break;
			case 2: {
				sr.enabled = true;
				sr.sprite = towerSide_2;
			} break;
			default: {
				sr.enabled = false;
			} break;
		}
	}

	public void MoveToSlot(int slotId, float delay, OnDone onDone = null) {
		int diff = Mathf.Abs(SlotId - slotId);
		SlotId = slotId;
		// dont actually move if we are below last
		if (slotId < 0) {
			return;
		}
		if (tween != null) {
			tween.Kill();
		}
		float duration = .5f * diff;
		Vector3 pos = controller.GetSlotPosition(slotId);
		tween = transform.DOMove(pos, duration)
            	.SetId(tweenId)
				.SetDelay(delay)
				.SetEase(Ease.InOutFlash)
				.OnComplete(()=>{
					// Debug.Log("Moved to slot " + slotId);
					if (onDone != null) {
						onDone.Invoke(this);
					}
                }
			);

	}

	public delegate void OnDone(TowerBox box);
}
