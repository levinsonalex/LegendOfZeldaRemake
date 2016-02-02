using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FlashLight : MonoBehaviour {

	public Image redLight;
	public bool flashing = false;
	bool	red = false;
	float startTime;
	public float durTime = .5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (flashing) {
			if(Time.time - startTime > durTime){
				startTime = Time.time;
			flicker ();
			}
		}
	
	}

	public void flash(){
		flashing = true;
	}

	void flicker(){
		if (red) {
			red = false;
			redLight.color = new Color32(63,69,255,255);
		} else {
			red = true;
			redLight.color = Color.red;
		}
	}
}
