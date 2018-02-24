using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUtils {

	public static Vector3 WorldMousePosition() {
		return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
	}

}
