using UnityEngine;
using System.Collections;

//ATTACH TO THE THING THAT HAS CHARACTTER MOTION ON IT
[RequireComponent (typeof (CharacterMotion))]
public class JetpackJump : Powerup {
	float jumpSpeed = 30;
	
	//TESTING, THIS'LL NEED TO BE CALED FROM ANOTHER CLASS/CONTROLLER
	void Start(){
		Setup();
	}
	
	void Update(){
		if(Input.GetKey(KeyCode.Space)){
			transform.position += new Vector3(0,Time.deltaTime*jumpSpeed,0);
		}
	}
	
	public override void Setup(){
		//CharacterMotion cm = gameObject.GetComponent<CharacterMotion>();
		//cm.jumpSpeed = 50;
		//Once we increase jump, destroy this power
		//print("Gott into jetpackjump");
		//Destroy(this);
	}
	
}
