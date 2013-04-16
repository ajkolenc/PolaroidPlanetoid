using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {
	public MovementController movementController;
	Memory bob;
	// Use this for initialization
	void Start () {
		GameObject playah = GameObject.FindGameObjectWithTag("Player") as GameObject;
		
		bob = new Memory(playah, 0, 11.0f);
	}
	
	// Update is called once per frame
	void Update () {
		movementController.setWalking(true);
		movementController.MoveTowards(new Vector3(0,0,0), 10);
		
		
	}
}
