using UnityEngine;
using System.Collections;


//MovementController controls the creature's overall movement
//Needs to be attached to the highest level of the creature
public class WhaleMovementController : MonoBehaviour {
	//The height of the creature
	public float ourHeight=1.4f;
	
	
	//The legs (limbs that should animate while the creature is walking/moving
	public Leg[] legs;
	//The feet of the creature, or the parts that should be in touch with the ground at all times
	//TODO: WORRY ABOUT THIS?
	//public GameObject[] feets;
	//Speed the creature should move at when walking, speed for when running
	public float walkSpeed=5.0f, runSpeed=10.0f;
	//If true, we're walking, otherwise we are assumed to be running
	private bool isWalking;
	
	
	//Whether or not we're currently falling
	private bool falling;
	//Current gravity speed
	private float gravitySpeed;
	//Max gravity speed
	private float maxGravitySpeed = 9.0f;
	private float gravityAcceleration = 9.0f;
	
	public bool stuck = true;
	
	//Sets the isWalking variable to the passed in bool
	public void setWalking(bool walking){
		isWalking=walking;
	}
	
	//Getter for the isWalking variable
	public bool getWalking(){
		return isWalking;
	}
	
	
	//Returns true if facing
	public bool TurnToFace(Vector3 goal){
		//print("Turning to face");
		goal.y=transform.position.y;
		Vector3 difference = goal-transform.position;
		difference.Normalize();
		
		if((difference-transform.forward).magnitude>1.0f){
			float angleBetween = Vector3.Angle(difference,transform.forward);
			
			
			Vector3 levelGoal = new Vector3(goal.x,transform.position.y,goal.z);
			
			if(!(levelGoal-transform.position).Equals(Vector3.zero)){
				//print("Should be rotating");
				Quaternion targetRotation = (Quaternion.LookRotation((levelGoal-transform.position)));
				
			
				transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,Time.deltaTime*0.01f);
				
				//MoveYourLegs();
			}
			else{
				return true;
			}
			return false;
			
		}
		else{
			return true;
		}
	}
	
	
	/**
	 * Handles walking and running towards target, virtual so it can be extended 
	 * in further classes to handle other types of movement
	 * 
	 * 
	 * Goal: the place we want to get to
	 * Distance: The excepted distance to the goal this method will allow such that it will stop moving
	 * 
	 * Return: Whether or not we reached our goal
	*/
	public virtual bool MoveTowards(Vector3 goal, float distance){
		//print("Moving Towards");
		bool facingGoal = TurnToFace(goal);
		
		if(facingGoal){
			//print("Moving Towards");
			//don't want to be moving in the y direction
			goal.y=transform.position.y;
			//Difference between our position and the goal
			Vector3 difference = goal-transform.position;
			
			
			//print("Difference Magnitude: "+difference.magnitude);
			//Move Towards Goal
			if(difference.magnitude>distance){
				difference.Normalize();
				
				float speed = 0.0f;
				if(isWalking){
					speed=walkSpeed;
				}
				//Not walking? We MUST BE RUNNING
				else{
					speed=runSpeed;
				}
				
				RaycastHit hit;
				if(Physics.Raycast(transform.position+transform.forward, difference, out hit, difference.magnitude*speed*Time.deltaTime)){
					//Don't go there
					//Stuck is the way to pass messages up to the place that's calling us
					stuck=true;
				}
				else{
					
					Vector3 newPosition = transform.position+(difference*speed*Time.deltaTime);
					
					if(Physics.Raycast(newPosition, -1*transform.up, out hit, 46.0f)){
						//Only move there if there's water there
					
						//print("Hit collider: "+hit.collider.name);
						if(hit.collider.tag=="Water"){
							transform.position+=Time.deltaTime*difference*speed;
						}
						else{
							stuck=true;
						}
					}
					
					//transform.position+=Time.deltaTime*difference*speed;
					//Move your legs, we got a place to go to!
					//WARNING: DON'T MOVE YOUR LEGS
					//MoveYourLegs();
				}
			}
			else{
				//Stop moving, we're there
				StopAnimations();
				return true;
			}
			
			//GroundedCheck();
			//StopAnimations();
			return false;
		}
		else{
			return facingGoal;
		}
	}
	
	//Moving the Legs (Animation-Wise) [Virtual so we can override if we have more animations]
	virtual public void MoveYourLegs(){
		//If we're walking
		if(isWalking){
			foreach(Leg leg in legs){
				//if the leg is not yet playing the walk animation, set it to do so
				if(!leg.isWalking()){
					leg.setWalking();
				}
				
			}
		}
		//ASSUMING WE ARE RUNNING
		else{
			foreach(Leg leg in legs){
				//if the leg is not yet playing the run animation, set it to do so
				if(!leg.isRunning()){
					leg.setRunning();
				}
				
			}
		}
	}
	
	//Hard stops all Animations
	virtual public void StopAnimations(){
		//print("I got here, I guess");
		foreach(Leg leg in legs){
			//print("I got a non-null leg");
			leg.StopAnimation();
		}
	}
	
	
	//Checks if we're grounded, if we're too high up, make Gravity happen
	public void GroundedCheck(){
		//Make sure not to sink into the ground
		RaycastHit hit;
		
		if(Physics.Raycast(transform.position, -1*transform.up, out hit, ourHeight/2)){
			//Keep us from falling over EVER
			if(hit.collider.tag!="Creature"){
				transform.rotation = Quaternion.LookRotation(transform.forward, hit.normal);
				falling=false;
				if(hit.distance<(ourHeight/2)-0.1){
					transform.position+=((ourHeight/2)-0.1f-hit.distance)*transform.up;
				}
			}
			else{
				if(!falling){
				falling=true;
				gravitySpeed=0.0f;
				}
				
				Gravity();
			}
		}
		else{
			
			//There's nothing beneath us, let's GRAVITY
			//Did you just start falling? if so, let's start up gravity
			if(!falling){
				falling=true;
				gravitySpeed=0.0f;
			}
			
			Gravity();
		}
	}
	
	
	//GRAVITTYYYYYYY
	public void Gravity(){
		//If we're not at our terminal velocity, increment
		if(gravitySpeed<maxGravitySpeed){
			gravitySpeed+=Time.deltaTime*gravityAcceleration;
		}
		
		//Fall
		transform.position-= new Vector3(0,gravitySpeed*Time.deltaTime,0);
		
	}
}
