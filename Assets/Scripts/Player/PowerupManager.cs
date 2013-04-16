using UnityEngine;
using System.Collections;

public class PowerupManager : MonoBehaviour {
	
	GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		foreach (string power in DataHolder.powerUps){
			print (power);
			if (DataHolder.previousPowerUps.Contains(power)){
				AddPower(power);	
			}
			else{
				StartCoroutine(Notification.Notify("You've got a new gadget, the " + power + "!",false	, 5f));
				AddPower(power);
			}
		}
		DataHolder.previousPowerUps = DataHolder.powerUps;
	}
	
	void AddPower(string power){
		switch (power){
		case "Water Shoes":
			player.AddComponent<WaterShoes>();
			break;
		case "Flashlight":
			player.transform.FindChild("Main Camera").gameObject.AddComponent<Flashlight>();
			break;
		case "Jetpack":
			player.AddComponent<JetpackJump>();
			break;
		}
	}
}
