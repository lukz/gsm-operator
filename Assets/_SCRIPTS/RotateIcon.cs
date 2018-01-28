using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIcon : MonoBehaviour {
	public GameObject icon;
	public void rotateLeft() {
		Vector3 angles = icon.transform.rotation.eulerAngles;
		icon.transform.rotation = Quaternion.Euler(angles.x, angles.y, angles.z - 90);
	}
	public void rotateRght() {
		Vector3 angles = icon.transform.rotation.eulerAngles;
		icon.transform.rotation = Quaternion.Euler(angles.x, angles.y, angles.z + 90);
	}
}
