using UnityEngine;
using System.Collections;

//Little tentacle bits that hover off of the main body of a cube creature
public class TentacleBit : MonoBehaviour {
	//The gameobject these tentacle bits should follow
	public GameObject above;
	//Max distance that this gameobject can be away from the above object that it follows
	public float maxDistance=1.0f;
	//Speed at which this bit can move
	public float speed=5.0f;
	//bool's for use with random movement, the last (looks) just determines if this object should look at the gameObject it follows or not
	public bool rising=true,appearing, lefting,forthing, looks=true;
	//Used in random movement
	private float distanceFloatY,distanceFloatX,distanceFloatZ;
	
	void Start(){
		int val = Random.Range(0,2);
		
		//Determine which direction it should start moving with it's random hover
		if(val==1){
			rising=true;
			lefting=false;
			forthing=false;
		}
		else{
			rising=false;
			lefting=true;
			forthing=true;
		}
		
		//To start off set it to a random degree of transparency
		//renderer.material.color = new Color(Random.Range(0.9f,0.99f),Random.Range(0.9f,0.99f),Random.Range(0.9f,0.99f),Random.Range(0.5f,0.8f));
	}
	
	// Update is called once per frame
	void Update () {
		//Look at your above
		if(looks){
			//transform.LookAt(above.transform.position);
		}
		
		//Going up and down movement
		HoverVert();
		
		//Follow the above gameobject
		if(above!=null){
			Vector3 distToAbove = transform.position-above.transform.position;
			
			//if too far away from the above object, move closer
			if(distToAbove.magnitude>maxDistance){
				Vector3 newPos = transform.position;
				newPos-=(distToAbove/distToAbove.magnitude)*Time.deltaTime*speed;
				transform.position=newPos;
				
			}
		}
		
		
	}
	
	void HoverVert(){
		if(rising){
			if(distanceFloatY<0.3f){
				Vector3 hoverPos = transform.position;
				int multiplier = Random.Range(0,3);
				hoverPos.y+=(Time.deltaTime/2.0f)*multiplier;
				distanceFloatY+=(Time.deltaTime/2.0f)*multiplier;
				
				transform.position=hoverPos;
			}
			else{
				rising=false;
			}
		}
		else{
			if(distanceFloatY>-0.3f){
				Vector3 hoverPos = transform.position;
				int multiplier = Random.Range(0,3);
				hoverPos.y-=(Time.deltaTime/2.0f)*multiplier;
				distanceFloatY-=(Time.deltaTime/2.0f)*multiplier;
				
				transform.position=hoverPos;
			}
			else{
				rising=true;
			}
		}
	}
	
	//Unnused, maybe later
	/**
	void HoverHorz(){
		if(lefting){
			if(distanceFloatX<0.3f){
				Vector3 hoverPos = transform.position;
				int multiplier = Random.Range(0,3);
				hoverPos.x+=(Time.deltaTime/2.0f)*multiplier;
				distanceFloatX+=(Time.deltaTime/2.0f)*multiplier;
				
				transform.position=hoverPos;
			}
			else{
				rising=false;
			}
		}
		else{
			if(distanceFloatX>-0.3f){
				Vector3 hoverPos = transform.position;
				int multiplier = Random.Range(0,3);
				hoverPos.x-=(Time.deltaTime/2.0f)*multiplier;
				distanceFloatX-=(Time.deltaTime/2.0f)*multiplier;
				
				transform.position=hoverPos;
			}
			else{
				rising=true;
			}
		}
	}
	
	void HoverZ(){
		if(forthing){
			if(distanceFloatZ<0.3f){
				Vector3 hoverPos = transform.position;
				int multiplier = Random.Range(0,3);
				hoverPos.z+=(Time.deltaTime/2.0f)*multiplier;
				distanceFloatZ+=(Time.deltaTime/2.0f)*multiplier;
				
				transform.position=hoverPos;
			}
			else{
				rising=false;
			}
		}
		else{
			if(distanceFloatZ>-0.3f){
				Vector3 hoverPos = transform.position;
				int multiplier = Random.Range(0,3);
				hoverPos.z-=(Time.deltaTime/2.0f)*multiplier;
				distanceFloatZ-=(Time.deltaTime/2.0f)*multiplier;
				
				transform.position=hoverPos;
			}
			else{
				rising=true;
			}
		}
	}
	*/
}
