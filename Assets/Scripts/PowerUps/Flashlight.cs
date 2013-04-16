using UnityEngine;
using System.Collections;

//ATTACH TO MAIN 'eyes' CAMERA
public class Flashlight : Powerup {
	public Light myLight;
	
	//FOR TESTING ONLY
	void Start(){
		Setup();
	}
	
	//Set up the Flash light
	public override void Setup(){
		myLight = gameObject.transform.FindChild("Camera-Camera").gameObject.AddComponent<Light>();
		myLight.type = LightType.Spot;
		
		myLight.intensity = 0;
		
		myLight.spotAngle = 80;
		myLight.range = 50;
	}
	
	//Turning off, turning on flashlight
	void Update(){
		if(Input.GetKeyDown(KeyCode.V)){
			if(myLight.intensity ==0){
				myLight.intensity = 5;
			}
			else{
				myLight.intensity = 0;
			}
		}
	}
	
	
}
