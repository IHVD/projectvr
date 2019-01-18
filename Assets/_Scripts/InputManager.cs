﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
	private float pointerLength;
	public Text text_debug;
	public Text text_trigger;
	public Text text_rotation;
	[SerializeField] private LineRenderer pointerLine;

	[Header("Throwing Object")]
	public float velocityMultiplier;
	public GameObject objectToMove;
	public Rigidbody objectToMoveRb;
	Vector3 prevPos;
	Quaternion prevRotation;

	[Header("Raycast")]
	LayerMask layerMask = 1 << 9;
	Vector3 fwd;
	public Vector3 cameraTargetPos;
	public GameObject pointerHit;

	[Header("Fading")]
	//public FadeController fCont;
	[SerializeField]
	//Animator animator;
	public int cameraHeight;

	[Header("Input")]
	public List<InputKey> inputKeys;

	public enum Inputs {interact}; //possible actions / interactions that we can do.
	
	private sfxManager theSFXManager;
	// ADDING sfxManager TO A SCRIPT: you need to add a "private sfxManager theSFXManager;" E.g. on the line above.
	// Then adding "theSFXManager = FindObjectOfType<sfxManager>();" in the script's start function.
	// ADDING SFX:
	// theSFXManager.PlaySound(theSFXManager.clickSFX);
	// Or any other sfx, which you can find in the manager script.
	// Or by simply going to the sfxManager GameObject in the hierarchy and reading them from the left side.

	private void Start() {
		theSFXManager = FindObjectOfType<sfxManager>();
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

		//raycast for the pointer thing
		RaycastHit hit;
		if (Physics.Raycast(pointer.transform.position, fwd, out hit, Mathf.Infinity)) { //TODO I dont want to be using a continuous raycast for this buy maybe we can anyway if it doesnt affect performance anyway
			pointerLength = hit.distance;
			//Debug.Log(hit.transform.name);
		}

		UpdatePointer();

		if (ButtonWePressed(Inputs.interact)) {
		//TODO layermask
			if (objectToMove == null) { //Already have an object so dont have to fire again.
				RaycastHit hitInfo;

				if (Physics.Raycast(pointer.transform.position, fwd, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Interactable"))) { //TODO change range
					

					if (hitInfo.transform.tag == "Player"){
						Student student = hitInfo.transform.GetComponent<Student>();
						if (!student.myExperiment.experimentStarted) {
							PlayerUI = hitInfo.transform.GetComponent<PlayerStatusUI>();
							PlayerUI.removePlayerStatus();
						}
						return;
					}
					
					if (hitInfo.transform.tag == "Fumehood") { //clicking the fumehood :P
						if(hitInfo.transform.GetComponent<FumehoodSliderScript>() != null){
							fumeSlider = hitInfo.transform.GetComponent<FumehoodSliderScript>();
							fumeSlider.GlassSlide();
						}
						return;
					}

					if (hitInfo.transform.tag == "Teleport") { //teleport
						cameraTargetPos = new Vector3(hitInfo.transform.position.x, cameraHeight, hitInfo.transform.position.z);
						screenFade.StartCoroutine(screenFade.Fade(1, 0, 0.5f));
						MoveCamera();
						return;
					}

					if(hitInfo.transform.tag == "UIButton") { //ui buttons
						Button tempButton = hitInfo.transform.GetComponent<Button>();
						IPointerClickHandler clickHandler = tempButton.GetComponent<IPointerClickHandler>();
						PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
						clickHandler.OnPointerClick(pointerEventData);
						return;
					}

					if(hitInfo.transform.tag == "ground") {
						return;
					}

					text_debug.text = "Currently Hitting: " + hitInfo.transform.name;
					objectToMove = hitInfo.transform.gameObject;
					objectToMoveRb = objectToMove.GetComponent<Rigidbody>();

					//if (prevRotation != prevRotation) {
						prevRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
					//}

					if (objectToMoveRb != null) {
						Debug.Log("got this part");
						Debug.Log(objectToMove);
						objectToMoveRb.isKinematic = true;
					}
				}
			} else { 
				Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
				//MOVE THE OBJECT
				Debug.Log("are u there? " + objectToMove == null);
				if(objectToMove.tag != "Player") {
					objectToMove.transform.parent = pointerStick.transform.parent; //works because it gets parented, so it follows rotation etc from the controller.
				} else {
					Student student = objectToMove.GetComponent<Student>();
					if (student.studentMovable) { //only movable when the experiment has started and is movable. //TODO ADD EXPERIMENT STARTED!!
						Quaternion direction = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote) * Quaternion.Inverse(prevRotation);
						objectToMove.transform.position += new Vector3(direction.x, objectToMove.transform.position.y, direction.z);
					}
				}
				
			}
		} else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger)) {
			if (objectToMove != null) {
				objectToMove.transform.parent = null;

				if (objectToMoveRb != null)
				objectToMoveRb.isKinematic = false;

				Vector3 currPos = objectToMove.transform.position;

				
				objectToMoveRb.velocity = (currPos - prevPos) / Time.deltaTime * velocityMultiplier / 2;
				
				
				objectToMove = null;
				pointerStick.SetActive(true);
			}
		}

		if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)){
			if (objectToMove != null) {
				Vector3 currPos = objectToMove.transform.position;
				prevPos = currPos;
			}
		}

		//To see the rotational debug stuff.
		text_rotation.text = "Q: " + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote) + "\n" + "V: " + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).eulerAngles;
		
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

	public void UpdatePointer() {
		pointerLine.SetPosition(0, pointer.transform.position);
		pointerLine.SetPosition(1, pointer.transform.position + fwd * pointerLength);
	}
}