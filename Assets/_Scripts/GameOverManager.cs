using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour {

	//variable for time 
	public int ExperimentTimeLeft = 200;

	public GameObject EndPhaseCanvas;

	//acces to the score text 
	public Text scoreValue;

	public void EndPhaseStart() {
        Time.timeScale = 0f;
        EndPhaseCanvas.SetActive(true);		
	}
	// Use this for initialization
	void Update () {
		//set the text of the score
		scoreValue.text = "Experiments Wrong: " + GameController.gCont.experimentsGoneWrong + "\n Accidents Fixed: " + GameController.gCont.accidentsFixed;
	}


	//put on restart button so game resets properly
	public void RestartGame(){
		
	}
}
