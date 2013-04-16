using UnityEngine;
using System.Collections;

//For sleeping
public class SleepHandler : MonoBehaviour {
	//At least these three animations are necessary to 
	public string goingToSleep, sleeping, wakingUp;
	//The amount of time to sleep for
	public float sleepTime=20.0f;
	
	public bool wakingUpNow;
	
	
	//Returns True when done sleeping
	public bool Sleeping(){
		
		if(!animation.IsPlaying(goingToSleep) && !animation.IsPlaying(sleeping) && !animation.IsPlaying(wakingUp)){
			animation.Play(goingToSleep);
		
			animation.CrossFadeQueued(sleeping,0.2f, QueueMode.CompleteOthers);
		}
		else if(animation.IsPlaying(sleeping) && sleepTime>0){
			sleepTime-=Time.deltaTime;
		}
		else if(sleepTime<=0 && !animation.IsPlaying(wakingUp) && !wakingUpNow){
			wakingUpNow=true;
			animation.Play(wakingUp);
		}
		else if(wakingUpNow && !animation.IsPlaying(wakingUp)){
			animation.Stop();
			return true;
		}
		
			return false;
		
		
		
	}
	
}
