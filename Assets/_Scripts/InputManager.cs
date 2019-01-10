using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class InputKey {

	public bool getDown;
	public InputManager.Inputs input;
	public OVRInput.Button ocGO; //Go button to check
	public OVRInput.Button ocRift; //Rift button to check
	public bool usesAxis;
	public OVRInput.Axis1D ocGoAx; //axis button to check
	public OVRInput.Axis1D ocRiftAx; //axis button to check

	public bool pressed; //to use for debugging.
}

public class InputManager : MonoBehaviour {

	public static InputManager inputMan;
	public OVRScreenFade screenFade;
	public FumehoodSliderScript fumeSlider;
	public PlayerStatusUI PlayerUI;

	[Header("Pointer and Debug")]
	public GameObject pointer;
	public GameObject pointerStick;
	public Text text_debug;
	public Text text_trigger;
	public Text text_rotation;

	[Header("Throwing Object")]
	public float velocityMultiplier;
	public GameObject objectToMove;
	Vector3 prevPos;

	[Header("Raycast")]
	LayerMask layerMask = 1 << 9;
	Vector3 fwd;
	public Vector3 cameraTargetPos;

	[Header("Fading")]
	//public FadeController fCont;
	[SerializeField]
	//Animator animator;
	public int cameraHeight;

	[Header("Input")]
	public List<InputKey> inputKeys;

	public enum Inputs {interact}; //possible actions / interactions that we can do.
	
	private void Start() {
		inputMan = this;
		if (screenFade == null)
			screenFade = GetComponentInChildren<OVRScreenFade>();
	}

	// Update is called once per frame
	void Update () {
		fwd = pointer.transform.TransformDirection(Vector3.forward);
		text_trigger.text = "" + OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);

		if (ButtonWePressed(Inputs.interact)) {
			text_debug.text = ("working!!");
		} else {
			text_debug.text = ("not working!!");
		}

		//if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote)) { //making sure its the right one.
			//if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.One)) { //If we have the trigger down.
		if (ButtonWePressed(Inputs.interact)) {
			//TODO layermask
			if (objectToMove == null) { //Already have an object so dont have to fire again.
				RaycastHit hitInfo;

				if (Physics.Raycast(pointer.transform.position, fwd, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Interactable"))) {

					if (hitInfo.transform.tag == "Player"){
						PlayerUI = hitInfo.transform.GetComponent<PlayerStatusUI>();
						PlayerUI.CheckTriggerPress();
						PlayerUI.removePlayerStatus();
						return;
					}
					
					if (hitInfo.transform.tag == "Fumehood") { //clicking the fumehood :P
						if(hitInfo.transform.GetComponent<FumehoodSliderScript>() != null){
							fumeSlider = hitInfo.transform.GetComponent<FumehoodSliderScript>();
							//do function in fumehoodsliderscript that moves that glass.	
							fumeSlider.GlassSlide();
						}
						return;
					}

					if (hitInfo.transform.tag == "Teleport") { //teleport
						cameraTargetPos = new Vector3(hitInfo.transform.position.x, cameraHeight, hitInfo.transform.position.z);
						screenFade.StartCoroutine(screenFade.Fade(1, 0, 0.5f));
						MoveCamera();
						text_debug.text = "should teleport";
						return;
					}

					if(hitInfo.transform.tag == "UIButton") { //ui buttons
						Button tempButton = hitInfo.transform.GetComponent<Button>();
						IPointerClickHandler clickHandler = hitInfo.transform.GetComponent<IPointerClickHandler>();
						PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
						clickHandler.OnPointerClick(pointerEventData);
					}

					if(hitInfo.transform.tag == "ground") {
						return;
					}

					//text_debug.text = "Currently Hitting: " + hitInfo.transform.name;
					objectToMove = hitInfo.transform.gameObject;
					objectToMove.GetComponent<Rigidbody>().isKinematic = true;
					pointerStick.SetActive(false);

				}
			}
				
				Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);

				//MOVE THE OBJECT
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
		//}
	}

	public void MoveCamera() {
		transform.position = cameraTargetPos;
		//animator.SetTrigger("FadeOut");
	}

	//might be stupid and not work at all and super resource intensive but we're gonna do it anyway.
	public bool ButtonWePressed(Inputs input) {
		InputKey inputKey = new InputKey();

		for (int i = 0; i < inputKeys.Count; i++) {
			if(inputKeys[i].input == input) {
				inputKey = inputKeys[i];
			}
		}

		if (inputKey.getDown) {
			if (OVRInput.GetDown(inputKey.ocGO) || OVRInput.GetDown(inputKey.ocRift)) {
				return true;
			}
		} else {
			if (OVRInput.Get(inputKey.ocGO) || OVRInput.Get(inputKey.ocRift)) {
				return true;
			}
		}
		
		return false;
	}
}