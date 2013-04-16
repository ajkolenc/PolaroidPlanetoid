using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Basically the "Brain" of the creature
 * 
 * 
 */
public class CreatureController : BasicCreature {
	//The space in which the creature can hear or small a thing
	public float senseAuraSize =10.0f;
	
	
	//List of Behaviors/Actions for this creature
	public Dictionary<string, CreatureAction> behaviors;
	
	public EatingHandler eatingHandler;
	
	//How much energy this creature has (out of 1)
	//Energy is used to determine if we need to eat or sleep more
	private float energy=3000;
	//The maxEnergy of the thing, set in start
	public float maxEnergy;
	//Point at which the creature will need to sleep
	public float sleepEnergyPoint = 20;
	
	//Amount it will wander from it's home location if it doesn't have anything to do
	public float wanderAmount = 10;
	//Home location: Where it starts
	public Vector3 homeLocation;
	
	//The Number of things this creature can be aware of at once
	public int numberOfObjectsThisCreatureCanThinkAbout=10;
	//Dictionary of GameObjects to time remaining to remember it values
	public Dictionary<GameObject, float > memories;
	private int numberOfMemories = 0;
	public float howQuicklyDoIForgetStuff = 4.0f;
	
	public float howOftenToCheckAura = 0.5f;
	private float checkAuraTimer = 0;
	
	//Movement Controller for this creature
	public MovementController movementController;
	
	//Sleep Handler
	public SleepHandler sleepHandler;
	
	//FOR RUNNING AWAY
	public float safeDistance = 30.0f;
	
	
	//Public for testing
	public Vector3 goal;
	public GameObject target;
	private CreatureAction currentAction;
	
	//Virtual so it can be overridden in later creatures for specific behaviors
	public virtual void Start() {
		memories = new Dictionary<GameObject,float>();
		behaviors = new Dictionary<string, CreatureAction>();
		homeLocation = transform.position;
		
		maxEnergy = energy;
		behaviors.Add("Bull", new CreatureAction(RUNNINGAWAY, 0.2f, RunningAway));
		behaviors.Add("First Person Controller", new CreatureAction(RUNNINGAWAY, 0.5f, RunningAway));
		behaviors.Add(name, new CreatureAction(STANDING, 0.1f, HangOutWith));
		behaviors.Add("BlueSeed", new CreatureAction(EATING, 0.7f, Eat));
		
	}
	
	// Update is called once per frame
	void Update () {
		//Current state is not equal to sleeping
		if(currentAction ==null || currentAction.actionStrategy!=SLEEPING){
			//If we're purposeless
			if(goal.magnitude==0 && target==null){
				checkTheOutsideWorld(2.0f);
				
				
			}
			//The usual dude moving around stuff
			else if(target==gameObject){
				//If we've got a good amount of energy, just go some place if active type, otherwise, just wait	
				if(energy>sleepEnergyPoint && goal.magnitude==0){
					goal = homeLocation + new Vector3(Random.Range(-wanderAmount, wanderAmount), 0, Random.Range(-wanderAmount, wanderAmount));
				}
				else if(energy<=sleepEnergyPoint && currentAction.actionStrategy!=SLEEPING){
					//GO TO SLEEP
					setUpSleep();
				}
				
				//Move to goal
				//print("MOVING"):
				if(movementController.MoveTowards(goal, 1.0f)){
					goal = Vector3.zero;
				}
				else if(movementController.stuck){
					movementController.stuck=false;
					goal = Vector3.zero;
				}
				else{
					//Drain energy as you move
					energy-=Time.deltaTime;
				}
			}
			//We've got an actual target, let's go for it
			else{
				
				currentAction.act(target);
			}
			
			
			
			
			if(checkAuraTimer<howOftenToCheckAura){
				checkAuraTimer+=Time.deltaTime;
			}
			else{
				checkTheOutsideWorld(1.0f);
				checkAuraTimer=0.0f;
			}
			
			List<GameObject> keys = new List<GameObject>(memories.Keys);
			
			//Go through our memories to determine if we should "forget" anything
			foreach(GameObject memory in keys){
				
				memories[memory]-=Time.deltaTime;
				
				//We don't remember it anymore
				if(memories[memory]<0){
					memories.Remove(memory);
					
					if(memory==target){
						target=null;
						goal= Vector3.zero;
					}
				}
			}
		}
		else{
			Debug.Log("SLEEPING");
			movementController.StopAnimations();
			
			//Sleep Handler
			
			if(sleepHandler.Sleeping()){
				//Fully rested!
				energy=maxEnergy;
				currentAction = new CreatureAction(RUNNINGAWAY, 0);
				target=null;
				goal=Vector3.zero;
				print("I'm done sleeping!");
				
			}
			
			
			if(checkAuraTimer<howOftenToCheckAura){
				checkAuraTimer+=Time.deltaTime;
			}
			else{
				//Check the outside world, but in a way reduced capacity
				checkTheOutsideWorld(0.1f);
				checkAuraTimer=0.0f;
			}
			
			
			
			
			//Just remember the stuff you were remembering anyway before you went to sleep
		}
		
		/**
		//Don't get stuck in other people: 
		GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
		foreach(GameObject creature in creatures){
				if(creature != gameObject && creature.transform.position==transform.position){
					movementController.MoveTowards(homeLocation,0.1f);
				}
		}
		*/
	}
	
