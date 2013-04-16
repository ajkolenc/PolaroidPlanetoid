using UnityEngine;
using System.Collections;

//Information Holder to be used in CreatureController that helps determine we should act based on the name of a creature
public class CreatureAction{
	//Strategy for this action
	public float actionStrategy;
	//The score of a given action
	public float actionScore;
	//Distance to do the action in
	public float distance=1.0f;
	//The action the creature will take in terms of this object
	public CreatureAct act;
	//Variable used by the CreatureAct act, potentially
	public float genericVariable, genericVariable2;
	
	public CreatureAction(float strategy, float score){
		actionStrategy=strategy;
		actionScore = score;
	}
	
	public CreatureAction(float strategy, float score, float _distance){
		actionStrategy=strategy;
		actionScore = score;
		distance=_distance;
	}
	
	public CreatureAction(float strategy, float score, CreatureAct _act){
		actionStrategy=strategy;
		actionScore = score;
		act=_act;
	}
	
	
	public delegate void CreatureAct(GameObject other);
}
