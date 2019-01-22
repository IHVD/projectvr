using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Student : MonoBehaviour {

	public GameObject[] requirements = new GameObject[3];
	public GameObject[] particles = new GameObject[3];
	public GameObject studentCard;
	public GameObject buttons_requirement;
	public GameObject buttons_danger;
	public GameObject buttons_waste;

	public Vector3 originalPosition;
	public Quaternion originalRotation;

	public Sprite[] dangerTextures;
	public Sprite[] wasteTextures;

	public bool studentMovable;
	public bool inSnapPoint;

	public float timeForSnapResolve;

	public Experiment myExperiment;

	private void Start() {
		originalPosition = transform.position;
		originalRotation = transform.rotation;
	}

	public void ActivateRequirement(int requirement, bool activate) {
		requirements[requirement].SetActive(activate);
	}

	public void ActivateDanger(int danger, bool activate) {
		buttons_danger.transform.GetChild(danger).GetComponent<Image>().sprite = (activate ? dangerTextures[danger] : dangerTextures[danger + 3]); //activate? true : false (false + 3 because they will be after the coloured textures);
	}

	public void ActivateWastebin(int bin, bool activate) {
		buttons_waste.transform.GetChild(bin).GetComponent<Image>().sprite = (activate ? dangerTextures[bin] : dangerTextures[bin + 3]); //activate? true : false (false + 3 because they will be after the coloured textures);
	}

	public void ActivateParticles(int danger, bool activate) {
		particles[danger].SetActive(activate);
	}

	private void OnTriggerEnter(Collider other) {
		if(other.tag == "SnapOnPoint") {
			//snap to point of said danger.
			//get rid of danger (disable the acid fire/burn/bleed)
			//be able to pick em back up again (or click on them to move back to originalPosition)

			SnapOnPoint snap = other.GetComponent<SnapOnPoint>();
			if(snap.danger == myExperiment.theActualDanger) {
				transform.position = other.transform.position;
				studentMovable = false;
				GameController.gCont.inputManager.objectToMove.transform.parent = null;
				GameController.gCont.inputManager.objectToMove = null;
				inSnapPoint = true;

				//start a timer.
				StartCoroutine(DangerResolver());
			} else {
				SnapDone();
			}
		}
		if(other.tag == "boundaryPlane") {
			SnapDone();
		}
	}

	IEnumerator DangerResolver() {
		yield return new WaitForSeconds(timeForSnapResolve);
		SnapDone();
	}

	void SnapDone() {
		transform.position = originalPosition;
		transform.rotation = originalRotation;
		inSnapPoint = false;
		studentMovable = true;
	}
}
