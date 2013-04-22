using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendlyCreatureController : CreatureController {

	void Start(){
		memories = new Dictionary<GameObject,float>();
		behaviors = new Dictionary<string, CreatureAction>();
		homeLocation = transform.position;
		
		energy=maxEnergy;
		behaviors.Add("Rabbit", new CreatureAction(STANDING, 0.1f, FriendlyHangOutWith));
	}
	
	//Hang Out With
	private void FriendlyHangOutWith(GameObject buddy){
		Vector3 differenceToTarget = target.transform.position-transform.position;
		float distance =differenceToTarget.magnitude;
		
		
		
		
		
		
		if(distance>1){
			movementController.MoveTowards(buddy.transform.position,0.1f);
		}
		else if(distance>0.5){
			//Just hang out
			movementController.StopAnimations();
			movementController.TurnToFace(buddy.transform.position);
			//print("HANGING");
			
			//Happy when hanging
			happiness +=10*Time.deltaTime;
			
			
			
		}
		else{
			differenceToTarget*=-1;
			
			if(differenceToTarget.magnitude>0.1){
				movementController.MoveTowards(differenceToTarget+transform.position,0.1f);
			}
			else{
				movementController.MoveTowards(homeLocation,0.1f);
			}
		}
	}
}
