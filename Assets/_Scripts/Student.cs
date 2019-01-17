using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Student : MonoBehaviour {

	public GameObject[] requirements = new GameObject[3];
	public GameObject studentCard;
	public GameObject buttons_requirement;
	public GameObject buttons_danger;

	public Vector3 originalPosition;

	public Sprite[] dangerTextures;

	

	public Experiment myExperiment;

	private void Start() {
		originalPosition = transform.position;
	}

	public void ActivateRequirement(int requirement, bool activate){
	    requirements[requirement].SetActive(activate);
	}

	public void ActivateDanger(int danger, bool activate) {
		buttons_danger.transform.GetChild(danger).GetComponent<Image>().sprite = (activate ? dangerTextures[danger] : dangerTextures[danger + 3]); //activate? true : false (false + 3 because they will be after the coloured textures);
		
	}
	/*
	private void OnTriggerEnter(Collider other) {
		if(other.tag == "SnapOnPoint") {
			foreach (ExperimentController.ExperimentDangers danger in GameObject.Find("experiment").GetComponent<Experiment>().dangers)
				if (danger == other.GetComponent<SnapOnPoint>().danger &&
				other.GetComponent<SnapOnPoint>().danger == GameObject.Find("experiment").GetComponent<Experiment>().dangers[GameObject.Find("experiment").GetComponent<Experiment>().currentDanger]){
					Debug.Log(danger);
				}
			}
			//snap to point of said danger.
			//get rid of danger (disable the acid fire/burn/bleed)
			//be able to pick em back up again (or click on them to move back to originalPosition)
	}*/
}
