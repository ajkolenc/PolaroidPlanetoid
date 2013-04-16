using UnityEngine;
using System.Collections;

public class TimedForce : MonoBehaviour {
	
	//Time limit is lifespan of script, currTime is current Time (0.2)
	public float timeLimit=0.5f, currTime;
	//Direction of force
	public Vector3 forceDirection;
	//Magnitude of force
	public float forceMagnitude;
	
	
	void Update () {
		//Apply force while below time limit, past then destory this script
		if(currTime<timeLimit){
			ApplyForce(gameObject);
			
			
			currTime+=Time.deltaTime;
		}
		else{
			Destroy(this);
		}
	}
	
	//ApplyForce to Object 
	void ApplyForce( GameObject applyForceTo){
		//print("Applying force to: "+applyForceTo);
		if(applyForceTo!=null){
			Vector3 pushedBackPos = applyForceTo.transform.position;
			//Trying Adding RayCastHit
			RaycastHit hit;
			pushedBackPos+=forceDirection*(forceMagnitude);
			float distance =(applyForceTo.transform.position-pushedBackPos).magnitude;
			
			applyForceTo.transform.position=pushedBackPos;	
			/**
			if( distance!=0 && Physics.Raycast(applyForceTo.transform.position,pushedBackPos, out hit, distance)){
					if(hit.collider.tag=="NoCollision"){
						applyForceTo.transform.position=pushedBackPos;	
					}
					else{
						//Reflect force
						forceDirection *=-1;
						Vector3 diff = hit.normal-forceDirection;
						forceDirection +=diff*2;
					}
			}
			else{
				applyForceTo.transform.position=pushedBackPos;	
			}
			*/
			
		}
		
	}
}
