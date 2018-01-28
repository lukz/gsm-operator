using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIcon : MonoBehaviour {


	public GameObject icon;
    public float angle;

    public void rotateLeft() {
		Vector3 angles = icon.transform.rotation.eulerAngles;
        angle = angles.z - 90;

        icon.transform.rotation = Quaternion.Euler(angles.x, angles.y, angle);
    }
	public void rotateRght() {
        Vector3 angles = icon.transform.rotation.eulerAngles;
        angle = angles.z + 90;

        icon.transform.rotation = Quaternion.Euler(angles.x, angles.y, angle);

        
    }
}
