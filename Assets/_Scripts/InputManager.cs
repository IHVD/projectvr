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

	public float velocityMultiplier;

	public GameObject objectToMove;

	Vector3 fwd;
	Vector3 prevPos;

	// Update is called once per frame
	void Update () {
		fwd = pointer.transform.TransformDirection(Vector3.forward);
		text_trigger.text = "" + OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);

		if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote)) { //making sure its the right one.
			if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) { //If we have the trigger down.

				//TODO layermask
				if (objectToMove == null) { //Already have an object so dont have to fire again.
					RaycastHit hitInfo;
					LayerMask layerMask = 1 << 9;
					if (Physics.Raycast(pointer.transform.position, fwd, out hitInfo, Mathf.Infinity, ~layerMask)) {
						text_debug.text = "Currently Hitting: " + hitInfo.transform.name;
						objectToMove = hitInfo.transform.gameObject;
						objectToMove.GetComponent<Rigidbody>().isKinematic = true;
						pointerStick.SetActive(false);
						
					}
				}
				
				Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);

				//MOVE THE OBJECT
				//objectToMove.transform.position = new Vector3(objectToMove.transform.position.x + controllerRotation.y, objectToMove.transform.position.y + -controllerRotation.x, objectToMove.transform.position.z); //y, x
				objectToMove.transform.parent = pointerStick.transform.parent;

			} else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger)) {
				if (objectToMove != null) {
					objectToMove.transform.parent = null;
					objectToMove.GetComponent<Rigidbody>().isKinematic = false;

					Vector3 currPos = objectToMove.transform.position;
					objectToMove.GetComponent<Rigidbody>().velocity = (currPos - prevPos) / Time.deltaTime * velocityMultiplier;

					objectToMove = null;
					pointerStick.SetActive(true);
				}
			}
			if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)){
				Vector3 currPos = objectToMove.transform.position;
				prevPos = currPos;
			}

			//To see the rotational debug stuff.
			text_rotation.text = "Q: " + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote) + "\n" + "V: " + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).eulerAngles;
		}
	}
}
