using UnityEngine;
using System.Collections;

public class WaveY: MonoBehaviour {
	
	bool afraid;
	float afraidTimer;
	float timer;
	public bool forth;
	
	void Start(){
		
	}
	
	// Update is called once per frame
	void Update () {
		float amnt = Time.deltaTime*20.0f * Random.Range(0.7f,0.9f);
		
		if(forth){
			amnt*=-1;
		}
		
		
		transform.Rotate(new Vector3(0,amnt,0));
		
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
