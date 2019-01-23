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

	[Header("toggled by the buttons in-game")]
	public List<bool> requirementsPresent = new List<bool>(3);
	public List<bool> dangersPresent = new List<bool>();
	public List<bool> wasteBinUsed = new List<bool>(); //MAKE SURE ONLY 1 IS ACTIVE.

	[Header("the required variables for this experiment")]
	public List<bool> requirementsNecessary = new List<bool>(); //the ones actually needed.
	public List<bool> dangersNecessary = new List<bool>(); //SET IN THE INSPECTOR
	public List<bool> wastebinNecessary = new List<bool>();

	public ExperimentController.ExperimentType type;
	public ExperimentController.ExperimentDifficulty difficulty;
	#endregion

	#region Experiment Local Variables
	public float experimentTime;
	public float experimentFailureProbability; //percentage
	public float experimentFailureTimer;

	public GameObject textExperiment;

	public bool experimentGoingWrong;
	public bool allRequirementsPresent;
	public bool allDangersPresent;

	public bool experimentStarted;
	public bool experimentStopped;
	#endregion

	public List<Student> students = new List<Student>(); //local list of students to address.

	[Header("required variables for this experiment")]
	public ExperimentController.ExperimentDangers theActualDanger;
	public ExperimentController.ExperimentWasteBin theCorrectWastebin;
	
	private void Start() {
		if (experimentController == null) {
			experimentController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExperimentController>();
		}

		experimentFailureTimer = experimentController.checkTimer;
	}

	public void Update() {
		//checks every second for failure.
		if (experimentStarted) {
			textExperiment.SetActive(true);
			if (experimentFailureTimer < Time.time) {
				CheckForFailure();
				experimentFailureTimer = (int)Time.time + 1f;
			}
			experimentTime -= (int)Time.deltaTime;
		}

		if(experimentTime <= 0) {
			ExperimentStop();
		}
	}

	public void CheckForFailure() {
		//based on requirements, danger, type etc, it should be more or less difficult to complete the experiment.
		if(Random.Range(0f, 1f) < experimentFailureProbability * 100){ //TODO reset this to actual values?
			int randomStudent = Random.Range(0, students.Count);
			students[randomStudent].studentMovable = true; //sets a random student movable.
			switch (dangers[(int)theActualDanger]) { //TODO can be simplified.
				case ExperimentController.ExperimentDangers.Fire:
					students[randomStudent].ActivateParticles(0, true);
					break;
				case ExperimentController.ExperimentDangers.Acid:
					students[randomStudent].ActivateParticles(1, true);
					break;
				case ExperimentController.ExperimentDangers.Physical:
					students[randomStudent].ActivateParticles(2, true);
					break;
			}
			experimentGoingWrong = true;
		}
	}

	//[BUTTON] toggles the requirement on the card.
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

	//[BUTTON] Toggles the danger on the card
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

	//[BUTTON] which bin to use on card
	public void ToggleWastebin(int bin) {
		if (experimentStarted) {
			return;
		}

		//set each false, and the required to true.
		foreach (Student student in students) {
			for(int w = 0; w < wasteBinUsed.Count; w++) {
				student.ActivateWastebin(bin, false);
			}
			student.ActivateWastebin(bin, true);
		}
	}

	public void ExperimentStart() { //need to check this because this probably doesn't work, array doesn't resize correctly.
		if (allRequirementsPresent) {
			for(int i = 0; i < students.Count; i++) {
				if (!students[0].experimentStarted) {
					break;
				} else {
					experimentStarted = true;
				}
			}
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

		for (int w = 0; w < wastebinNecessary.Count; w++) {
			if (wastebinNecessary[w] && theCorrectWastebin == (ExperimentController.ExperimentWasteBin)w) {
				//if neccessary[w] is true and the correctwastebin is that same number, u good.
			} else {
				experimentFailureProbability++;
			}
		}
	}

	//set everything back to false
	public void ExperimentStop() {
		experimentStarted = false;
		experimentGoingWrong = false;
		foreach(Student student in students) {
			student.experimentStarted = false;
			student.studentMovable = false;
		}

		experimentStopped = true;
	}
}
