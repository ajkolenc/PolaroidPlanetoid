using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReviewGUI : GUIScreen {
	
	List<Texture2D> pics;
	List<PictureData> data;
	List<string> currentCreatures;
	List<float> picScores;
	List<string> picDescriptions;
	int currentIndex = 0;
	int newCreatures = 0;
	Rect pictureRect = new Rect(targetWidth/2f - 500, 60, 1000, 600);
	Rect previewRect = new Rect(targetWidth/2f - 500, 660, 1000, 300);
	Rect buttonsRect = new Rect(targetWidth/2f + 500, 60, 300, 600);
	Rect pictureMovingRect = new Rect(0, 60, targetWidth, 600);
	Vector2 previewScroll, newScrollPosition, oldScrollPosition;
	Dictionary<string, int> creaturePicSet;
	bool lockPicture = false;
	bool newCreaturePopup = false;
	BigPicture currentPic;
	float scrollTime = .6f;
	Texture2D checkbox;
	GUIStyle labelStyle, buttonStyle;
	
	void Start () {
		labelStyle = new GUIStyle(skin.label);
		labelStyle.fontSize = 24;
		
		buttonStyle = new GUIStyle(skin.button);
		buttonStyle.fontSize = 24;
		buttonStyle.margin = new RectOffset(0,0,0,0);
		
		picScores = new List<float>();
		picDescriptions = new List<string>();
		
		if (DataHolder.pictures.Count > 0){
			creaturePicSet = new Dictionary<string, int>();
			pics = new List<Texture2D>();
			data = new List<PictureData>();
			currentCreatures = new List<string>();
			checkbox = Resources.Load("checkbox") as Texture2D;
			foreach (KeyValuePair<Texture2D, PictureData> pair in DataHolder.pictures){
				pics.Add(pair.Key);
				data.Add(pair.Value);
			}
			
			ProcessPictures();
			
			currentPic = this.gameObject.AddComponent<BigPicture>();
			currentPic.picture = pics[0];
			currentPic.scoreText = picDescriptions[0];
			currentPic.setLocalBounds(pictureRect);
			
			foreach (string s in data[0].namesOfCreatures){
				if (s != null)
					currentCreatures.Add(s);
			}

			StartCoroutine(currentPic.FadeIn());
		}
		else{
			pics = new List<Texture2D>();
			data = new List<PictureData>();
		}
		Screen.lockCursor = false;
		Screen.showCursor = true;
		StartCoroutine(FadeIn());
	}
	
	void ProcessPictures(){
		int j = 0;
		foreach (PictureData p in data){
			int i = 0;
			float highestScore = 0;
			if (p.namesOfCreatures != null){
				foreach (string creature in p.namesOfCreatures){
					
					float score = 0;
					// New Creature Bonus
					if (PlayerPrefs.GetInt(creature) != 143){
						PlayerPrefs.SetInt(creature,143);
						DataHolder.creatureScores["Bonus"][0] = DataHolder.creatureScores["Bonus"][0] + 1000;
						DataHolder.creatureScores.Add(creature, new Dictionary<float, float>());
						newCreatures++;
						newCreaturePopup = true;
					}
					Dictionary<string, float> criteria = p.scoringCriteria[i];
					
					// Score the picture
					score = criteria["light"] * criteria["facing"] * criteria["center"] * criteria["rareness"] * (criteria["distance"] + criteria["behavior"]);
					if (score > highestScore)
						highestScore = score;
					
					// Check if there is a picture for this behavior of this creature
					if ((DataHolder.creatureScores[creature].ContainsKey(criteria["behavior"]))){
						// If the new score is higher, change it
						if (DataHolder.creatureScores[creature][criteria["behavior"]] < score)
							DataHolder.creatureScores[creature][criteria["behavior"]] = score;
					}
					else{
						DataHolder.creatureScores[creature].Add(criteria["behavior"], score);
					}
					i++;
				}
			}
			picScores.Add(highestScore);
			picDescriptions.Add(GetDescription(p, highestScore));
			j++;
			
		}
		
		if (newCreaturePopup){
			PopupGUI pop = this.gameObject.AddComponent<PopupGUI>();	
			pop.message = "You discovered "+ newCreatures + " new creatures!";
			pop.button = true;
		}
	}
	
	protected override void DrawGUI (){
		//Previews
		int index = 0;
		GUILayout.BeginHorizontal();
		GUILayout.BeginArea(previewRect);
		previewScroll = GUILayout.BeginScrollView(previewScroll, GUILayout.Width(1000), GUILayout.Height(200));
		GUILayout.BeginHorizontal();
		foreach (Texture2D pic in pics){
			if (GUILayout.Button(pic, buttonStyle, GUILayout.Width(300), GUILayout.Height(170))){
				StartCoroutine(ToIndex(index));
			}
			index++;
		}		
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		GUILayout.BeginArea(buttonsRect);
		GUILayout.BeginVertical();
		GUI.enabled = false;
		GUILayout.Button("Download", buttonStyle, GUILayout.Width(150), GUILayout.Height(40));
		GUI.enabled = true;
		GUILayout.Label("Set as Field Guide Picture:", labelStyle);
		if (currentCreatures != null && currentCreatures.Count > 0){
			foreach (string s in currentCreatures){
				GUILayout.BeginHorizontal();
				if (GUILayout.Button(DataHolder.creatureInfo[s]["name"].text, buttonStyle, GUILayout.Width(200), GUILayout.Height(40))){
					SetAsProfile(s);
					if (!creaturePicSet.ContainsKey(s)){
						creaturePicSet.Add(s, currentIndex);
					}
					else{
						creaturePicSet[s] = currentIndex;
					}
				}
				if (creaturePicSet.ContainsKey(s)){
					if (creaturePicSet[s] == currentIndex){
						GUILayout.Label(checkbox, GUILayout.Height(40), GUILayout.Width(40));
					}
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
		GUILayout.EndHorizontal();
		
		//Leave Button
		if (GUI.Button(new Rect(targetWidth/2f - 200, 900, 400, 100),"Done")){
			StartCoroutine(TransitionGUI.SwitchLevel("spaceship"));	
		}
	}
	
	void SetAsProfile(string creature){
		DataHolder.creatureInfo[creature]["image"] = new GUIContent(pics[currentIndex]);	
	}
	
	IEnumerator ToIndex(int index){
		if (index != currentIndex && !lockPicture){
			BigPicture newPic = this.gameObject.AddComponent<BigPicture>();
			newPic.picture = pics[index];
			newPic.scoreText = picDescriptions[index];
			newPic.setLocalBounds(pictureRect);
			
			UpdateButtons(data[index]);
			
			float actualDistance = Mathf.Clamp(300*index - 350, 0, 300*pics.Count);
			float scrollDistance = actualDistance * 1000f / (310f*(pics.Count));
			oldScrollPosition = previewScroll;
			newScrollPosition = new Vector2(actualDistance,0);

			StartCoroutine(Scroll(scrollTime));
			
			if (index > currentIndex){
				StartCoroutine(newPic.MoveLeft (newPic.size, .6f));
				lockPicture = true;
				currentIndex = index;
				yield return StartCoroutine(currentPic.MoveLeft (currentPic.size, .6f));
				Destroy(currentPic);
				currentPic = newPic;
				lockPicture = false;
			}
			else{
				StartCoroutine(newPic.MoveRight (newPic.size, .6f));
				lockPicture = true;
				currentIndex = index;
				yield return StartCoroutine(currentPic.MoveRight (currentPic.size, .6f));
				Destroy(currentPic);
				currentPic = newPic;
				lockPicture = false;
			}			
			
		}
	}
	
	void UpdateButtons(PictureData pic){
		currentCreatures.Clear();
		foreach (string s in pic.namesOfCreatures){
			if (s!=null)
				currentCreatures.Add(s);
		}
	}
	
	IEnumerator Scroll(float time){
		Timer scrollTimer = new Timer(time);
		while (!scrollTimer.IsFinished()){
			previewScroll = Vector2.Lerp(oldScrollPosition, newScrollPosition, Mathf.Sin(Mathf.PI*scrollTimer.Percent()/2f));
			yield return 0;
		}
	}
	
	string GetDescription(PictureData p, float score){
		List<string> goodDescriptions = new List<string>();
		goodDescriptions.Add("This picture is spot-on!");
		goodDescriptions.Add("This picture is very clear! Feels like I’m right there in the action!");
		goodDescriptions.Add("Excellent photo, very well positioned.");

		List<string> badDescriptions = new List<string>();
		badDescriptions.Add("I can’t see the creature very well in this one.");
		badDescriptions.Add("I don't think this will work well for us.");
		badDescriptions.Add("Try to get a better position.");
		
		List<string> noCreature = new List<string>();
		noCreature.Add("Pretty background, but there’s no creature here!");
		noCreature.Add("The scenery is nice, but we really need pictures of creatures.");
		noCreature.Add("There's no creature in this picture!");
		
		string descript;
		
		if (p.namesOfCreatures.Count == 0)
			descript = noCreature.ToArray()[(int)(2*Random.value)];
		else if (score > 50){
			descript = goodDescriptions.ToArray()[(int)(2*Random.value)];	
		}
		else
			descript = badDescriptions.ToArray()[(int)(2*Random.value)];
		return descript;	
	}
}

internal class BigPicture : GUIScreen {
	
	public Texture2D picture;
	public string scoreText;
	public Rect size = new Rect(0,0,1000,600);
	public Rect area;
	public GUIStyle scoreStyle;
	
	public void setLocalBounds(Rect area){
		this.localBounds = area;
	}
	
	void Start(){
		scoreStyle = new GUIStyle();
		scoreStyle.fontSize = 30;
		scoreStyle.alignment = TextAnchor.MiddleCenter;
		scoreStyle.normal.textColor = Color.white;
	}

	protected override void DrawGUI (){
		GUI.DrawTexture(size, picture);
		GUI.Label(new Rect(0, 550, 1000, 50),scoreText,scoreStyle);
	}
}