	//Running Away (This thing could be located anywhere)
	//Using the second variable as a timer, use the first variable as a boolean
	private void RunningAway( GameObject target){
		
		Vector3 differenceToTarget = target.transform.position-transform.position;
		
		float distance =differenceToTarget.magnitude;
		
		//Reverse, because we're RUNNING AWAY
		differenceToTarget*=-1;
		differenceToTarget.Normalize();
		differenceToTarget*=safeDistance;
		
		float mvmtSpeed = 1.2f;
		
		if(energy<0){
			mvmtSpeed/=Mathf.Abs(energy);
		}
		
		
		if(distance<3 || ( currentAction.genericVariable2>2.0f)){
			
			if(currentAction.genericVariable2>2.0f){
				currentAction.genericVariable=1;
				if(movementController.MoveTowards(differenceToTarget+transform.position, 1.2f)){
					//If we're ever there, if we ever get away from the thing, we're done running away
					target=null;
					currentAction = new CreatureAction(RUNNINGAWAY, 0);
				}
				else{
					energy-=10*Time.deltaTime;
				}
			}
			else{
				currentAction.genericVariable2+=Time.deltaTime;
			}
		}
		else if(currentAction.genericVariable==0){
			//Look at it
			if(movementController.TurnToFace(target.transform.position)){
				movementController.StopAnimations();
				//Only stare for so long before running (0.5f seconds)
				currentAction.genericVariable2+=Time.deltaTime;
				currentState=STANDING;
			}
		}
		else{
			//You got too close once, it's time to run away
			if(movementController.MoveTowards(transform.position+differenceToTarget, 1.2f)){
				//If we're ever there, if we ever get away from the thing, we're done running away
				target=null;
				
				//Reset Creature Action variables
				currentAction.genericVariable=0.0f;
				currentAction.genericVariable2=0.0f;
				
				
				
				currentAction = new CreatureAction(RUNNINGAWAY, 0);
				
				
			}
			else{
				energy-=10*Time.deltaTime;
			}
		}
		
		//If totally exhausted
		if(energy<-100){
			setUpSleep();
		}
		
	}
	
