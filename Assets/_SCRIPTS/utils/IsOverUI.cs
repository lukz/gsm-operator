using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IsOverUI : MonoBehaviour {

	GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

	// Use this for initialization
	void Awake () {
		//Fetch the Raycaster from the GameObject (the Canvas)
		Canvas canvas = FindObjectOfType<Canvas>();
        raycaster = canvas.GetComponent<GraphicRaycaster>();
	//	Debug.Log("raycaster " + raycaster);
        //Fetch the Event System from the Scene
        eventSystem = FindObjectOfType<EventSystem>();
	//	Debug.Log("events " + eventSystem);
	}	


	public int getOverCount(Vector3 pos) {
		//Set up the new Pointer Event
		pointerEventData = new PointerEventData(eventSystem);
		//Set the Pointer Event Position to that of the mouse position
		pointerEventData.position = pos;

		//Create a list of Raycast Results
		List<RaycastResult> results = new List<RaycastResult>();

		//Raycast using the Graphics Raycaster and mouse click position
		raycaster.Raycast(pointerEventData, results);

		//For every result returned, output the name of the GameObject on the Canvas hit by the Ray
		// foreach (RaycastResult result in results)
		// {
		// 	Debug.Log("Hit " + result.gameObject.name);
		// }
		return results.Count;
	}
}
