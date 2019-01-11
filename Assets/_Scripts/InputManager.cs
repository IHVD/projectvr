using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	bool GlovesOn = false;
	bool CoatOn = false;
	bool GlassesOn = false;

	private void Start() {
		inputMan = this;
		if (screenFade == null)
			screenFade = GetComponentInChildren<OVRScreenFade>();
	}

	// Update is called once per frame
	void Update () {
		fwd = pointer.transform.TransformDirection(Vector3.forward);
		text_trigger.text = "" + OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);

		if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote)) { //making sure its the right one.
			if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) { //If we have the trigger down.

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

						if (hitInfo.transform.tag == "ground") { //teleport
							cameraTargetPos = hitInfo.point + hitInfo.normal * cameraHeight;
							screenFade.StartCoroutine(screenFade.Fade(1, 0, 0.5f));
							MoveCamera();
							return;
						}

						//------------------FOR GLOVES-----------------------//
						if (hitInfo.transform.tag == "UIButtonGloves" && GlovesOn == false){
							text_debug.text = "Currently Hitting: " + hitInfo.transform.name;
							GlovesOn = true;
							GetComponent<Image>().color = Color.black;
							return;
						}
						else if(hitInfo.transform.tag == "UIButtonGloves" && GlovesOn == true)
						{
							text_debug.text = "Currently Hitting: " + hitInfo.transform.name;
							GlovesOn = false;
							GetComponent<Image>().color = Color.white;
							return;
						}

						//------------------FOR GLASSES-----------------------//
						if (hitInfo.transform.tag == "UIButtonGlasses" && GlassesOn == false){
							text_debug.text = "Currently Hitting: " + hitInfo.transform.name;
							GlassesOn = true;
							GetComponent<Image>().color = Color.black;
							return;
						}
						else if(hitInfo.transform.tag == "UIButtonGlasses" && GlassesOn == true)
						{
							text_debug.text = "Currently Hitting: " + hitInfo.transform.name;
							GlassesOn = false;
							GetComponent<Image>().color = Color.white;
							return;
						}

						//------------------FOR COATS-----------------------//
						if (hitInfo.transform.tag == "UIButtonCoat" && CoatOn == false){
							text_debug.text = "Currently Hitting: " + hitInfo.transform.name;
							CoatOn = true;
							GetComponent<Image>().color = Color.black;
							return;
						}
						else if(hitInfo.transform.tag == "UIButtonCoat" && CoatOn == true)
						{
							text_debug.text = "Currently Hitting: " + hitInfo.transform.name;
							CoatOn = false;
							GetComponent<Image>().color = Color.white;
							return;
						}

						text_debug.text = "Currently Hitting: " + hitInfo.transform.name;
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
		}
	}

	public void MoveCamera() {
		transform.position = cameraTargetPos;
		//animator.SetTrigger("FadeOut");
	}
}

