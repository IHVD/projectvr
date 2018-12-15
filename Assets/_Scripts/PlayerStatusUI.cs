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
		if(theBackground == null)
			theBackground = transform.GetChild(0).GetComponent<Image>();
		if(theName == null)
			theName = transform.GetChild(1).GetComponent<Text>();
		if(doing == null)
			doing = transform.GetChild(2).GetComponent<Text>();
		if(theButton == null)
			theButton = transform.GetChild(3).GetComponent<Image>();
		if(theButtonText == null) 
			theButtonText = transform.GetChild(3).GetChild(0).GetComponent<Text>();
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
