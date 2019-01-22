using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundaryPlaneScript : MonoBehaviour {
	

	private bookScript theBookScript;
	// Use this for initialization
	void Start () {
		theBookScript = FindObjectOfType<bookScript>();
	}
	void Update () {
		theBookScript = FindObjectOfType<bookScript>();
	}	

	private void OnTriggerEnter(Collider other) {
		if(other.tag == "Book") {
			theBookScript.theActualBook.transform.position = theBookScript.bookOriginalPosition;
			theBookScript.theActualBook.transform.rotation = theBookScript.bookOriginalRotation;
		}
		print("Out of bounds!");
	}
}
