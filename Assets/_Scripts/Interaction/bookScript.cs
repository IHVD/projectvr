using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookScript : MonoBehaviour {

	public GameObject text1;
	public GameObject text2;
	public GameObject text3;
	public GameObject text4;
	public GameObject text5;

	public bool restartTutorial;
	public bookScript theBookScript;
	public GameObject theActualBook;
	public Vector3 bookOriginalPosition;
	public Quaternion bookOriginalRotation;	

	private InputManager theInputManager;

	// Use this for initialization
	void Start () {
		theInputManager = FindObjectOfType<InputManager>();
		theBookScript = FindObjectOfType<bookScript>();
		bookOriginalPosition = theActualBook.transform.position;
		bookOriginalRotation = theActualBook.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (theInputManager.bookPage == 0) {
			restartTutorial = false;
		}		
		if (theInputManager.bookPage == 1) {
			disableAllText();
			text1.SetActive(true);
		}
		if (theInputManager.bookPage == 2) {
			disableAllText();
			text2.SetActive(true);
		}	
		if (theInputManager.bookPage == 3) {
			disableAllText();
			text3.SetActive(true);
		}	
		if (theInputManager.bookPage == 4) {
			disableAllText();
			text4.SetActive(true);
		}	
		if (theInputManager.bookPage == 5) {
			disableAllText();
			text5.SetActive(true);
		} 		
		if (theInputManager.bookPage >= 6) {
			disableAllText();
			restartTutorial = true;
		}								
	}

	void disableAllText() {
		text1.SetActive(false);
		text2.SetActive(false);
		text3.SetActive(false);
		text4.SetActive(false);
		text5.SetActive(false);
	}

}