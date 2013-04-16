using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	bool walkingMode=true;
	public Camera main, other;
	public CharacterMotion motion;
	public GUITexture camMark;
	
	
	void ActivateCamera(){
		//Switch to camera mode
				main.enabled=false;
				other.enabled=true;
				
				main.GetComponent<FirstPersonPerspective>().enabled=false;
				
				main.GetComponent<ObjectHolder>().ReleaseObj();
				main.GetComponent<ObjectHolder>().enabled=false;
				other.GetComponent<FirstPersonPerspective>().enabled=true;
				other.GetComponent<PictureTaking>().enabled=true;
				
				other.transform.rotation=main.transform.rotation;
				
				//motion.currMovementState =motion.Movement
				
				//camMark.enabled=true;
	}
	
	void ActivateNormal(){
		//Switch to normal mode
				main.enabled=true;
				other.enabled=false;
				
				
				main.transform.rotation=other.transform.rotation;
				main.GetComponent<ObjectHolder>().enabled=true;
				main.GetComponent<FirstPersonPerspective>().enabled=true;
				other.GetComponent<FirstPersonPerspective>().enabled=false;
				other.GetComponent<PictureTaking>().enabled=false;
				
				//motion.enabled=true;
		
				//main.transform.rotation=other.transform.rotation;
				//other.transform.eulerAngles = Vector3.zero;
				other.transform.rotation=main.transform.rotation;	
		
		
				//camMark.enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mainRot = main.transform.eulerAngles;
		mainRot.z=0;
		main.transform.eulerAngles=mainRot;
		
		Vector3 otherRot = other.transform.eulerAngles;
		otherRot.z=0;
		other.transform.eulerAngles=otherRot;
		
		if(Input.GetKeyDown(KeyCode.Q)){
			if(walkingMode){
				ActivateCamera();
			}
			else{
				ActivateNormal();
			}
			
			
			
			
			walkingMode=!walkingMode;
		}
		
		
	}
}