	//Hang Out With
	private void HangOutWith(GameObject buddy){
		Vector3 differenceToTarget = target.transform.position-transform.position;
		float distance =differenceToTarget.magnitude;
		
		
		
		
		
		
		if(distance>3){
			movementController.MoveTowards(buddy.transform.position,0.1f);
		}
		else if(distance>1){
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
	
	//Eating
	private void Eat(GameObject food){
		if(food!=null){
			Vector3 differenceToTarget = target.transform.position-transform.position;
			float distance =differenceToTarget.magnitude;
			
			if(distance<1.0f){
				//Stand still
				movementController.StopAnimations();
				
				if(food.rigidbody!=null){
					food.rigidbody.isKinematic=true;
				}
				
				if(eatingHandler.moveMouthToThing(food.transform.position) && currentAction.genericVariable2==0){
					
					
					
					if(food.rigidbody!=null && !food.rigidbody.isKinematic){
						food.rigidbody.isKinematic=true;
						
					}
					
					if(currentAction.genericVariable<eatingHandler.eatingTime){
						currentAction.genericVariable+=Time.deltaTime;
					}
					else{
						food.renderer.enabled=false;
						currentAction.genericVariable2=1;
					}
				}
				else if(eatingHandler.GoBackToNormal()){
					Destroy(food);
					currentAction.genericVariable=0.0f;
					energy+=500;
					target=null;
					currentAction = null;
					goal=Vector3.zero;
				}
				
			}
			else{
				movementController.MoveTowards(food.transform.position, 1.0f);
			}
		}
		else{
			currentAction.genericVariable=0.0f;
					energy+=500;
					target=null;
					currentAction = null;
					goal=Vector3.zero;
		}
	}
	
	
	//Check our ears/nose whatever so we can see the outside world place
	private void checkTheOutsideWorld( float auraModifier){
		Collider[] colliders = Physics.OverlapSphere(transform.position,auraModifier*(senseAuraSize/2));
			foreach(Collider c in colliders){
				if(c!=null && behaviors.ContainsKey(c.name)){
					if(target==null){
						target=c.gameObject;
						currentAction=behaviors[c.name];
						currentAction.genericVariable=0;
						currentAction.genericVariable2=0;
						currentState = behaviors[c.name].actionStrategy;
					}
					else{
						//If this new action is a more pressing action than the target action
						if(target==gameObject || behaviors[c.name].actionScore>behaviors[target.name].actionScore){
							//Set as new target
							target=c.gameObject;
							currentAction=behaviors[c.name];
							currentAction.genericVariable=0;
							currentAction.genericVariable2=0;
							currentState = behaviors[c.name].actionStrategy;
						}
					}
					
					//If we don't know about this thing, but we know how to deal with it
					if(numberOfMemories<numberOfObjectsThisCreatureCanThinkAbout && !memories.ContainsKey(c.gameObject)){
						memories.Add(c.gameObject, howQuicklyDoIForgetStuff);
					}
					else if(memories.ContainsKey(c.gameObject)){
						//If we know about this thing, reload it into memories
						memories[c.gameObject]=howQuicklyDoIForgetStuff;
					}
					
				
					
				
				}
			}
		
		//If we still don't have a target, check our memories
		if(target==null){
			foreach(GameObject obj in memories.Keys){
				if(target==null){
					target=obj;
					currentAction=behaviors[obj.name];
					currentAction.genericVariable=0;
					currentAction.genericVariable2=0;
					currentState = behaviors[obj.name].actionStrategy;
				}
				else if(currentAction.actionScore<behaviors[obj.name].actionScore){
					target=obj;
					currentAction=behaviors[obj.name];
					currentAction.genericVariable=0;
					currentAction.genericVariable2=0;
					currentState = behaviors[obj.name].actionStrategy;
				}
			}
		}
		
		if(target==null){
			//If we still don't have a target, just pick ourselves till something better comes along
			target=gameObject;
		}
	}
	
	
	
	//Setting up entering into sleeping
	private void setUpSleep(){
		print("Setting up sleeping");
		movementController.StopAnimations();
		currentAction = new CreatureAction(SLEEPING, 0.2f);
		currentState = SLEEPING;
		target=null;
		goal=Vector3.zero;
	}
	
	public override void Pet ()
	{
		base.Pet ();
		//Less likely to run away
		if(behaviors["First Person Controller"].actionScore>0){
			behaviors["First Person Controller"].actionScore-=0.1f;
		}
		else{
			//Change to like the player a lot
			behaviors["First Person Controller"] = new CreatureAction(1.0f,0.3f, HangOutWith);
		}
	}
	
	
}
