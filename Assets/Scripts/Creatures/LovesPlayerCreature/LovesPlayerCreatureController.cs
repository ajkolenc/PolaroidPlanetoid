using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LovesPlayerCreatureController : CreatureController {
	void Start(){
		memories = new Dictionary<GameObject,float>();
		behaviors = new Dictionary<string, CreatureAction>();
		homeLocation = transform.position;
		
		maxEnergy = 10000;
		behaviors.Add("First Person Controller", new CreatureAction(STANDING, 0.4f, FriendlyHangOutWith));
	}
	
	//Hang Out With
	private void FriendlyHangOutWith(GameObject buddy){
		Vector3 differenceToTarget = target.transform.position-transform.position;
		float distance =differenceToTarget.magnitude;
		
		
		
		
		
		
		if(distance>0.1){
			movementController.MoveTowards(buddy.transform.position,0.05f);
		}
		else if(distance>0.05f){
			//Just hang out
			movementController.StopAnimations();
			movementController.TurnToFace(buddy.transform.position);
			//print("HANGING");
			
			
			
			
			
		}
		else{
			differenceToTarget*=-1;
			
			if(differenceToTarget.magnitude>0.5f){
				movementController.MoveTowards(differenceToTarget+transform.position,0.1f);
			}
			else{
				movementController.MoveTowards(homeLocation,0.1f);
			}
		}
	}
}
