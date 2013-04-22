using UnityEngine;
using System.Collections;

public class RegisterGUI : GUIScreen {
	float width = 500f;
	float height = 200f;
	float buttonWidth, buttonHeight, topMargin, heightBlock;
	public GUISkin skin;
	string username = "";
	string password = "";
	string passwordConfirm = "";
	
	void Start ()
	{
		width = 400;
		height = 220;
		buttonWidth = 100f;
		buttonHeight = 30f;
		topMargin = 15f;
	}
	
	protected override void DrawGUI(){
		GUI.skin = skin;
		GUI.BeginGroup(new Rect(targetWidth/2f - width/2f, targetHeight/2f - height/2f, width, height));
		GUI.Box(new Rect(0,0,width, height), "");

		GUI.Label(new Rect(width/2 - 200f, 0f, 400f, 70), "New Account", skin.customStyles[0]);

		GUI.Label(new Rect(width/2 - 105f, 70, 100f, 25), "Username: ");
		username = GUI.TextField(new Rect(width/2, 70, 100f, 25f), username);
		GUI.Label(new Rect(width/2 - 105f, 100, 100f, 25f), "Password: ");
		password = GUI.PasswordField(new Rect(width/2, 100, 100f, 25f), password, '*');
		GUI.Label(new Rect(width/2 - 195f, 130, 190f, 25f), "Confirm Password: ");
		passwordConfirm = GUI.PasswordField(new Rect(width/2, 130, 100f, 25f), passwordConfirm, '*');

		if (GUI.Button(new Rect(width/2f - buttonWidth - 5, height-topMargin-buttonHeight, buttonWidth, buttonHeight), "Create Account"))
			MakeAccount();
		if (GUI.Button(new Rect(width/2f + 5, height-topMargin-buttonHeight, buttonWidth, buttonHeight), "Clear"))
			Clear();
		GUI.EndGroup();
	}
	
	void MakeAccount(){
		StartCoroutine(ScaleOut());
		StartCoroutine(gameObject.GetComponent<LoginGUI>().ScaleIn());
	}
	
	void Clear(){
		username = "";
		password = "";
		passwordConfirm = "";
	}
}
