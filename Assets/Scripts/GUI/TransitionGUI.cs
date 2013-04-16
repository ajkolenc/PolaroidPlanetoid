using UnityEngine;
using System.Collections;

public class TransitionGUI : GUIScreen {
	
	Texture2D blackTex;
	static TransitionGUI guiReference;
	
	void Start () {
		depth = 0;
		useLetterBox(true);
		guiReference = this;
		blackTex = new Texture2D(1,1);
		blackTex.SetPixel(0,0,Color.black);
		blackTex.Apply();
		displayed = true;
		StartCoroutine(FadeOut(.6f));
	}
	
	protected override void DrawGUI (){
		GUI.DrawTexture(new Rect(0,0, GUIScreen.targetWidth, GUIScreen.targetHeight), blackTex);
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.H)){
			useLetterBox(true);	
		}
		if (Input.GetKeyDown(KeyCode.G)){
			useLetterBox(false);	
		}
	}
	
	public static IEnumerator SwitchLevel(string levelName, float time = 0.6f){
		yield return TransitionGUI.guiReference.StartCoroutine(TransitionGUI.guiReference.FadeIn(time));
		Application.LoadLevel(levelName);
	}
}
