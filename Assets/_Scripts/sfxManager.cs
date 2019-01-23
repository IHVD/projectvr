using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxManager : MonoBehaviour {

	//Select pitch and volume
    public float lowPitchRange = 0.9f;
    public float highPitchRange = 1.1f;	
    public float volumeValue;

	//Player SFX
	public AudioSource clickSFX;
	public AudioSource impact1SFX;
	public AudioSource impact2SFX;
	public AudioSource impact3SFX;
	public AudioSource impact4SFX;
	public AudioSource hit1SFX;
	public AudioSource hit2SFX;
	public AudioSource hit3SFX;
	public AudioSource pickup1SFX;
	public AudioSource pickup2SFX;


	//Play sound function. Only has one sound!
	public void PlaySound(AudioSource sound) {
		sound.pitch = Random.Range(lowPitchRange, highPitchRange); 
		sound.Play(0);
	}

}
