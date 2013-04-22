using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureCompass : MonoBehaviour {

	bool hidden = false;
	Transform closestCreature;
	List<Transform> creatures;
	
	void Start(){
		creatures = new List<Transform>();
		GameObject[] creatureObjects = GameObject.FindGameObjectsWithTag("CreatureCore");
		foreach (GameObject o in creatureObjects){
			print (o.name);
			creatures.Add(o.transform);	
		}
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)){
			if (hidden){
				Show();
			}
			else
				Hide();
		}
		FindClosestCreature();
//		AlignWithCamera();
		PointAtCreature();
	}
	
	public void RemoveCreature(Transform t){
		if (creatures.Contains(t)){
			creatures.Remove(t);	
		}
	}
	
	void FindClosestCreature(){
		closestCreature = creatures[0];
		float shortestDistance = float.MaxValue;
		foreach (Transform t in creatures){
			float dist = (t.position - transform.position).magnitude;
			if (dist < shortestDistance){
				closestCreature = t;
				shortestDistance = dist;
			}
		}
		//print (closestCreature.gameObject.name);
	}
	
	void PointAtCreature(){
		Vector3 axis = closestCreature.transform.position - gameObject.transform.position;
		axis.y = 0;
		axis.Normalize();
		transform.forward = axis;
	}
	
	void AlignWithCamera(){
		transform.up = transform.parent.up;
//		transform.rotation = Quaternion.Euler(new Vector3(transform.parent.rotation.eulerAngles.x, 0f, 0f));
	}
	
	void Hide(){
		hidden = true;
		for (int i = 0; i < transform.childCount; i++){
			transform.GetChild(i).gameObject.renderer.enabled = false;
		}
	}
	
	void Show(){
		hidden = false;
		for (int i = 0; i < transform.childCount; i++){
			transform.GetChild(i).gameObject.renderer.enabled = true;
		}
	}
}
