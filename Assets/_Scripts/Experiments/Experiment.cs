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

	public List<bool> requirementsPresent = new List<bool>(3); //these are active or inactive based on the button presses
	public List<bool> dangersPresent = new List<bool>();

	[Header("These are the ones actually needed to be set")]
	public List<bool> requirementsNecessary = new List<bool>(); //the ones actually needed.
	public List<bool> dangersNecessary = new List<bool>(); //SET IN THE INSPECTOR

	public ExperimentController.ExperimentType type;
	public ExperimentController.ExperimentDifficulty difficulty;
	#endregion

	#region Experiment Local Variables
	public float experimentTime;
	public float experimentFailureProbability; //percentage
	float experimentFailureTimer;

	public bool experimentGoingWrong;
	public bool allRequirementsPresent;
	public bool allDangersPresent;

	public bool experimentStarted;
	#endregion

	public List<Student> students = new List<Student>(); //local list of students to address.

	public ExperimentController.ExperimentDangers theActualDanger;

	private void Start() {
		if (experimentController == null) {
			experimentController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExperimentController>();
		}

		experimentFailureTimer = experimentController.checkTimer;
	}

	public void Update() {
		//checks every second for failure.
		if (experimentStarted) {
			if (experimentFailureTimer < Time.time) {
				CheckForFailure();
				experimentFailureTimer = (int)Time.time + 1f;
			}
		}
	}

	//TODO make this.
	public void CheckForFailure() {
		//based on requirements, danger, type etc, it should be more or less difficult to complete the experiment.
		if(Random.Range(0f, 100f) < experimentFailureProbability){
			int randomStudent = Random.Range(0, students.Count);
			students[randomStudent].studentMovable = true; //sets a random student movable.
			switch (dangers[(int)theActualDanger]) {
				case ExperimentController.ExperimentDangers.Fire:
					//light randomstudent on fire
					break;
				case ExperimentController.ExperimentDangers.Acid:
					//go burn yourself randomstudent
					break;
				case ExperimentController.ExperimentDangers.Physical:
					//bleed to death bitch randomstudent
					break;
			}
		}
	}

	//toggles the requirement if it's true or false.
	public void ToggleRequirement(int requirement) {
		if (experimentStarted) {
			return;
		}

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
	}

	//Toggles if the person should be aware of dangers.
	public void ToggleDanger(int danger) {
		if (experimentStarted) {
			return;
		}

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

	public void ExperimentStart() { //need to check this because this probably doesn't work, array doesn't resize correctly.
		if (allRequirementsPresent) {
			experimentStarted = true;
		}

		for(int r = 0; r < requirementsNecessary.Count; r++) {
			if(requirementsPresent[r] == requirementsNecessary[r] || !requirementsPresent[r] == !requirementsNecessary[r]) {
				//if its the same
				allRequirementsPresent = true;
			} else {
				experimentFailureProbability++;
			}
		}

		for (int d = 0; d < dangers.Count; d++) {
			if (dangersPresent[d] == dangersNecessary[d] || !dangersPresent[d] == !dangersNecessary[d]) {
				//if its the same
				allDangersPresent = true;
			} else {
				experimentFailureProbability++;
			}
		}
	}
}
