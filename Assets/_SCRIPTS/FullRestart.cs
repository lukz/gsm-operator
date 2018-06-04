using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FullRestart : MonoBehaviour {

	[SerializeField]
	private GameManager gameManager;
	bool touching;
	// Use this for initialization
	void Start () {
		EventTrigger trigger = GetComponent<EventTrigger>();
        {
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { 
				PointerEventData ped = (PointerEventData)data; 
				Debug.Log("Touch down");
				// 0 >= on device
				if (ped.pointerId == 0) {
					touching = true;
				}
			});
			trigger.triggers.Add(entry);
		}
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerUp;
			entry.callback.AddListener((data) => { 
				PointerEventData ped = (PointerEventData)data; 
				Debug.Log("Touch up");
				// 0 >= on device
				if (ped.pointerId == 0) {
					touching = true;
				}
			});
			trigger.triggers.Add(entry);
		}
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerExit;
			entry.callback.AddListener((data) => { 
				PointerEventData ped = (PointerEventData)data; 
				Debug.Log("Touch exit");
				// 0 >= on device
				if (ped.pointerId == 0) {
					touching = true;
				}
			});
			trigger.triggers.Add(entry);
		}
	}
	
	void Update () {
		if (!touching) return;
		if (Input.touchCount == 0) {
			touching = false;
			return;
		}
		// 6.667 appears to be max on iphone 6s
		Touch touch = Input.GetTouch(0);
		// Debug.Log("pressure = " + touch.pressure);
		if (touch.pressure >= 3) {
			if (gameManager.RestartAll()) {
				Debug.Log("Returned all towers");
				// TODO animation of some sort?
			} else {
				Debug.Log("Nothing to return");
			}
		}
	}
}
