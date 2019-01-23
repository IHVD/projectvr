using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentController : MonoBehaviour {

	public enum ExperimentType {Biological, Chemical};
	public enum ExperimentDangers {Fire, Acid, Physical};
	[SerializeField]
	public enum ExperimentRequirements {Gloves, LabCoat, Glasses, HairTucked}
	public enum ExperimentDifficulty {Easy, Middle, Hard};
	public enum ExperimentWasteBin { Flammable, Acidic, TheOtherOne};

	public List<Experiment> allExperiments = new List<Experiment>();

	public float checkTimer;

	
}
