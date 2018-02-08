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
	
	void Update () {
		float targetAspect = width/height;
		float aspect = Screen.width/(float)Screen.height;
		
		Camera.main.orthographicSize = Mathf.Max(orthoSize, orthoSize * targetAspect/aspect);
		// Camera.main.orthographicSize = orthoSize * targetAspect/aspect;		

		//Vector3 newpos = new Vector3 ((Screen.width-900), (Screen.height/600)-1f, guiCanvas.transform.position.z);

		//guiCanvas.transform.position = newpos;
	}
}
