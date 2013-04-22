using UnityEngine;
using System.Collections;

public class DragonController : BasicCreature {
	public AudioClip snore; 
	//The time between snores
	public float timeBetweenSnores = 5.0f;
	private float snoreTimer;
	
	
	void Update(){
		if(snoreTimer<timeBetweenSnores){
			snoreTimer+=Time.deltaTime*Random.Range(0.5f,1.5f);
		}
		else{
			snoreTimer=0.0f;
			audio.PlayOneShot(snore);
		}
	}
	
	
}
