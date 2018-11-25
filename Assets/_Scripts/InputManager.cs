using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

	public GameObject pointer;
	public GameObject pointerStick;
	public Text text_debug;
	public Text text_trigger;
	public Text text_rotation;

	public GameObject objectToMove;

	// Use this for initialization
	void Start () {
		
	}

	Vector3 fwd;
	// Update is called once per frame
	void Update () {
		fwd = pointer.transform.TransformDirection(Vector3.forward);
		text_trigger.text = "" + OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);

		if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote)) { //making sure its the right one.
			if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) { //If we have the trigger down.

				if (objectToMove == null) { //Already have an object so dont have to fire again.
					RaycastHit hitInfo;
					if (Physics.Raycast(pointer.transform.position, fwd, out hitInfo, Mathf.Infinity)) {
						text_debug.text = "Currently Hitting: " + hitInfo.transform.name;
						objectToMove = hitInfo.transform.gameObject;
						pointerStick.SetActive(false);
					}
				}

				Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);

				//MOVE THE OBJECT
				objectToMove.transform.position = new Vector3(objectToMove.transform.position.x + controllerRotation.y, objectToMove.transform.position.y + -controllerRotation.x, objectToMove.transform.position.z); //y, x

			} else {
				if (objectToMove != null) {
					objectToMove = null;
					pointerStick.SetActive(true);
				}
			}

			//To see the rotational debug stuff.
			text_rotation.text = "Q: " + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote) + "/n" + "V: " + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).eulerAngles;
		}
	}
}
