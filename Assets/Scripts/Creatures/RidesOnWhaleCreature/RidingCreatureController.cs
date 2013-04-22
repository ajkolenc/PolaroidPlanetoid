using UnityEngine;
using System.Collections;

public class RidingCreatureController : BasicCreature {
	
	public GameObject body;
	//Animation
	public int curAnimState = -1;
	
	public Vector3 homePosition;
	const int SITTING =0;
	const int FLYING =1;
	
	private float timer = 0.0f;
	private float normalTime = 4.0f;
	private float flyTime = 2.0f;
	
	//Creature sound stuff
	public AudioClip randomCreatureSound; 
	public float timeBetweenSounds=5.0f; 
	private float soundTimer;
	
	public float maxEnergy = 200;
	private float energy = 0;
	public float sleepLevel = 30;
	private bool sleeping = false;
	public SleepHandler sleepHandler;
	
	void Start(){
		homePosition = transform.localPosition;
		energy = maxEnergy;
	}
	
	void Update(){
		
		if(!sleeping){
			
			
			if(soundTimer<timeBetweenSounds){
				soundTimer+=Time.deltaTime*Random.Range(0.5f,1.5f);
			}
			else{
				soundTimer=0;
				if(randomCreatureSound!=null){
					audio.PlayOneShot(randomCreatureSound);
				}
			}
			
			
			//Reset
			if(curAnimState==-1){
				curAnimState = Random.Range(0,2);
				if(curAnimState==0){
					timer=normalTime;
				}
				else if(curAnimState==1){
					timer = flyTime;
					body.animation.Play();
				}
				
			}
			else if(curAnimState==SITTING){
				energy-=Time.deltaTime;
				
				if(energy<sleepLevel){
					sleeping=true;
				}
				
				
				if(timer>=0){
					timer-=Time.deltaTime*Random.Range(0.8f,1.0f);
				}
				else{
					curAnimState=-1;
				}
			}
			else if(curAnimState==FLYING){
				energy-=Time.deltaTime*20;
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
		else{
			if(sleepHandler.Sleeping()){
				sleeping=false;
				energy = maxEnergy;
			}
		}
		
		
	}
}
