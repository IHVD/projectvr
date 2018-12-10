using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FumehoodSliderScript : MonoBehaviour {
    public bool SliderUp = true;
    public Transform pointA, pointB;
    float slide; 
    private void OnMouseOver()
    {
        slide = 10*Time.deltaTime;

        //when clicking on the slider, the slider will go either up or down depending on the bool. The Mouse button line is for editor testing without having to build
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && SliderUp == true || Input.GetMouseButtonDown(0) && SliderUp == true)
        {
            transform.position = Vector3.MoveTowards(pointA.position, pointB.position, slide);
            SliderUp = false;
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && SliderUp == false || Input.GetMouseButtonDown(0) && SliderUp == false)
        {
            transform.position = Vector3.MoveTowards(pointB.position, pointA.position, slide);
            SliderUp = true;
        }
    }
}
