using UnityEngine;
using System.Collections;

public class EndChoiceGUI : GUIScreen {
	
	Rect area;
	// Use this for initialization
	void Start () {
		StartCoroutine(FadeIn());
		area = new Rect(targetWidth/2f - 300, targetHeight/2f + 100, 600, 300);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	protected override void DrawGUI (){
		GUILayout.BeginArea(area);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Leave Notterra", GUILayout.Height(40))){
			Leave ();
		}
		if (GUILayout.Button("Stay on Notterra", GUILayout.Height(40))){
			Stay();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
	
	void Leave(){
		Application.Quit();
	}
	
	void Stay(){
		StartCoroutine(TransitionGUI.SwitchLevel("spaceship"));	
	}
}
