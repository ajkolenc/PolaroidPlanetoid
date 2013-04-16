using UnityEngine;
using System.Collections;


//Attached to first person controller
public class WaterShoes : Powerup{
	
	
	void Start(){
		Setup();
	}
	
	public override void Setup ()
	{
		base.Setup ();
		
		CharacterMotion cMotion = gameObject.GetComponent<CharacterMotion>();
		
		for(int i = 0; i<cMotion.tagsNotToGoTo.Length; i++){
			if(cMotion.tagsNotToGoTo[i]=="Water"){
				cMotion.tagsNotToGoTo[i]="";
			}
		}
		
	}
}
