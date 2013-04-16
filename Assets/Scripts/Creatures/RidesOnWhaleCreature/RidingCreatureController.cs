using UnityEngine;
using System.Collections;

public class RidingCreatureController : BasicCreature {
	
	public GameObject wing1, wing2;
	//Animation
	public int curAnimState = -1;
	
	public Vector3 homePosition;
	const int SITTING =0;
	const int FLYING =1;
	
	private float timer = 0.0f;
	private float normalTime = 4.0f;
	private float flyTime = 2.0f;
	
	
	void Start(){
		homePosition = transform.localPosition;
	}
	
	void Update(){
		//Reset
		if(curAnimState==-1){
			curAnimState = Random.Range(0,2);
			if(curAnimState==0){
				timer=normalTime;
			}
			else if(curAnimState==1){
				timer = flyTime;
				wing1.animation.Play();
				wing2.animation.Play();
			}
			
		}
		else if(curAnimState==SITTING){
			
			wing1.animation.Stop();
			wing2.animation.Stop();
			if(timer>=0){
				timer-=Time.deltaTime*Random.Range(0.8f,1.0f);
			}
			else{
				curAnimState=-1;
			}
		}
		else if(curAnimState==FLYING){
			if(timer>=0){
				timer-=Time.deltaTime*Random.Range(0.8f,1.0f);
				
				//Fly Higher!
				transform.localPosition += new Vector3(0, Time.deltaTime*Random.Range(3,5), 0);
			}
			else{
				if((transform.localPosition-homePosition).magnitude>0.1f){
					transform.localPosition = Vector3.Lerp(transform.localPosition, homePosition, Time.deltaTime*Random.Range(3,5));
				}
				else{
					curAnimState=-1;
				}
			}
			
		}
	}
}
