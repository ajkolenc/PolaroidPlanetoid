using UnityEngine;
using System.Collections;

//Just holds objects, we don't have throwing stuff
//And we just need to write a basic object physics script
public class ObjectHolder : MonoBehaviour {
	//Used to determine Vector to grab an object
	public GameObject look;
	//Object we're carrying
	private PickupableObject carryObj;
	private float clickTimer=0.1f,mouseHeld;
	private float playerRange= 7.0f, playerArmLength = 1.5f;
	
	public KeyCode throwObjectKey = KeyCode.F;
	//The charContr using this thing
	public CapsuleCollider charContr;
	//The hand symbol
	
	//Determines if we're over the obj or not
	public GUITexture hand;
	
	// Use this for initialization
	void Start () {
		look = GameObject.Find("Look");
	}
	
	// Update is called once per frame
	void Update () {
		if(carryObj==null){
			
			//if(Input.GetMouseButtonDown(0) && clickTimer<0){
				RaycastHit hit;
				
        		Vector3 p1 = charContr.center + transform.position + Vector3.up *  charContr.height * 0.5F;
        		Vector3 p2 = p1 + Vector3.up * charContr.height;
				
				// Debug.DrawLine(p2,playerRange*(look.transform.position-p2)+p2);
				
				//Debug.DrawLine(transform.position, (transform.forward*7)+transform.position);
				if(Physics.Raycast(transform.position, transform.forward, out hit, playerRange)){
					//|| Physics.Raycast(transform.position, look.transform.position-transform.position, out hit, playerRange)){
					
					
					GameObject carryObjGO = hit.collider.gameObject;
					
					PickupableObject carryObjPotential = carryObjGO.GetComponent<PickupableObject>();
					
					if(carryObjPotential!=null ){
						//print("Should be enabled");
						hand.enabled=true;
						if(Input.GetMouseButtonDown(0) && clickTimer<0){
							carryObj=carryObjPotential;
							//Make object at arm length
							Vector3 differenceToHit = hit.transform.position-transform.position;
							differenceToHit.Normalize();
							differenceToHit*=playerArmLength;
							hit.transform.position=transform.position+differenceToHit;
							
							carryObj.transform.position-=0.5f*transform.up;
							carryObj.transform.parent=transform;
							if(carryObj!=null && carryObj.rigidbody!=null && !carryObj.rigidbody.isKinematic){
								carryObj.rigidbody.velocity=Vector3.zero;
							}
							carryObj.collider.enabled=false;
							carryObj.rigidbody.useGravity=false;
							carryObj.rigidbody.freezeRotation=true;
							clickTimer=0.1f;
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
		else{
			hand.enabled=false;	
			//Throw ze Object!
			if(Input.GetKeyDown(throwObjectKey)){
				carryObj.transform.parent=null;
				carryObj.rigidbody.useGravity=true;
				//carryObj.velocity = 0.4f*(look.transform.position-transform.position);
				carryObj.rigidbody.AddForce(300.0f*(transform.forward));
				carryObj.rigidbody.freezeRotation=false;
				
				carryObj.collider.enabled=true;
				carryObj=null;
				
				clickTimer=0.5f;
			}
			
			if(Input.GetMouseButtonDown(0)){
				//Still holding
				
				
				
				
			}
			else if(carryObj!=null){
				if(Input.GetMouseButtonUp(0)){
					carryObj.transform.parent=null;
					
					carryObj.rigidbody.freezeRotation=false;
					carryObj.rigidbody.useGravity=true;
					
					carryObj.collider.enabled=true;
					
					carryObj=null;
					
				}
			}
			
				
				
		}
		
		
			
		
		
		if(clickTimer>=0){
			clickTimer-=Time.deltaTime;
		}
		
	}
	
	
	public void ReleaseObj(){
		if(carryObj!=null){
				
			carryObj.transform.parent=null;
					
			carryObj.rigidbody.freezeRotation=false;
			carryObj.rigidbody.useGravity=true;
					
			carryObj.collider.enabled=true;
					
			carryObj=null;
					
				
		}
	}
}
