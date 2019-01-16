using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Experiment : MonoBehaviour {

	public ExperimentController experimentController; //TODO not sure if needed.

	#region Experiment Controller Variables
	[Header("DO NOT CHANGE ORDER OF VALUES IN DANGER/REQUIRE!")]
	public List<ExperimentController.ExperimentDangers> dangers = new List<ExperimentController.ExperimentDangers>();
	public List<ExperimentController.ExperimentRequirements> requirements = new List<ExperimentController.ExperimentRequirements>(); //necessary requirements for this experiment

	public List<bool> requirementsPresent = new List<bool>(3); //should be equal to how many requirements there are.
	public List<bool> dangersPresent = new List<bool>(); //SET IN THE INSPECTOR

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

	public List<Student> students = new List<Student>(); //local list of students to address.

	private void Start() {
		if (experimentController == null) {
			experimentController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExperimentController>();
		}

		experimentFailureTimer = experimentController.checkTimer;

		//requirementsPresent = new List<bool>(requirements.Capacity);
		//requirementsPresent = new List<bool>(3);
		Debug.Log("started with " + requirementsPresent.Count + " while it should be " + requirements.Capacity + " or maybe " + requirements.Count);
	}

	public void Update() {
		//checks every second for failure.
		if(experimentFailureTimer < Time.time) {
			CheckForFailure();
			experimentFailureTimer = (int)Time.time + 1f;
		}
	}

	//TOOD make this.
	public void CheckForFailure() {
		//based on requirements, danger, type etc, it should be more or less difficult to complete the experiment.
		
	}

	//toggles the requirement if it's true or false.
	public void ToggleRequirement(int requirement) {

		Debug.Log(requirement);
		Debug.Log(requirementsPresent.Count);

		if (requirementsPresent[requirement]) {
			requirementsPresent[requirement] = false;
			foreach(Student student in students){
    			student.ActivateRequirement(requirement, false);
			}
		} else {
			requirementsPresent[requirement] = true;
			foreach(Student student in students){
    			student.ActivateRequirement(requirement, true);
			}
		}

		if (requirementsPresent.All(x => x)) { //if all true
			allRequirementsPresent = true;
		} else {
			//every requirement is not present!!
			allRequirementsPresent = false;
		}
	}

	//Toggles if the person should be aware of dangers.
	public void ToggleDanger(int danger) {
		if (dangersPresent[danger]) {
			dangersPresent[danger] = false;
			foreach (Student student in students) {
				student.ActivateDanger(danger, false);
			}
		} else {
			dangersPresent[danger] = true;
			foreach (Student student in students) {
				student.ActivateDanger(danger, true);
			}
		}
	}

	public void ExperimentStart() {
		if (allRequirementsPresent) {
			//start the experiment!
		}
	}
}
