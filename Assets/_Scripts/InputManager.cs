using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    public float movementSpeed;
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
	
	public bool touchingBook;
	public float bookPage = 0;
	public float amountBookTouched = 0;
	private sfxManager theSFXManager;
	private bookScript theBookScript;
	// ADDING sfxManager TO A SCRIPT: you need to add a "private sfxManager theSFXManager;" E.g. on the line above.
	// Then adding "theSFXManager = FindObjectOfType<sfxManager>();" in the script's start function.
	// ADDING SFX:
	// theSFXManager.PlaySound(theSFXManager.clickSFX);
	// Or any other sfx, which you can find in the manager script.
	// Or by simply going to the sfxManager GameObject in the hierarchy and reading them from the left side.

	private void Start() {
		theBookScript = FindObjectOfType<bookScript>();	
		theSFXManager = FindObjectOfType<sfxManager>();
		inputMan = this;
		if (screenFade == null)
			screenFade = GetComponentInChildren<OVRScreenFade>();
	}

	// Update is called once per frame
	void Update () {
		fwd = pointer.transform.TransformDirection(Vector3.forward);

		//raycast for the pointer thing
		RaycastHit hit;
		if (Physics.Raycast(pointer.transform.position, fwd, out hit, Mathf.Infinity)) {
			pointerLength = hit.distance;
		}

		UpdatePointer();
		if (ButtonWePressed(Inputs.interact)) {
	
			if (objectToMove == null) { //Already have an object so dont have to fire again.
				RaycastHit hitInfo;

				if (Physics.Raycast(pointer.transform.position, fwd, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Interactable"))) {
					if (hitInfo.transform.tag == "Player"){
						Student student = hitInfo.transform.GetComponent<Student>();
						if(!student.myExperiment.experimentStarted && !student.experimentStarted && !student.myExperiment.experimentStopped) {
							CloseStudentUI(student);
						} else { //so if it did start;
							if (student.myExperiment.experimentGoingWrong && student.studentMovable) { //and the experiment is going wrong;
								//be able to pick up the student.
								objectToMove = student.transform.gameObject;
								objectToMoveRb =  objectToMove.AddComponent<Rigidbody>();
								if (objectToMoveRb != null) {
									objectToMoveRb.useGravity = false;
								}
								//objectToMoveRb = objectToMove.GetComponent<Rigidbody>();
								//objectToMove.transform.parent = pointer.transform;
								objectToMove.transform.position = new Vector3(	objectToMove.transform.position.x + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).y * movementSpeed * Time.deltaTime, 
																				objectToMove.transform.position.y, 
																				objectToMove.transform.position.z + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).x * movementSpeed * Time.deltaTime);
								//objectToMove.transform.rotation = pointer.transform.rotation;
								//objectToMove.transform.LookAt(pointer.transform.parent);
								if (objectToMoveRb != null) {
									objectToMoveRb.isKinematic = true;
								}
							}
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
						theSFXManager.PlaySound(theSFXManager.pickup1SFX);
						MoveCamera();
						return;
					}

					if (hitInfo.transform.tag == "UIButton") { //ui buttons
						Button tempButton = hitInfo.transform.GetComponent<Button>();
						IPointerClickHandler clickHandler = tempButton.GetComponent<IPointerClickHandler>();
						PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
						clickHandler.OnPointerClick(pointerEventData);
						theSFXManager.PlaySound(theSFXManager.clickSFX);
						//play sound effect (button click);
						return;
					}


					if (hitInfo.transform.tag == "Book" && theBookScript.restartTutorial == false) {
						// Not movable, otherwise people will lose the book
						/*objectToMove = hitInfo.transform.gameObject;
						objectToMoveRb = objectToMove.GetComponent<Rigidbody>();
						objectToMove.transform.parent = pointer.transform;
						if (objectToMoveRb != null) {
							objectToMoveRb.isKinematic = true;
						}*/
						touchingBook = true;
						bookPage += 1;
						amountBookTouched = 1;
						theSFXManager.PlaySound(theSFXManager.impact1SFX);
						return;
					} else if(theBookScript.restartTutorial == true) {
						touchingBook = false;
						bookPage = 0;
						amountBookTouched = 0;
					} else {
						touchingBook = false;
						amountBookTouched = 0;
						return;						
					}

					if (hitInfo.transform.tag == "ground") {
						return;
					}

					if(hitInfo.transform.tag == "Movable") {

						objectToMove = hitInfo.transform.gameObject;
						objectToMoveRb = objectToMove.GetComponent<Rigidbody>();
						objectToMove.transform.parent = pointer.transform;
						if (objectToMoveRb != null) {
							objectToMoveRb.isKinematic = true;
						}
						return;
					}


					//if (prevRotation != prevRotation) {
						prevRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
					//}

				} //end of raycast
			} else { 
				Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote); //does this even get used tho?
																														 //MOVE THE OBJECT
				if (objectToMove.tag != "Player") {
					objectToMove.transform.parent = pointer.transform; //works because it gets parented, so it follows rotation etc from the controller.
				} else {
					objectToMove.transform.position = new Vector3(objectToMove.transform.position.x + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).y * movementSpeed * Time.deltaTime,
																				objectToMove.transform.position.y,
																				objectToMove.transform.position.z + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).x * movementSpeed * Time.deltaTime);
				}
			}
		} else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger)) { //let go object
			if (objectToMove != null) {
				objectToMove.transform.parent = null;
				Vector3 currPos = objectToMove.transform.position;
				if (objectToMoveRb != null) {
					objectToMoveRb.isKinematic = false;
					objectToMoveRb.velocity = (currPos - prevPos) / Time.deltaTime * velocityMultiplier;
				}
				

				if (objectToMove.tag == "Player") {
					Student student = objectToMove.GetComponent<Student>();
					objectToMoveRb = null;
					Destroy(objectToMove.GetComponent<Rigidbody>()); //should theoretically remove the rigidbody component.

					student.transform.position = student.originalPosition;
					student.inSnapPoint = false;
					student.studentMovable = true;
					//student.transform.rotation = student.originalRotation;
				}

				objectToMove = null;
				//pointerStick.SetActive(true);
			}
		}

		if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)){
			if (objectToMove != null) {
				if(objectToMove.tag == "Player") {
					objectToMove.transform.position = new Vector3(objectToMove.transform.position.x + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).y * movementSpeed * Time.deltaTime,
																				objectToMove.transform.position.y,
																				objectToMove.transform.position.z + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).x * movementSpeed * Time.deltaTime);
				}
				Vector3 currPos = objectToMove.transform.position;
				prevPos = currPos;
			}
		}
	}

	public void MoveCamera() {
		transform.position = cameraTargetPos;
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

	public void CloseStudentUI(Student student) {
		student.transform.GetComponent<PlayerStatusUI>().removePlayerStatus();
	}
}