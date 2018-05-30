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

	[SerializeField]
	private GameObject leftPanel;


	void Awake () {
		float targetAspect = width/height;
		// seems some weird stuff is going on when running in edit mode...
		if (Screen.width == 0 || Screen.height == 0) {
			return;
		}
		if (guiCanvas == null || leftPanel == null) {
			return;
		}
		float aspect = Screen.width/(float)Screen.height;
		
		Camera.main.orthographicSize = Mathf.Max(orthoSize, orthoSize * targetAspect/aspect);
		// Camera.main.orthographicSize = orthoSize * targetAspect/aspect;		

		Vector3 newpos = new Vector3 (Camera.main.orthographicSize*aspect   -2f, Mathf.Ceil((Screen.height/600)/200f*100f)/100f, guiCanvas.transform.position.z);
		newpos.x = Mathf.Round(newpos.x * 100f) / 100 +0.02f;
		newpos.y = Mathf.Round(newpos.y * 100f) / 100;
		guiCanvas.transform.position = newpos;

		newpos.x = leftPanel.transform.position.x - newpos.x;
		//leftPanel.transform.position = new Vector3(newpos.x, guiCanvas.transform.position.y, guiCanvas.transform.position.z);
	}
}
