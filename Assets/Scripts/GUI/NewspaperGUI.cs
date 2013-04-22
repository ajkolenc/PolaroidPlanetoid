using UnityEngine;
using System.Collections;

public class NewspaperGUI : GUIScreen {
	Texture2D newspaper, picture;
	GUIStyle headerStyle;
	string headline;

	// Use this for initialization
	void Start () {
		headline = getHeadline();
		headerStyle = new GUIStyle();
		headerStyle.fontSize = 134;
		headerStyle.normal.textColor = Color.black;
		headerStyle.stretchWidth = true;
		headerStyle.alignment = TextAnchor.MiddleCenter;
		newspaper = getNewspaperImage();
		picture = getPicture();
		useLetterBox(true);
		StartCoroutine(CoStart());
	}
	
	IEnumerator CoStart(){
		yield return StartCoroutine(SpinIn());
		yield return new WaitForSeconds(3f);
		StartCoroutine(TransitionGUI.SwitchLevel("spaceship", 1f));
//		StartCoroutine(FadeOut(1f));
	}
	
	protected override void DrawGUI (){
		GUI.DrawTexture(new Rect(0,0,targetWidth, targetHeight), newspaper);
		GUI.Label(new Rect(40, 300, targetWidth-80, 140), headline, headerStyle);
		GUI.DrawTexture(new Rect(500, 470, 920, 440), picture);
	}
	
	string getHeadline(){
		return "New Creature Discovered!";	
	}
	
	Texture2D getPicture(){
		return DataHolder.newspicture;	
	}
	
	Texture2D getNewspaperImage(){
		return Resources.Load("newspaper") as Texture2D;
	}
}
