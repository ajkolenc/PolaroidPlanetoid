using UnityEngine;
using System.Collections;

public class PauseMenuGUI : GUIScreen {
	
	float width = 800f;
	float height = 700f;
	float originalFixedDeltaTime;
	float buttonWidth, buttonHeight, heightBlock;
	bool paused = false;
	public GUISkin skin;
	Texture2D blackTex, controlsPic;
	
	void Start (){
		blackTex = new Texture2D(1,1);
		blackTex.SetPixel(0,0,new Color(0,0,0,.3f));
		blackTex.Apply();
		originalFixedDeltaTime = Time.fixedDeltaTime;
		controlsPic = Resources.Load("controls") as Texture2D;
		buttonWidth = 200f;
		buttonHeight = 50f;
		if (DataHolder.tutorial){
			Time.timeScale = 0;
			Time.fixedDeltaTime = 0;
			StartCoroutine(FadeIn());
			paused = true;
			Screen.lockCursor = false;
			Screen.showCursor = true;
			DataHolder.tutorial = false;
		}
	}
	
	protected override void DrawGUI(){
		if (paused){
			Screen.lockCursor = false;
			Screen.showCursor = true;
		}
		GUI.skin = skin;
		GUI.DrawTexture(new Rect(0,0,2*targetWidth,2*targetHeight),blackTex, ScaleMode.StretchToFill);
		GUI.BeginGroup(new Rect(targetWidth/2f - width/2f, targetHeight/2f - height/2f, width, height));
		GUI.Box(new Rect(0,0,width, height), "");
		GUI.Label(new Rect(width/2 - 200f, 0f, 400f, 70), "Pause");
		GUI.DrawTexture(new Rect(width/2 - 300, 90, 600, 500), controlsPic);
		if (GUI.Button(new Rect(width/2 - buttonWidth - 5, height-80, buttonWidth, buttonHeight),"Back to Ship"))
			MainMenu();
		if (GUI.Button(new Rect(width/2 + 5, height-80f, buttonWidth, buttonHeight), "Continue"))
			Login();
		GUI.EndGroup();
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (paused){
				Time.fixedDeltaTime = originalFixedDeltaTime;
				Time.timeScale = 1;
				StartCoroutine(FadeOut(.2f));
				paused = false;
				Screen.lockCursor = true;
				Screen.showCursor = false;
			}
			else{
				StartCoroutine(PauseGame());
			Screen.lockCursor = false;
			Screen.showCursor = true;
			}
		}
	}
	
	IEnumerator PauseGame(){
		StartCoroutine(FadeIn());
		originalFixedDeltaTime = Time.fixedDeltaTime;
		Time.timeScale = 0.00001f;
		Time.fixedDeltaTime = 0.00000001f;
		paused = true;
		yield return 0;
	}
	
	void MainMenu(){
		Time.fixedDeltaTime = originalFixedDeltaTime;
		Time.timeScale = 1;
		DayNightCycle.ToReview();
		StartCoroutine(FadeOut(.2f));
	}
	
	void Login(){
		StartCoroutine(FadeOut(.2f));
		paused = false;
		Time.fixedDeltaTime = originalFixedDeltaTime;
		Time.timeScale = 1;
		Screen.lockCursor = true;
		Screen.showCursor = false;
	}
}