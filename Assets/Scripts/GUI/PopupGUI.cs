using UnityEngine;
using System.Collections;

public class PopupGUI : GUIScreen {
	
	public string message = "Error";
	public bool button = false;
	int width = 500;
	int height = 300;
	Texture2D blackTex;
	Rect windowBounds;
	public SpaceShipGUI parent;
	LightsOff background;
	GUISkin skin;
	
	void Start(){
		depth = 9;
		if (!button){
			height = 200;
		}
		windowBounds = new Rect(targetWidth/2f - width/2f, targetHeight/2f - height/2f, width, height);
		background = this.gameObject.AddComponent<LightsOff>();
		StartCoroutine(MoveDown(new Rect(0,0,targetWidth, targetHeight), .7f));
	}
	
	protected override void DrawGUI (){
		GUI.skin.label.fontSize = 30;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.skin.button.fontSize = 24;
		GUI.BeginGroup(windowBounds);
		GUI.Box(new Rect(0,0,width, height),"");
		GUI.Label(new Rect(30, 30, 440,140), message);
		if (button){
			if (GUI.Button(new Rect(width/2f - 100, 220, 200, 60), "Close")){
				Close();
			}
		}
		GUI.EndGroup();
	}
	
	void Close(){
		StartCoroutine(WrapDown(new Rect(0,0,targetWidth,targetHeight),.3f));
		StartCoroutine(this.background.FadeOut(.3f));
//		parent.MoveUp(new Rect(0,0,targetWidth, targetHeight), .7f);
	}
	
	void Update(){
		if (!displayed){
			Destroy(background);
			Destroy(this);	
		}
	}
}

internal class LightsOff : GUIScreen {
	Texture2D blackTex;
	
	void Start(){
		blackTex = new Texture2D(1,1);
		blackTex.SetPixel(0,0,new Color(0,0,0,.5f));
		blackTex.Apply();
		FadeIn();
	}

	protected override void DrawGUI (){
		depth = 10;
		GUI.DrawTexture(new Rect(0,0,targetWidth,targetHeight),blackTex);
	}
}