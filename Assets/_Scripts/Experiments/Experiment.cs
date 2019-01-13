using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Experiment : MonoBehaviour {

	public ExperimentController experimentController; //TODO not sure if needed.

	#region Experiment Controller Variables
	public List<ExperimentController.ExperimentDangers> dangers = new List<ExperimentController.ExperimentDangers>();
	[Header("DO NOT MESS UP THE ORDER. USE THE ORDER AS DESCRIBED IN EXPERIMENTCONTROLLER!")]
	public List<ExperimentController.ExperimentRequirements> requirements = new List<ExperimentController.ExperimentRequirements>(); //required for this experiment
	public List<bool> requirementsPresent = new List<bool>();
	public ExperimentController.ExperimentType type;
	public ExperimentController.ExperimentDifficulty difficulty;
	#endregion

	#region Experiment Local Variables
	public float experimentTime;
	public float experimentFailureProbability; //percentage
	float experimentFailureTimer;

	public bool experimentGoingWrong;
	public bool allRequirementsPresent;
	#endregion

	public List<StudentScript> students = new List<StudentScript>();

	//In ToggleRequirement


	private void Start() {
		if (experimentController == null) {
			experimentController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExperimentController>();
		}

		experimentFailureTimer = experimentController.checkTimer;
		//requirementsPresent.Capacity = requirements.Count; //should set the list to x requirements.
	}

	public void Update() {
		//checks every second for failure.
		if(experimentFailureTimer < Time.time) {
			CheckForFailure();
			experimentFailureTimer = (int)Time.time + 1f;
		}
	}

	public void CheckForFailure() {
		//based on requirements, danger, type etc, it should be more or less difficult to complete the experiment.
		
	}

	//toggles the requirement if it's true or false.
	public void ToggleRequirement(int requirement) { 
		if (requirementsPresent[requirement]) {
			requirementsPresent[requirement] = false;
			foreach(StudentScript student in students){
    			student.ActivateRequirement(requirement, false);
			}
		} else {
			requirementsPresent[requirement] = true;
			foreach(StudentScript student in students){
    			student.ActivateRequirement(requirement, true);
			}
		}

		if (requirementsPresent.All (x => x)) { //if all true
			allRequirementsPresent = true;
		} else {
			//every requirement is not present!!
			allRequirementsPresent = false;
		}
	}

	public void ExperimentStart() {
		if (allRequirementsPresent) {
			
		}
	}
}
