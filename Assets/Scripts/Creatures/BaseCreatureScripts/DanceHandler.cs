using UnityEngine;
using System.Collections;

public class DanceHandler : MonoBehaviour {
	public string danceName;
	bool begunDancing;
	public float danceTime = 5.0f;
	private float origDanceTime;
	
	
	void Start(){
		origDanceTime = danceTime;
	}
	
	
	
	//Returns true when done dancing
	public bool Dance(){
		print("Did I ever get into you? DANCING.");
		
		if(!begunDancing){
			print(name+" is playing: "+danceName);
			animation.Play(danceName);
			begunDancing=true;
		}
		else{
			if(danceTime>0.0f){
				
				
				danceTime-=Time.deltaTime;
			}
			else{
				danceTime = origDanceTime;
				begunDancing=false;
				return true;
			}
		}
		return false;	
	}
	
}
