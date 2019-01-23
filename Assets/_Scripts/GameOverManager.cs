﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour {

	//acces to the score text 
	public Text scoreValue;

	// Use this for initialization
	void Update () {
		//set the text of the score
		scoreValue.text = GameController.gCont.score.ToString();
	
	}
	
	// Update is called once per frame

	//put on restart button so game resets properly
	public void RestartGame(){
		//Experiment.ScoreInstance.ResetGameVoidWeNeedToMake <--- resets score and puts player back in starting point, only puts back in starting point if the canvas stays in the same scene
		//SceneManager.LoadScene("SceneWeNeedToLoadToRestartTheGame"); <--- this is only if we make the end screen in a new scene
	}
}