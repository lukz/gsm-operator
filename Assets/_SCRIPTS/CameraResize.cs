using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraResize : MonoBehaviour {
	float orthoSize = 3;
	float width = 9;
	float height = 6;

	public float widX;

	[SerializeField]
	private Canvas guiCanvas;

	[SerializeField]
	private GameObject notchMask;

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
		float ox = 0;
		if (Screen.safeArea.x > 0) {
			Vector3 offsetStart = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, Camera.main.nearClipPlane));
			Vector3 offsetEnd = Camera.main.ScreenToWorldPoint(new Vector3(118.0f, 0.0f, Camera.main.nearClipPlane));
			Vector3 offset = offsetEnd - offsetStart;
			ox = offset.x;

			Vector3 newPo = notchMask.transform.localPosition;
			newPo.x += 12f;
			notchMask.transform.localPosition = newPo;

		}
		leftPanel.transform.position = new Vector3(-Camera.main.orthographicSize * aspect +0.2f + ox, guiCanvas.transform.position.y+1f, guiCanvas.transform.position.z);
		widX = Camera.main.orthographicSize * aspect*2f;
	}
}
