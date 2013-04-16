using UnityEngine;
using System.Collections;

public class MainMenuGUI : GUIScreen {
	
	float width = 500f;
	float height = 200f;
	float buttonWidth, buttonHeight, heightBlock;
	public GUISkin skin;

	void Start (){
		width = 400;
		height = 220;
		buttonWidth = 100f;
		buttonHeight = 50f;
		StartCoroutine(FadeIn());
	}
	
	protected override void DrawGUI(){
		GUI.skin = skin;
		GUI.BeginGroup(new Rect(targetWidth/2f - width/2f, targetHeight/2f - height/2f, width, height));
		GUI.Box(new Rect(0,0,width, height), "");
		GUI.Label(new Rect(width/2 - 200f, 0f, 400f, 70), "Main Menu", skin.customStyles[0]);
		if (GUI.Button(new Rect(width/2 - buttonWidth/2f, 80, buttonWidth, buttonHeight),"Start Game"))
			StartGame();
		if (GUI.Button(new Rect(width/2 - buttonWidth/2f, 150f, buttonWidth, buttonHeight), "Credits"))
			Credits();
		GUI.EndGroup();
	}
	
	void StartGame(){
		TransitionGUI.SwitchLevel("spaceship");
		StartCoroutine(ScaleOut());
	}
	
	void Credits(){
		StartCoroutine(this.gameObject.GetComponent<CreditsGUI>().ScaleIn());
		StartCoroutine(ScaleOut());
	}
}
