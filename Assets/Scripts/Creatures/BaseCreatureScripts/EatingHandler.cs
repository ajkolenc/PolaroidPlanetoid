using UnityEngine;
using System.Collections;

public class EatingHandler : MonoBehaviour {
	public GameObject mouth;
	private Quaternion originalRotation;
	public float eatingTime = 1.0f;
	
	public string eatAnim;
	
	public virtual bool moveMouthToThing(Vector3 goal){
		if(Mathf.Abs(goal.y-transform.position.y)>0.5f){
			if(goal.y<transform.position.y){
				//Rotate down for food
				transform.Rotate( new Vector3(Time.deltaTime,0,0));
			}
			else{
				//Rotate up to food
				transform.Rotate( new Vector3(-1*Time.deltaTime,0,0));
			}
		}
		else{
			//animation.Play(eatAnim);
			Vector3 difference = goal-transform.position;
			
			if(difference.magnitude<1.0f){
				originalRotation=transform.rotation;
				return true;
			}
			else{
				Quaternion targetRotation = (Quaternion.LookRotation((goal-transform.position)));
				transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,Time.deltaTime*5.0f);
			}
		}
		
		return false;
	}
	
	public virtual bool GoBackToNormal(){
		float angle = Quaternion.Angle(transform.rotation, originalRotation);
		
		if(angle>10){
			transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime*5.0f);
		}
		else{
			return true;
		}
		
		return false;
	}
	
	
	
	
	
}
