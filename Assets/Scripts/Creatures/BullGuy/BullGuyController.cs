using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BullGuyController : CreatureController {
	public AudioClip charge;
	
	
	
	void Start(){
		memories = new Dictionary<GameObject,float>();
		behaviors = new Dictionary<string, CreatureAction>();
		homeLocation = transform.position;
		
		energy=maxEnergy;
		
		behaviors.Add("First Person Controller", new CreatureAction(DANCING, 0.4f, ThrowIntoAir));
		behaviors.Add("RabbitCreature", new CreatureAction(DANCING, 0.2f, ThrowIntoAir));
	}
	
	//Hang Out With
	private void ThrowIntoAir(GameObject buddy){
		Vector3 differenceToTarget = target.transform.position-transform.position;
		float distance =differenceToTarget.magnitude;
		
		energy-=Time.deltaTime*3;
		
		
		//print("Distance: "+distance);
		
		
		if(distance>2){
			movementController.MoveTowards(buddy.transform.position,0.05f);
		}
		else{
			
			TimedForce hasForce = buddy.GetComponent<TimedForce>();
			
			if(hasForce==null){
				energy-=10;
				//You're so happy!
				happiness+=50;
				print("Happiness: "+happiness);
				
				audio.PlayOneShot(charge);
				//Shoot player into air
				
				TimedForce tf = buddy.AddComponent<TimedForce>();
				tf.forceDirection = transform.up+ transform.forward;
				
				tf.forceMagnitude=1;
			}
			
		}
	}
}