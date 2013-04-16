using UnityEngine;
using System.Collections;

public class LoginGUI : GUIScreen {
	float width = 500f;
	float height = 200f;
	float buttonWidth, buttonHeight, leftMargin, topMargin, heightBlock;
	public GUISkin skin;
	string username = "";
	string password = "";
	
	void Start (){
		width = 400;
		height = 200;
		buttonWidth = 100f;
		buttonHeight = 30f;
		leftMargin = 15f;
		topMargin = 15f;
		StartCoroutine(FadeIn());
	}
	
	protected override void DrawGUI(){
		GUI.skin = skin;
		GUI.BeginGroup(new Rect(targetWidth/2f - width/2f, targetHeight/2f - height/2f, width, height));
		GUI.Box(new Rect(0,0,width, height), "");
		GUI.Label(new Rect(width/2 - 200f, 0f, 400f, 70), "Alien Game", skin.customStyles[0]);
		GUI.Label(new Rect(width/2 - 105f, 70, 100f, 25f), "Username: ");
		username = GUI.TextField(new Rect(width/2, 70, 100f, 25f), username);
		GUI.Label(new Rect(width/2 - 105f, 100, 100f, 25f), "Password: ");
		password = GUI.PasswordField(new Rect(width/2, 100f, 100f, 25f), password, '*');
		if (GUI.Button(new Rect(width/2f - buttonWidth/2f, height-topMargin-buttonHeight, buttonWidth, buttonHeight), "Login"))
			Login();
		if (GUI.Button(new Rect(width - leftMargin - buttonWidth, height-topMargin-buttonHeight, buttonWidth, buttonHeight), "Register"))
			Register();
		GUI.EndGroup();
	}
	
	void Login(){
		if (username != "" && username == password){
			TransitionGUI.SwitchLevel("testlevel");
			StartCoroutine(FadeOut());
		}
	}
	
	void Register(){
		StartCoroutine(ScaleOut());
		StartCoroutine(gameObject.GetComponent<RegisterGUI>().ScaleIn());
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.Space))
			StartCoroutine(ScaleIn());
	}
}