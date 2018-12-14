using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtCamera : MonoBehaviour {
	public Camera theCamera;

	void Update () {
		transform.LookAt(transform.position + theCamera.transform.rotation * Vector3.forward, theCamera.transform.rotation * Vector3.up);
	}
}
