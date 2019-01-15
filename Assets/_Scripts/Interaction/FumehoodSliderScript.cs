using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FumehoodSliderScript : MonoBehaviour {
    public bool SliderUp = true;
    public Transform pointA, pointB;
    float slide; 

    public void GlassSlide(){
        //based on current 'position', move down (or up).
        slide = 10*Time.deltaTime;

        if(SliderUp == true){
            transform.position = Vector3.MoveTowards(pointA.position, pointB.position, slide);
            SliderUp = false;
        }

        else if(SliderUp == false){
            transform.position = Vector3.MoveTowards(pointB.position, pointA.position, slide);
            SliderUp = true;
        }
    }
    
}
