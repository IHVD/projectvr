using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameVariables gVars;
	public static GameController gCont;

	public InputManager inputManager;

    public GameOverManager theGameOverManager;

	public float score = 2;
	public float scoreIncreasePerSecond = 3;

	public void Update()
	{
		score += scoreIncreasePerSecond * Time.deltaTime;
	}
	public void Start() {
        theGameOverManager = FindObjectOfType<GameOverManager>();
        gCont = this;
		gVars.StartCoroutine(gVars.CSVLoad(gVars.urlToDownload));
	}

	public void ApplySettings() {
		inputManager.velocityMultiplier = gVars.downloadedVariables["VelocityMultiplier"];
	}
}
