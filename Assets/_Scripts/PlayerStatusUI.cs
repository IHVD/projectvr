using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour {

	public GameObject theCanvas;

	public string displayText;
	public string displayButtonGloves;
	public string displayButtonGlasses;
	public string displayButtonCoat;
	public string displayDoing;
	public Text theName;
	public Image theButtonGloves;
	public Image theButtonGlasses;
	public Image theButtonCoat;
	public Image theBackground;
	public Text theGlovesText;
	public Text theGlassesText;
	public Text theCoatText;
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
		if(theButtonGloves == null)
			theButtonGloves = transform.GetChild(3).GetComponent<Image>();
		if(theButtonGlasses == null)
			theButtonGlasses = transform.GetChild(4).GetComponent<Image>();
		if(theButtonCoat == null)
			theButtonCoat = transform.GetChild(5).GetComponent<Image>();
		if(theGlovesText == null) 
			theGlovesText = transform.GetChild(3).GetChild(0).GetComponent<Text>();
		if(theGlassesText == null) 
			theGlassesText = transform.GetChild(4).GetChild(0).GetComponent<Text>();
		if(theCoatText == null) 
			theCoatText = transform.GetChild(5).GetChild(0).GetComponent<Text>();
		
		removePlayerStatus ();
	}

	public void CheckTriggerPress()
	{
		displayInfo = !displayInfo;
	}

	public void removePlayerStatus(){
		if(displayInfo == true){
			theCanvas.SetActive(true);
		}
		else{
			theCanvas.SetActive(false);
		}
	}
}
