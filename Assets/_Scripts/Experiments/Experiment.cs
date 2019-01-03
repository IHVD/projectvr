using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment : MonoBehaviour {

	public ExperimentController experimentController; //TODO not sure if needed.

	#region Experiment Controller Variables
	public List<ExperimentController.ExperimentDangers> dangers = new List<ExperimentController.ExperimentDangers>();
	public List<ExperimentController.ExperimentRequirements> requirements = new List<ExperimentController.ExperimentRequirements>();
	public ExperimentController.ExperimentType type;
	public ExperimentController.ExperimentDifficulty difficulty;
	#endregion

	#region Experiment Local Variables
	public float ExperimentTime;
	public float ExperimentFailureProbability; //percentage
	public bool ExperimentGoingWrong;
	#endregion

	private void Start() {
		if (experimentController == null) {
			experimentController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExperimentController>();
		}
	}

	public void Update() {
		
	}

	public void CheckForFailure() {
		//based on requirements, danger, type etc, it should be more or less difficult to complete the experiment.
	}
}
