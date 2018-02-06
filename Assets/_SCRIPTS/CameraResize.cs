using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResize : MonoBehaviour {
	public float orthoSize = 3;
	public float width = 9;
	public float height = 6;
	void Update () {
		float targetAspect = width/height;
		float aspect = Screen.width/(float)Screen.height;
		
		Camera.main.orthographicSize = Mathf.Max(orthoSize, orthoSize * targetAspect/aspect);		
		// Camera.main.orthographicSize = orthoSize * targetAspect/aspect;		
	}
}
