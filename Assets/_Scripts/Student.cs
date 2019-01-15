using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Student : MonoBehaviour {

	public GameObject[] requirements = new GameObject[3];
	public GameObject studentCard;
	public GameObject buttons_requirement;
	public GameObject buttons_danger;

	public Sprite[] dangerTextures;

	public void ActivateRequirement(int requirement, bool activate){
	    requirements[requirement].SetActive(activate);
	}

	public void ActivateDanger(int danger, bool activate) {
		buttons_danger.transform.GetChild(danger).GetComponent<Image>().sprite = (activate ? dangerTextures[danger] : dangerTextures[danger + 3]); //activate? true : false (false + 3 because they will be after the coloured textures);
	}
}
