using UnityEngine;
using System.Collections;

public class TestCreature : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		
		if(Physics.Raycast(transform.position, -1*transform.up, out hit, 1.2f)){
			transform.rotation = Quaternion.LookRotation(transform.forward, hit.normal);
			
			
			if(hit.distance<1.2f){
				transform.position+=(1.2f-hit.distance)*transform.up;
			}
		}
		
		transform.position+=Time.deltaTime*transform.forward;
	}
}
