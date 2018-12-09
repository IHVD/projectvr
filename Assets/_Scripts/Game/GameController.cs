using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameVariables gVars;
	public static GameController gCont;

	public InputManager inputManager;

	public void Start() {
		gCont = this;
		gVars.StartCoroutine(gVars.CSVLoad(gVars.urlToDownload));
	}

	public void ApplySettings() {
		inputManager.velocityMultiplier = gVars.downloadedVariables["VelocityMultiplier"];
	}
}
