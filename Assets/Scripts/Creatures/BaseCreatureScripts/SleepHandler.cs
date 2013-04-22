using UnityEngine;
using System.Collections;

//For sleeping
public class SleepHandler : MonoBehaviour {
	//At least these three animations are necessary to 
	public string goingToSleep, sleeping, wakingUp;
	
	public AnimationClip goingToSleepA, sleepingA, wakingUpA;
	//The amount of time to sleep for
	public float sleepTime=20.0f;
	public float waitTime=1.0f;
	private float origWaitTime, origSleepTime;
	public bool wakingUpNow;
	int stage;
	
	public GameObject z;
	private GameObject ourZ;
	private bool zSpanwed;
	
	void Start(){
		origWaitTime = waitTime;
		origSleepTime = sleepTime;
	}
	
	//Returns True when done sleeping
	public bool Sleeping(){
		
		if(waitTime>0.0f){
			
			if(z!=null){
				if(!zSpanwed){
					ourZ = Instantiate(z, transform.position+Vector3.up, transform.rotation) as GameObject;
					zSpanwed=true;
				}
			}
			
			animation.Stop();
			waitTime-=Time.deltaTime;
		}
		else{
			
			//print("Animation clip: "+animation.clip.name);
			if(!animation.IsPlaying(goingToSleep) && !animation.IsPlaying(sleeping) && !animation.IsPlaying(wakingUp)){
				animation.clip = goingToSleepA;
				animation.Play();
			
			
				
				animation.CrossFadeQueued(sleeping,0.2f, QueueMode.CompleteOthers);
			}
			else if(animation.IsPlaying(sleeping) && sleepTime>0){
			//	print("Did I ever get here?");
				sleepTime-=Time.deltaTime*Random.Range(0.9f,1.0f);
			}
			else if(sleepTime<=0 && !animation.IsPlaying(wakingUp) && !wakingUpNow){
				wakingUpNow=true;
				animation.clip=wakingUpA;
				animation.Play();
			}
			else if(wakingUpNow && !animation.IsPlaying(wakingUp)){
				animation.Stop();
				
				waitTime = origWaitTime;
				wakingUpNow=false;
				sleepTime = origSleepTime;
				zSpanwed=false;
				Destroy(ourZ);
				return true;
			}
			
		}
			return false;
		
		
		
	}
	
}
