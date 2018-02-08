using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraResize : MonoBehaviour {
	float orthoSize = 3;
	float width = 9;
	float height = 6;

	[SerializeField]
	private Canvas guiCanvas;
	
	void LateUpdate () {
		float targetAspect = width/height;
		float aspect = Screen.width/(float)Screen.height;
		
		Camera.main.orthographicSize = Mathf.Max(orthoSize, orthoSize * targetAspect/aspect);
		// Camera.main.orthographicSize = orthoSize * targetAspect/aspect;		

		Vector3 newpos = new Vector3 (Camera.main.orthographicSize*aspect   -2f, Mathf.Round((Screen.height/600)/200f*100f)/100f, guiCanvas.transform.position.z);
		newpos.x = Mathf.Round(newpos.x * 100f) / 100;
		newpos.y = Mathf.Round(newpos.y * 100f) / 100;
		guiCanvas.transform.position = newpos;
	}
}
