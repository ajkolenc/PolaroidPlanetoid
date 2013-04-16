using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldGuideGUI : GUIScreen {
	
	int width = 500, height = 800;
	FieldGuidePage current;
	FieldGuideHome home;
	Rect screenRect;
	bool isHome = true;
	bool isOpen = false;
	
	void Start(){
		localBounds = new Rect(0, targetHeight - height, width, height);
		depth = 100;
		screenRect = new Rect(0, 0, width, height);
		home = gameObject.AddComponent<FieldGuideHome>();
		home.guideScreen = localBounds;
		home.parent = this;
//		StartCoroutine(FadeIn());
	}
	
	protected override void DrawGUI (){
		GUI.BeginGroup(screenRect);
		GUI.Box(new Rect(0,0,width,height),"");
		if (GUI.Button(new Rect(10, 10, 50, 50), "<")){
			if (!isHome){
				Home ();
				isHome = true;
			}
			else{
				Close();
			}
		}
		GUI.EndGroup();
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.Tab)){
			if (!isOpen)
				Open ();
			else
				Close();
		}
	}
	
	void Open(){
		Screen.lockCursor = false;
		Screen.showCursor = true;
		isOpen = true;
		StartCoroutine(MoveRight(screenRect, .3f));
		if (isHome)
			StartCoroutine(home.MoveRight(screenRect, .3f));	
		else
			StartCoroutine(current.MoveRight(screenRect, .3f));
	}
	
	public void Home(){
		isHome = true;
		StartCoroutine(home.MoveRight(screenRect, .3f));
		StartCoroutine(current.MoveRight(screenRect, .3f));
	}
	
	void Close(){
		StartCoroutine(MoveLeft(screenRect, .3f));
		if (isHome)
			StartCoroutine(home.MoveLeft(screenRect, .3f));
		else
			StartCoroutine(current.MoveLeft(screenRect, .3f));
		isOpen = false;
		Screen.lockCursor = true;
		Screen.showCursor = false;
	}
	
	public void SwitchToCreature(string name){
		Dictionary<string, GUIContent> info = DataHolder.GetCreatureData(name);
		isHome = false;
		FieldGuidePage newPage = gameObject.AddComponent<FieldGuidePage>();
		newPage.parent = this;
		newPage.name = info["name"];
		newPage.image = info["image"];
		newPage.description = info["description"];
		newPage.features = info["features"];
		newPage.notes = info["notes"];
		newPage.width = width;
		newPage.height = height;
		newPage.picWidth = 450;
		newPage.picHeight = 300;
		newPage.guideScreen = localBounds;
		StartCoroutine(home.MoveLeft(screenRect, .3f));
		StartCoroutine(newPage.MoveLeft(screenRect, .3f));
		current= newPage;
	}
}

internal class FieldGuidePage : GUIScreen {
	public GUIContent name, image, description, features, notes;
	public int width, height, picWidth, picHeight;
	public Rect guideScreen;
	public FieldGuideGUI parent;
	Rect size;
	bool getSkin = true;
	GUIStyle labelStyle, headerStyle,imageStyle, buttonStyle;
	GUISkin pageSkin;
	
	void Start(){
		
		localBounds = guideScreen;
		depth = 1;
		size = new Rect(15, 15, guideScreen.width-15, guideScreen.height - 15);
		width = (int)guideScreen.width-30;
		height = (int)guideScreen.height - 30;
		
		buttonStyle = new GUIStyle(parent.skin.button);
		buttonStyle.normal.textColor = Color.white;
		buttonStyle.fontSize = 24;
		buttonStyle.wordWrap = true;

		labelStyle = new GUIStyle(parent.skin.label);
		labelStyle.normal.textColor = Color.white;
		labelStyle.fontSize = 24;
		labelStyle.wordWrap = true;
		
		imageStyle = new GUIStyle();
		imageStyle.alignment = TextAnchor.MiddleCenter;
		
		headerStyle = new GUIStyle(parent.skin.label);
		headerStyle.normal.textColor = Color.white;
		headerStyle.fontSize = 40;
		headerStyle.wordWrap = true;
		headerStyle.alignment = TextAnchor.MiddleCenter;
	}
	
	protected override void DrawGUI (){
		GUI.BeginGroup(size);
		GUILayout.BeginVertical(GUILayout.Width(width-60));
//		GUILayout.Space(30);
		GUILayout.Label(name, headerStyle, GUILayout.Width(width));
		GUILayout.Space(10);
		GUILayout.Label(image, imageStyle, GUILayout.Width(picWidth), GUILayout.Height(picHeight));
		GUILayout.Space(20);
		GUILayout.Label(description, labelStyle, GUILayout.Width(width));
		GUILayout.Space(20);
		GUILayout.Label("Features: " + features.text, labelStyle, GUILayout.MaxWidth(width));
		GUILayout.Space(20);
		GUILayout.Label("Notes: \n" + notes.text, labelStyle, GUILayout.MaxWidth(width));
		GUILayout.EndVertical();
		GUI.EndGroup();
	}
}

internal class FieldGuideHome : GUIScreen {
	List<string> creatures;
	List<string> creatureNames;
	public Rect guideScreen;
	Rect size;
	GUIStyle labelStyle, headerStyle, buttonStyle;
	public FieldGuideGUI parent;
	bool getSkin = true;
	
	void Start(){
		localBounds = guideScreen;
		depth = 1;
		size = new Rect(30, 15, guideScreen.width-30, guideScreen.height - 15);
		creatureNames = new List<string>();
		creatures = new List<string>(DataHolder.creatureInfo.Keys);
		foreach (string s in creatures){
			creatureNames.Add(DataHolder.creatureInfo[s]["name"].text);
		}
		InitializeGUIStyles();		
	}
	
	void InitializeGUIStyles(){
		buttonStyle = new GUIStyle(parent.skin.button);
		buttonStyle.normal.textColor = Color.white;
		buttonStyle.border = new RectOffset(0,0,0,0);
		buttonStyle.margin = new RectOffset(0,0,0,0);
		buttonStyle.fontSize = 30;
		buttonStyle.wordWrap = true;

		labelStyle = new GUIStyle(parent.skin.label);
		labelStyle.normal.textColor = Color.white;
		labelStyle.fontSize = 24;
		labelStyle.alignment = TextAnchor.MiddleLeft;
		labelStyle.wordWrap = true;
		
		headerStyle = new GUIStyle(parent.skin.label);
		headerStyle.normal.textColor = Color.white;
		headerStyle.fontSize = 40;
		headerStyle.wordWrap = true;
		headerStyle.alignment = TextAnchor.MiddleCenter;
		getSkin = false;
	}
	
	protected override void DrawGUI (){
		GUILayout.BeginArea(size);
		GUILayout.BeginVertical();
//		GUILayout.Space(30);
		GUILayout.Label("Field Guide", headerStyle, GUILayout.Width(guideScreen.width));
		GUILayout.Space(20);
		int i = 0;
		foreach (string creature in creatures){
			if (GUILayout.Button(creatureNames[i], buttonStyle, GUILayout.Height(50), GUILayout.Width(450))){
				parent.SwitchToCreature(creature);
			}
			GUILayout.Space(10);
			i++;
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}