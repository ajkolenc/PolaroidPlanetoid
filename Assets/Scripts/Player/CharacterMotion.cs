using UnityEngine;
using System.Collections;

//Character motion for player, don't touch
public class CharacterMotion : MonoBehaviour {
	public GameObject directionIndicator;
	public float speed=10f, jumpTimer=0.0f;
	public float gravitySpeed=0.0f, jumpSpeed=5.0f, transferHeight;
	public enum MovementState{NORMAL, CROUCHING, SPRINTING, JUMPING, CAMERAING};
	public bool jumping=false, transferring, airborne;
	public MovementState currMovementState, prevMovementState;
	private Vector3 prevPos;
	//For storing the object below you
	public GameObject belowObj;
	private Vector3 prevPosBelow;
	//Bounce counter
	public float bounciness=5.0f;
	
	public bool shouldMove=true;
	
	//Gravity
	private bool doGravity;
	
	int groundedCheck=0;
	
	public string[] tagsNotToGoTo = {"Water"};
	
	
	// Update is called once per frame
	void Update () {
		prevPos=transform.position;	
	
		
		if(belowObj!=null){
			if(belowObj.transform.position!=prevPosBelow){
				transform.position += belowObj.transform.position-prevPosBelow;
			}
			
			prevPosBelow = belowObj.transform.position;
		}
		
		//Can't switch in camera mode
		if(currMovementState!=MovementState.CAMERAING){
			ModeSwitch();
		}
		
		//Transfer from normal height to crouching and back
		if(transferring){
			if(Mathf.Abs(transferHeight-transform.position.y)>0.1f){
				transform.position=Vector3.Lerp(transform.position,new Vector3(transform.position.x, transferHeight,transform.position.z),Time.deltaTime*speed);
				
				
				
			}
			else{
				transferring=false;
			}
		}
		//print(" Bef Grav Position Y: "+transform.position.y);
		
		doGravity=false;
		CheckGrounded();
		//print("After Grav Position Y: "+transform.position.y);
		
		MovementHandler();
		
		
		Vector3 verticalMvmt = new Vector3();
		if(jumping){
			verticalMvmt+=JumpHandler();
		}
		if(doGravity){
			verticalMvmt-=Gravity();
		}
		
		Vector3 verticalPos = transform.position+verticalMvmt;
		
		
		//Need to put a check in here to have some point at which we'll switch to going the other direction
		//if((transform.position-verticalPos).magnitude>0.1f){
		transform.position=verticalPos;
		//}
		
		
		//print("After Jump Position Y: "+transform.position.y);
		if(bounciness<5 && bounciness>0){
			bounciness+=Time.deltaTime*2;
		}
		else if(bounciness<0){
			bounciness=0.0f;
		}
	}
	
	
	private void ModeSwitch(){
		//JUMPING
		//JUMPING
		if(Input.GetAxis("Jump")>0 && jumpTimer<=0.0f && !jumping ){
			
			//print("Getting here");
			//prevMovementState=currMovementState;
			//currMovementState=MovementState.JUMPING;
			jumpTimer=0.2f;
			jumping=true;
			
			prevMovementState=currMovementState;
			currMovementState=MovementState.JUMPING;
		}
		
		if(jumpTimer>0){
			jumpTimer-=Time.deltaTime;
		}
		
		//Sprinting
		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) && currMovementState==MovementState.NORMAL){
			if(currMovementState!=MovementState.SPRINTING){
				prevMovementState=currMovementState;
			}
			currMovementState=MovementState.SPRINTING;
			
		}
		else{
			if(currMovementState==MovementState.SPRINTING){
				currMovementState=prevMovementState;
			}
			
		}
		
		//Crouching
		if(Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftControl)){
			float crouchAmount = 1.0f;
			
			if(currMovementState==MovementState.NORMAL){
				prevMovementState=currMovementState;
				currMovementState=MovementState.CROUCHING;
				transferring=true;
				
				//lower height slightly
				transferHeight = transform.position.y;
				transferHeight-=crouchAmount;
				
				
				//Change the size so you don't slip through the ground
				
			}
			else if(currMovementState==MovementState.CROUCHING){
				prevMovementState=currMovementState;
				currMovementState=MovementState.NORMAL;
				transferring=true;
				
				//raise height slightly
				transferHeight = transform.position.y;
				transferHeight+=crouchAmount;
				
			}
		}
	}
	
	//For switching to camera from CameraControl
	public void Cameraing(){
		currMovementState=MovementState.CAMERAING;
	}
	
	//For switching out of camera from CameraControl
	public void Walking(){
		currMovementState=MovementState.NORMAL;
		
	}
	
	public void MovementHandler(){
		if(shouldMove){
			if(currMovementState==MovementState.NORMAL){
				moveYourself(1.0f);
			}
			else if(currMovementState==MovementState.JUMPING){
				moveYourself(0.75f);
			}
			else if(currMovementState==MovementState.CROUCHING){
				moveYourself(0.1f);
			}
			else if(currMovementState==MovementState.CAMERAING){
				moveYourself(0.3f);
			}
			else if(currMovementState==MovementState.SPRINTING){
				moveYourself(1.75f);
			}
		}
	}
	
	//Handles jumping and gravity while in the air
	public Vector3 JumpHandler(){
		return new Vector3(0,jumpSpeed*Time.deltaTime,0);
		
	}
	
	//Returns Gravity
	public Vector3 Gravity(){
		if(gravitySpeed<1.0f){
			gravitySpeed+=Time.deltaTime/3;
		}
		//print("Gravity Speed: "+gravitySpeed);
		
		return new Vector3(0,gravitySpeed,0);
		
	}
	
	//Moves the player an amount based on the passed in multiple
	private void moveYourself(float multiplier){
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		
		
			//Square magnitude is cheaper for checking if Input was pressed at all
			if(movement.sqrMagnitude!=0){
				Vector3 currFacing = new Vector3(directionIndicator.transform.position.x,transform.position.y,directionIndicator.transform.position.z);
			
				Vector3 differenceVector = currFacing-transform.position;
				
				//The change along the local z axis
				Vector3 newPos = transform.position;
				newPos += differenceVector*movement.z*Time.deltaTime*(speed)*multiplier;
				
				//The change along the local x axis
				Vector3 differenceVectorHorz = new Vector3(differenceVector.z,0,-1*differenceVector.x);
				newPos += differenceVectorHorz*movement.x*Time.deltaTime*(speed)*multiplier/2;
			
				//Use Raycast to check for stuff around player
				RaycastHit hit;
			
			
				//MATTHEW. FIX THIS
				if(Physics.Raycast(transform.position,newPos-transform.position,out hit, 10*((newPos-transform.position).magnitude))){
					//You can't go there! There's a thing there!
					if(hit.collider.tag!="NoCollision"){
						//print("Hit a: "+hit.collider.name);
					}
					else{
						RaycastHit hit2;
						//Let's see if water is below you
						if(Physics.Raycast(newPos,transform.up*-1,out hit2, transform.localScale.y*2)){
							bool canGoThere = true;	
							
							for(int i =0; i<tagsNotToGoTo.Length; i++){
								if(tagsNotToGoTo[i]==hit2.collider.tag){
									canGoThere=false;
								}
							}
						
							if(canGoThere){
								transform.position=newPos;
							}
						}
						else{
							transform.position=newPos;
						}
					
						
					}
				}
				else{
					RaycastHit hit2;
						//Let's see if water is below you
						if(Physics.Raycast(newPos,transform.up*-1,out hit2, 10)){
							bool canGoThere = true;	
						
							for(int i =0; i<tagsNotToGoTo.Length; i++){
								if(tagsNotToGoTo[i]==hit2.collider.tag){
									canGoThere=false;
								}
							}
						
							if(canGoThere){
								transform.position=newPos;
							}
						}
						else{
							transform.position=newPos;
						}
				}
			}
	}
	
	void CheckGrounded(){
		
		float distance = transform.localScale.y/2;
		if(currMovementState == MovementState.CROUCHING){
			distance=0.1f;
			
		}
		
		RaycastHit hit;
			if(Physics.Raycast(transform.position,Vector3.down,out hit,distance)
			|| Physics.Raycast(transform.position,Vector3.down,out hit,distance*0.5f)
			|| Physics.Raycast(transform.position,Vector3.down,out hit,distance*1.5f)){
			
				//print("Name of hit: "+hit.collider.name);
				//GROUNDED
				
					
					groundedCheck=0;
					gravitySpeed=0.0f;
			
					if(jumpTimer<0.1){
						jumping=false;
						jumpTimer=0.0f;
				
						if(currMovementState==MovementState.JUMPING){
						currMovementState=MovementState.NORMAL;
						}
				
						if(hit.distance<distance){
							transform.position+=new Vector3(0,Time.deltaTime*20*(distance-hit.distance),0);
						}
					}
			
			
					if(hit.collider.gameObject!=belowObj){
						belowObj = hit.collider.gameObject;
						prevPosBelow=belowObj.transform.position;
					}
					
					
				
					
				//}
			}
			else{
				//Nothing below you, drop it
				if(Physics.Raycast(transform.position,Vector3.down,out hit,distance*2)){
					if(hit.collider.gameObject!=belowObj){
						belowObj=null;
					}
				}
				
				if(groundedCheck>3){
					doGravity=true;
				//Gravity();
				}
				else{
					groundedCheck++;
				}
			}
		
			
		
	}
	
	
	void OnTriggerEnter(Collider other){
		if(other.transform.position.y<transform.position.y){
			jumping=false;
			airborne=false;
			gravitySpeed=0.0f;
			
			if(currMovementState==MovementState.JUMPING){
				currMovementState=prevMovementState;
				prevMovementState=MovementState.JUMPING;
				
			}
			
			//prevMovementState=currMovementState;
			//currMovementState=MovementState.NORMAL;
		}
		else if(other.tag!="NoCollision"){
			//MAKE NOISE
			
			
			if(other.tag!="Projectile" && other.transform.eulerAngles.x==0){
				
				//Push back
				Vector3 pushBack = transform.position;
				
				pushBack-=5*(pushBack-prevPos);
				pushBack.y=transform.position.y;
				transform.position=pushBack;
				
				//bounciness-=2;
			}
		}
	}
	
	/**
	void OnTriggerExit(Collider other){
		if(other.transform.position.y<transform.position.y){
			if(!jumping){
				airborne=true;
			}
		}
	}
	*/
	
	
}
