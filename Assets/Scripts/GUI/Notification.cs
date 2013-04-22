using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Notification : GUIScreen {
	
//	public FamilyMember owner;
	public string content;
	public bool bigNotification = true;
	public string displayedContent = "", underscore="";
	public string transitionType = "Wrap";
	public delegate void buttonAction();
	public Dictionary<string, buttonAction> buttons, nextButtons;
	public bool comeIn = false;
	public bool inSpaceShip = false;
	public bool isSal = true;
	public bool typing = false;
	static Notification notifier = null;
	int width = 600;
	int height = 150;
	int buttonHeight = 30;
	int buttonWidth = 50;
	GUIStyle header, boxStyle, nameStyle;
	Timer timeout;
	float typeTime;
	float timeoutTime = 0;
	Rect windowBounds;
	Texture2D greyPic;
	bool word = false;
	bool underscoring = false;
	Sprite test;
	SpriteAnimation spriteAnimation;
	
	protected override void Awake (){
		base.Awake ();
		this.depth = 10;
	}
	void Start (){
		Notification.notifier = this;
		header = new GUIStyle(skin.label);
		header.alignment = TextAnchor.MiddleLeft;
		boxStyle = new GUIStyle(skin.box);
		buttons = new Dictionary<string, buttonAction>();
		nameStyle = new GUIStyle();
		nameStyle.fontSize = 18;
		nameStyle.normal.textColor = Color.white;
		nameStyle.alignment = TextAnchor.MiddleCenter;
		
		if (bigNotification){
			header.fontSize = 32;
			width = 1000;
			height = 200; 
			windowBounds = new Rect(targetWidth/2 - width/2, targetHeight/2f - height/2f, width, height);
		}
		else{
			header.fontSize = 26;
			windowBounds = new Rect(targetWidth/2 - width/2, targetHeight - height - 20, width, height);			
		}
		if (inSpaceShip){
			header.fontSize = 26;
			width = 585;
			height = 200;
			windowBounds = new Rect(targetWidth/2f - 70, 80, 585, 200);	
		}
		localBounds = windowBounds;
		if (isSal){
			test = new Sprite("sal3", 170, 128);
		}
		else
			test = new Sprite("cy3", 169, 128);
		spriteAnimation = test.animations[0];
		spriteAnimation.repeat = true;
		if (comeIn)
			StartCoroutine(CoStart());
	}
	
	public IEnumerator CoStart(){
//		StartCoroutine(Timeout(5));		
		yield return StartCoroutine(WrapUp(windowBounds));
		StartCoroutine(TypeInContent(.06f));
	}
	
	public IEnumerator TypeInContent(float time = 0.06f){
		typeTime = time;
		typing = true;
		int i = 1;
		bool ending = true;
		if (!underscoring){
			StartCoroutine(AddUnderscore());
			underscoring = true;
		}
		while (i <= content.Length){
			word = true;
			displayedContent = content.Substring(0, i);
			if (content[i-1] == ' ' || content[i-1] == ','){
				word = false;
				yield return new WaitForSeconds(2*typeTime);
			}
			else if (content[i-1] == '.' || content[i-1] == '!'){
				word = false;
				yield return new WaitForSeconds(8*typeTime);
			}
			else
				yield return new WaitForSeconds(typeTime);	
			i++;
		}
		word = false;
		spriteAnimation.StopAnimation();
		typing = false;
	}
	
	IEnumerator AddUnderscore(){
		bool present = false;
		while (displayed){
			if (present){
				underscore = "_";
				present = false;
			}
			else{
				underscore = " ";
				present = true;
			}
			yield return new WaitForSeconds(.3f);
		}
	}
	
	public IEnumerator Timeout(float time){
		timeoutTime = time;
		while (typing){
			yield return 0;
		}
		Timer timeout = new Timer(timeoutTime);
		while (!timeout.IsFinished()){yield return 0;}
		yield return StartCoroutine(Close());
	}
	
	public static IEnumerator Notify(string content, bool largeSize, float timeout = 0f){
		GUISkin skin = Notification.notifier.skin;
		if (Notification.notifier != null && Notification.notifier.displayed){
			yield return Notification.notifier.StartCoroutine(Notification.notifier.Close());
		}
		Notification.notifier = CameraGUI.camera.gameObject.AddComponent<Notification>();
		Notification.notifier.comeIn = true;
		Notification.notifier.skin = skin;
		Notification.notifier.content = content;
		Notification.notifier.bigNotification = largeSize;
		if (timeout>0f){
			Notification.notifier.StartCoroutine(Notification.notifier.Timeout(timeout));	
		}
	}
	
	protected override void DrawGUI (){
		GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(width), GUILayout.Height(height));
		GUILayout.Space(180);
		if (word){
			spriteAnimation.Continue();
			spriteAnimation.repeat = true;	
		}
		else{
			spriteAnimation.SetFrame(0);
		}
		GUI.DrawTexture(new Rect(15, 15, 150, height-50), spriteAnimation.PlayAnimation(.1f));
		if (isSal){
			GUI.Label(new Rect(15, height - 40, 150, 30), "Sal", nameStyle); 
		}
		else {
			GUI.Label(new Rect(15, height - 40, 150, 30), "Cy", nameStyle); 
		}
		GUILayout.Label(displayedContent+underscore, header, GUILayout.Width(width-15-180), GUILayout.Height(height));
		GUILayout.Space(20);
		GUILayout.EndHorizontal();
		
//		GUI.Label(new Rect(79, 15, windowBounds.width - 79, windowBounds.height - 15), content);
	/*
		foreach (KeyValuePair<string, buttonAction> button in buttons){
			if (buttons.Count%2 == 1)
				widthModifier = buttonNumber-(buttons.Count/2);
			else{
				widthModifier = buttonNumber-(buttons.Count/2) + .5f;
			}

			if (GUI.Button(new Rect(width/2f - (buttonWidth/2f) + (buttonWidth+10)*(widthModifier),
									height - buttonHeight - 10,
									buttonWidth, buttonHeight), button.Key)){
				button.Value();
			}
			buttonNumber++;
		}
//		GUI.DragWindow();
*/
	}
	
	public void Transition(string type, bool TransitionIn){
		switch (type){
		
		case "Wrap":
			if (TransitionIn){
				WrapUp(windowBounds);
			}
			else
				WrapDown(windowBounds);
			break;
		case "Fade":
			if (TransitionIn)
				FadeIn();
			else
				FadeOut ();
			break;
		}
	}
	
	void Update(){
		if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Space)){
			if (typing){
				typeTime = 0.01f;
			}
		}
		else{
			typeTime = 0.06f;	
		}
	}
	
	public IEnumerator Close(){
		yield return StartCoroutine(WrapDown(windowBounds, .5f));
	}
	
	void testButton(){
		WrapDown(windowBounds);
	}
	
	void testButton2(){
		MoveRight(windowBounds,2);
	}
}