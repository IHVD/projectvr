using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentScript : MonoBehaviour {

public GameObject[] requirements = new GameObject[3];

public void ActivateRequirement(int requirement, bool activate){
    requirements[requirement].SetActive(activate);
}

}
