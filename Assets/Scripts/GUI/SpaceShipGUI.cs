using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceShipGUI : GUIScreen {

	public GUISkin skin;
 	int experience, gainedExperience, nextLevelExperience, promotedLevelExperience;
	string currentRank, newRank;
	float gridScale;
	bool animateExperience = true;
	Timer experienceTimer;
	Texture2D blackTex;
	string[] items;
	Vector2 itemScroll = Vector2.zero;
	Rect experienceRect;
	
	void Start () {
		FadeIn();
		blackTex = new Texture2D(1,1);
		blackTex.SetPixel(0,0,Color.black);
		blackTex.Apply();
		items = new string[]{"None"};
		experience = (int) DataHolder.GetExperience();
		currentRank = DataHolder.currentRank;
		List<string> newItems = GetPowerUps(currentRank);
		if (newItems.Count > 0)
			items = newItems.ToArray();
		newRank = NextRank(currentRank);
		nextLevelExperience = GetExperience(NextRank(currentRank));
		promotedLevelExperience = nextLevelExperience;
		if (experience > nextLevelExperience){
			gridScale = 600/(experience*1.5f);
		}
		else{
			gridScale = 600/(nextLevelExperience*1.5f);			
		}
		
		skin.button.fontSize = 50;
		skin.label.fontSize = 50;
		skin.label.alignment = TextAnchor.MiddleCenter;
		
		StartCoroutine(ExperienceAnimation());
		Persist(true);
		Screen.showCursor = true;
		Screen.lockCursor = false;
		StartCoroutine(FadeIn(.6f));
		
		if (DataHolder.tutorial){
			StartCoroutine(SalTutorial());
			DimLightsGUI.LightsOn(false, 11);
		}
		else{
			Notification n = gameObject.AddComponent<Notification>();
			n.isSal = true;
			n.inSpaceShip = true;
			n.content = "Welcome back, Captain.";
			n.comeIn = true;
			n.skin = this.skin;
		}
	}
	
	IEnumerator SalTutorial(){
		Notification n = gameObject.AddComponent<Notification>();
		n.bigNotification = true;
		n.isSal = true;
		n.skin = skin;
		yield return StartCoroutine(n.FadeIn(.5f));
		yield return StartCoroutine(SpeakLine("Greetings Captain. Technically I am referred to as Small Astro-trans Lightship I. You may also call me Sal.", n));
		yield return StartCoroutine(SpeakLine("We have landed on the planet Notterra. Your compass, located in the bottom right-hand corner of your display, points to the nearest creature.", n));
		yield return StartCoroutine(SpeakLine("You may also access your Field Guide by pressing the TAB key to aid in your exploration of this world.", n));
		yield return StartCoroutine(SpeakLine("Notterra is dangerous at night; to ensure your safety, we will teleport you back to the ship when it gets dark. When you return I will appear...", n));
		DimLightsGUI.LightsOn(true);
		yield return StartCoroutine(n.Close());
		n = gameObject.AddComponent<Notification>();
		n.inSpaceShip = true;
		n.content = "Over here. Should you encounter any troubles navigating the rough alien terrain, please press the ESC key for further information.";
		n.comeIn = true;
		n.skin = this.skin;
	}
	
	IEnumerator SpeakLine(string line, Notification n){
		n.content = line;
		n.displayedContent = "";
		yield return StartCoroutine(n.TypeInContent());
		yield return new WaitForSeconds(1f);
	}	
	
	IEnumerator ExperienceAnimation(){	
		animateExperience = true;
		Timer experienceTimer = new Timer(3f);
		while (!experienceTimer.IsFinished()){
			experienceRect = new Rect(100, (600 - experience*gridScale) + experience*gridScale*(1-experienceTimer.Percent()), 300, experience*gridScale*experienceTimer.Percent());
			yield return 0;
		}

		if (experience > nextLevelExperience){
			LevelUp();
		}

		animateExperience = false;
	}
	
	string GetRank(int score){
		string rank = "Intern";
		if (score > 1100)
			rank = "Junior Photographer";
		if (score > 4400)
			rank = "Full-Time Photographer";
		if (score > 6500)
			rank = "Photojournalist";
		return rank;
	}
	
	int GetExperience(string rank){
		int experience = int.MaxValue;
		switch (rank){
		case "Junior Photographer":	
			experience = 1100;
			break;
		case "Full-Time Photographer":
			experience = 4400;
			break;
		case "Photojournalist":
			experience = 6500;
			break;
		default:
			experience = this.experience;
			break;
		}
		return experience;
	}
	
	public void LoadData(){
		
	}
	
	protected override void DrawGUI (){
		GUI.skin = skin;
		RankPanel();
		ExperiencePanel();
		PowerUpPanel();
		
		if (GUI.Button(new Rect(targetWidth/2f + 15, 800, 500, 200), "Leave Ship")){
			StartCoroutine(TransitionGUI.SwitchLevel("SceneTest-With Creatures"));
		}
	}
	
	void RankPanel(){
		GUI.BeginGroup(new Rect(targetWidth/2f - 515, 80, 430, 300));
		GUI.Box(new Rect(0, 0, 430, 200), "");
		GUI.Label(new Rect(15, 15, 400, 170), currentRank);
		skin.label.wordWrap = true;
		skin.label.fontSize = 30;
//		GUI.Label(new Rect(15,150,300,50),"Next: " + newRank);
		skin.label.fontSize = 50;
		GUI.EndGroup();
	}
	
	void ExperiencePanel(){
		GUI.BeginGroup(new Rect(targetWidth/2f - 515, 300, 500, 700));
		GUI.Box(new Rect(0, 0, 500, 700), "");
		GUI.Label(new Rect(15, 600, 470, 100),"Reputation");
		
		// Graph Area
		GUI.Box(new Rect(50,50,400,550),"");
		
		// Experience Bar
		if (animateExperience){
			GUI.Box(experienceRect,"");
		}
		else{
			GUI.Box(new Rect(100, 600 - experience*gridScale, 300, experience*gridScale),"");
		}
		
		// Requirement Line
		GUI.DrawTexture(new Rect(50, 600 - nextLevelExperience*gridScale, 400, 3), blackTex);
		
		
		// Secondary Requirement Line
		GUI.DrawTexture(new Rect(50, 600 - promotedLevelExperience*gridScale, 400, 3), blackTex);
		
		GUI.EndGroup();
	}
	
	string getCyMessage(string rank){
		string message = "";
	 	switch (rank){
		case "Photojournalist":
			message = "I'm speechless! You're in a league of your own now, sport! I bet you'll put this jetpack to good use.";
			break;
		case "Full-Time Photographer":
			message = "I'm impressed, you're really getting good at this! I managed to snag you these water shoes, they'll let you walk on water. Keep it up!";
			break;
		case "Junior Photographer":
			message = "Wow, a junior photographer already; you'll go far, kid. Here, take this flashlight, it'll help you take better pictures at night.";
			break;
		}
		return message;
	}
	
	void LevelUp(){
		currentRank = GetRank(experience);
		DataHolder.currentRank = currentRank;
		DataHolder.powerUps = GetPowerUps(currentRank);
		items = DataHolder.powerUps.ToArray();
		newRank = NextRank(newRank);
		promotedLevelExperience = GetExperience(newRank);
		StartCoroutine(PromotionMessage());
//		PopupGUI popup = this.gameObject.AddComponent<PopupGUI>();
//		popup.message = "You've been promoted to " +currentRank + "!";
//		popup.button = true;
//		popup.parent = this;
//		MoveDown(new Rect(0,0,targetWidth, targetHeight), .7f);
	}
	
	IEnumerator PromotionMessage(){
		Notification n = gameObject.AddComponent<Notification>();
		n.skin = this.skin;
		n.isSal = false;
		n.bigNotification = true;
		n.typing = true;
		n.Timeout(2f);
		DimLightsGUI.LightsOn(false, 11);
		yield return StartCoroutine(n.FadeIn());
		yield return StartCoroutine(SpeakLine(getCyMessage(currentRank),n));
		DimLightsGUI.LightsOn(true, 11);
		if (currentRank == "Photojournalist"){
			StartCoroutine(TransitionGUI.SwitchLevel("endscene"));
		}
		else
			StartCoroutine(n.Close());
	}
	
	List<string> GetPowerUps(string rank){
		List<string> powers = new List<string>();
		switch (rank){
		case "Photojournalist":
			powers.Add("Jetpack");
			goto case "Full-Time Photographer";
		case "Full-Time Photographer":
			powers.Add("Water Shoes");
			goto case "Junior Photographer";
		case "Junior Photographer":
			powers.Add("Flashlight");
			break;
		}
		return powers;
	}
	
	string NextRank(string rank){
		string nextRank = rank;
		switch (rank){
		case "Intern":
			nextRank = "Junior Photographer";
			break;
		case "Junior Photographer":
			nextRank = "Full-Time Photographer";
			break;
		case "Full-Time Photographer":
			nextRank = "Photojournalist";
			break;
		case "Photojournalist":
			nextRank = "None";
			break;
		}
		return nextRank;
	}
	
	void PowerUpPanel(){
		GUI.BeginGroup(new Rect(targetWidth/2f + 15, 300, 500, 500));
		GUI.Box(new Rect(0, 0, 500, 485), "");
		GUI.Label(new Rect(15, 15, 470, 60),"Items");
		
		// List Container
		GUI.Box(new Rect(50,90,400,350),"");
		GUILayout.BeginArea(new Rect(50,90,400,350));
		itemScroll = GUILayout.BeginScrollView(itemScroll, GUILayout.Width(400), GUILayout.Height(350));
		GUILayout.BeginVertical();
		foreach (string item in items){
			GUILayout.Label(item);
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		GUI.EndGroup();
	}
}
