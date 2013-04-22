using UnityEngine;
using System.Collections;

public class Pettable : MonoBehaviour {

	public void Pet(){
		BasicCreature bc = gameObject.GetComponent<BasicCreature>();
		
		if(bc!=null){
			bc.Pet();
		}
	}
}
