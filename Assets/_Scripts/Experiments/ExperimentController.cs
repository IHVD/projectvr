using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentController : MonoBehaviour {

	public enum ExperimentType {Biological, Chemical};
	public enum ExperimentDangers {Fire, Acid, Physical};
	[SerializeField]
	public enum ExperimentRequirements {Glasses, LabCoat, Gloves, HairTucked}
	public enum ExperimentDifficulty {Easy, Middle, Hard};

	public float checkTimer;

	public void InitiateNewExperiment() {
		//instantiate new experiment
		Experiment experiment;
	}
}
