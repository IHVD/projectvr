using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour {

	public string displayText;
	public string displayButton;
	public string displayDoing;
	public Text theName;
	public Image theButton;
	public Image theBackground;
	public Text theButtonText;
	public Text doing;
	public bool displayInfo = false;
	public Camera my_camera;

	// Use this for initialization
	void Start () {
		theName = GameObject.Find("Name").GetComponent<Text>();
		theButtonText = GameObject.Find("ButtonText").GetComponent<Text>();
		doing = GameObject.Find("Doing").GetComponent<Text>();
		theButton = GameObject.Find("Button").GetComponent<Image>();
		theBackground = GameObject.Find("Background").GetComponent<Image>();
		removePlayerStatus ();
	}

	public void CheckTriggerPress()
	{
		displayInfo = !displayInfo;
	}

	public void removePlayerStatus(){
		if(displayInfo == true){
			theName.text = displayText;
			theName.gameObject.SetActive(true);

			theButtonText.text = displayButton;
			theButtonText.gameObject.SetActive(true);

			doing.text = displayDoing;
			doing.gameObject.SetActive(true);

			theButton.gameObject.SetActive(true);

			theBackground.gameObject.SetActive(true);
		}
		else{
			theName.gameObject.SetActive(false);
			theButtonText.gameObject.SetActive(false);
			theButton.gameObject.SetActive(false);
			doing.gameObject.SetActive(false);
			theBackground.gameObject.SetActive(false);
		}
	}
}
