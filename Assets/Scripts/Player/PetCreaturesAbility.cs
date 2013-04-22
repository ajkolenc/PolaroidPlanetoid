using UnityEngine;
using System.Collections;

public class PetCreaturesAbility : MonoBehaviour {

	
//Used to determine Vector to grab an object
	public GameObject look;
	//Object we're carrying
	private float playerRange= 7.0f, playerArmLength = 1.5f;
	
	//The charContr using this thing
	public CapsuleCollider charContr;
	//The hand symbol
	
	//Shows if we're over the obj or not
	public GUITexture hand;
	
	// Use this for initialization
	void Start () {
		look = GameObject.Find("Look");
	}
	
	// Update is called once per frame
	void Update () {
			
			//if(Input.GetMouseButtonDown(0) && clickTimer<0){
				RaycastHit hit;
				
        		Vector3 p1 = charContr.center + transform.position + Vector3.up *  charContr.height * 0.5F;
        		Vector3 p2 = p1 + Vector3.up * charContr.height;
				
				// Debug.DrawLine(p2,playerRange*(look.transform.position-p2)+p2);
				
				//Debug.DrawLine(transform.position, (transform.forward*7)+transform.position);
				if(Physics.Raycast(transform.position, transform.forward, out hit, playerRange)){
					//|| Physics.Raycast(transform.position, look.transform.position-transform.position, out hit, playerRange)){
					
					
					GameObject pettableGO = hit.collider.gameObject;
					
					Pettable creatureObjPotential = pettableGO.GetComponent<Pettable>();
					
					if(creatureObjPotential!=null ){
						//print("Should be enabled");
						hand.enabled=true;
						if(Input.GetMouseButtonDown(0)){
							creatureObjPotential.Pet();
						}	
					}
					else{
					
						
					
						hand.enabled=false;
					}
				
				}
				else{
					hand.enabled=false;	
				}
			
			
		
		
		
		
	}
	
}

