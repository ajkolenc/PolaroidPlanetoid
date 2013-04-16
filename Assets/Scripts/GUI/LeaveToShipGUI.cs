using UnityEngine;
using System.Collections;

public class LeaveToShipGUI : GUIScreen {

	Rect leaveShip = new Rect(30, 30, 300, 50);
	
	void Start(){
		StartCoroutine(FadeIn());	
	}
	
	protected override void DrawGUI (){
		if (GUI.Button(leaveShip, "Back to Ship")){
			DayNightCycle.ToReview();
		}
	}
}
