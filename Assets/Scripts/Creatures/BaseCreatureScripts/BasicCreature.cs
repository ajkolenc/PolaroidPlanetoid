using UnityEngine;
using System.Collections;



//Basic Creature, this class is used as an information holder for certain information that must be held by all creatures
public class BasicCreature : MonoBehaviour {
	//This is the current state for this creature
	public float currentState=1;
	//The various states this creature can be in
	public const float RUNNINGAWAY = 0, STANDING=20, SLEEPING=25f, MOVING=50, ATTACKING=70, EATING=100, DANCING=150;
	//How rare this creature is (how many points a picture of it is worth
	public float rareness=1.0f;
	
	//How happy this creature is (out of 1)
	//Happiness is used to determine if this creature will dance or not
	private float happiness=50;
	
	
	/**
	 * determines how many points the state of this creature and it's rareness should give you 
	 * (This will be factored with how close to the center of the frame the picture of the creature was
	 * and how close you were to front facing with the creature)
	 * 
	 * Virtual so it can be overriden in later classes if need be 
	*/
	public virtual int getPoints(){
		return (int)(rareness*currentState);
	}
	
	//For petting the creature
	public virtual void Pet(){
		happiness+=20;
		
	}
	
}
