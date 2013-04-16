using UnityEngine;
using System.Collections;

//Parent of all the enemies
public class BasicEnemy : MonoBehaviour {
	//Reference to player
	public GameObject player;
	//Total hitpoints, change this for individual enemies
	public float hitpoints = 50.0f;
	//the speed of the enemy
	public float speed;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		Movement();
	}
	
	//No movement here, to be overridden
	public virtual void Movement(){}
	
	//Basic way to handle taking damage
	public virtual void ApplyDamage(float amount){
		if(hitpoints>0){
			hitpoints-=amount;
		}
		else{
			Destroy(gameObject);
		}
	}
	
	
}
