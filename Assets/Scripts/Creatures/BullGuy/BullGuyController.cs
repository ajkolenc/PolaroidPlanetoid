using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BullGuyController : CreatureController {
	public AudioClip charge;
	
	void Start(){
		memories = new Dictionary<GameObject,float>();
		behaviors = new Dictionary<string, CreatureAction>();
		homeLocation = transform.position;
		
		maxEnergy = 10000;
		behaviors.Add("First Person Controller", new CreatureAction(DANCING, 0.4f, ThrowIntoAir));
		behaviors.Add("RabbitCreature", new CreatureAction(DANCING, 0.2f, ThrowIntoAir));
	}
	
	
	
	//Hang Out With
	private void ThrowIntoAir(GameObject buddy){
		Vector3 differenceToTarget = target.transform.position-transform.position;
		float distance =differenceToTarget.magnitude;
		
		
		
		//print("Distance: "+distance);
		
		
		if(distance>2){
			movementController.MoveTowards(buddy.transform.position,0.05f);
		}
		else{
			
			TimedForce hasForce = buddy.GetComponent<TimedForce>();
			
			if(hasForce==null){
				audio.PlayOneShot(charge);
				//Shoot player into air
				
				TimedForce tf = buddy.AddComponent<TimedForce>();
				tf.forceDirection = transform.up+ transform.forward;
				
				tf.forceMagnitude=1;
			}
			
		}
	}
}