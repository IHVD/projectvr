using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyScript : MonoBehaviour {

	bool isReady;
	public GameObject readyCanvas;

	public void clickedReady() {
		bool isReady = true;
		readyCanvas.SetActive(false);
	}
}
