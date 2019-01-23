using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExperimentController : MonoBehaviour {

	public enum ExperimentType {Biological, Chemical};
	public enum ExperimentDangers {Fire, Acid, Physical};
	[SerializeField]
	public enum ExperimentRequirements {Gloves, LabCoat, Glasses, HairTucked}
	public enum ExperimentDifficulty {Easy, Middle, Hard};
	public enum ExperimentWasteBin { Flammable, Acidic, TheOtherOne};

	public List<Experiment> allExperiments = new List<Experiment>();

	public float checkTimer;

	public void CheckAllExperiments() {
		bool activate = false;
		
		for(int e = 0; e < allExperiments.Count; e++) {
			if (!allExperiments[e].experimentStopped) {
				activate = false;
				break;
			}
			activate = true;
		}
		
		if (activate) {
			//ActivateEndPhase();
            GameController.gCont.theGameOverManager.EndPhaseStart();
           
        }
	}
	public void restartGame() {
        Time.timeScale = 1f;
        Application.LoadLevel(Application.loadedLevel);
	}

}
