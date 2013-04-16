using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {
	
	bool afraid;
	float afraidTimer;
	float timer;
	bool forth;
	
	void Start(){
		int start= Random.Range(0,2);
		
		if(start==0){
			forth=true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		float amnt = Time.deltaTime*10.0f * Random.Range(0.7f,0.9f);
		
		if(forth){
			amnt*=-1;
		}
		
		
		transform.Rotate(new Vector3(0,0,amnt));
		
		timer+=Time.deltaTime*Random.Range(0.8f,1.2f);
		
		if(timer>1.5f){
			timer=0;
			forth=!forth;
		}
		
		
		if(afraid){
			
			
			
			afraidTimer-=Time.deltaTime;
			
			if(afraidTimer<0){
				afraid=false;
			}
		}
	}
}
