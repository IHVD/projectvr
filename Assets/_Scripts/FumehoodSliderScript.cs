using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FumehoodSliderScript : MonoBehaviour {
    public bool SliderUp = true;

    private void OnMouseOver()
    {
        //when clicking on the slider, the slider will go either up or down depending on the bool. The Mouse button line is for editor testing without having to build
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && SliderUp == true || Input.GetMouseButtonDown(0) && SliderUp == true)
        {
            transform.position += new Vector3(0, -1.5f, 0);
            SliderUp = false;
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && SliderUp == false || Input.GetMouseButtonDown(0) && SliderUp == false)
        {
            transform.position += new Vector3(0, 1.5f, 0);
            SliderUp = true;
        }
    }
}
