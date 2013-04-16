using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DataHolder : MonoBehaviour {
	public static List<KeyValuePair<Texture2D, PictureData>> pictures;
	public static Dictionary<string, Dictionary<float, float>> creatureScores;
	public static int currentScore, totalScore;
	public static bool created;
	public static string currentRank = "Intern";
	public static List<string> powerUps, previousPowerUps;
	public static Dictionary<string, Dictionary<string, GUIContent>> creatureInfo;
	public static bool tutorial = true;
		
	void Awake(){
		if (created)
			Destroy(this);
		else{
			PlayerPrefs.DeleteAll();
			DataHolder.created = true;
			DataHolder.powerUps = new List<string>();
			DataHolder.previousPowerUps = new List<string>();
			//print ("Made Data");
			DataHolder.pictures = new List<KeyValuePair<Texture2D, PictureData>>();
			DataHolder.creatureInfo = new Dictionary<string, Dictionary<string, GUIContent>>();
			DataHolder.creatureScores = new Dictionary<string, Dictionary<float, float>>();
			DataHolder.creatureScores.Add("Bonus", new Dictionary<float, float>());
			DataHolder.creatureScores["Bonus"].Add(0,0);
			InitializeCreatureInfo();
			DataHolder.totalScore = 0;
			DataHolder.currentScore = 0;
			DontDestroyOnLoad(this.gameObject);
		}
	}
	
	public static void AddPictures(ArrayList textures, PictureData[] data){
		DataHolder.pictures.Clear();
		int i = 0;
		if (textures != null){
			foreach(System.Object o in textures){
				DataHolder.pictures.Add(new KeyValuePair<Texture2D, PictureData>(o as Texture2D, data[i]));
				i++;
			}
		}
	}
	
	public static Dictionary<string, GUIContent> GetCreatureData(string creatureName){
		return creatureInfo[creatureName];
	}
	
	public static float GetExperience(){
		float score = 0;
		
		foreach (KeyValuePair<string, Dictionary<float, float>> pair in creatureScores){
			foreach (KeyValuePair<float,float> f in pair.Value){
				score += f.Value;
			}
		}
		print (score);
		return score;
	}
	
	public void InitializeCreatureInfo(){
		Dictionary<string, GUIContent> creature = new Dictionary<string, GUIContent>();
		
		// Rabbit Creature
		creature.Add("name",new GUIContent("Hoppalot"));
		creature.Add("description", new GUIContent("A cute, almost ordinary creature. Found everywhere, but that doesn’t detract from its charm. Maybe interacting with it will help us to learn more about it?"));
		creature.Add("features",new GUIContent("Round, pink, enormous ears, long tail"));
		creature.Add("notes",new GUIContent("Hmm, not very friendly. Looks like the only way to its heart (if it has one?) is through its stomach (has to have one, right?)"));
		creature.Add("image", new GUIContent(Resources.Load("unknown") as Texture2D));
		creatureInfo.Add("CreatureController",creature);

		creature = new Dictionary<string, GUIContent>();
		
		// Likes Other Creatures Creature
		creature.Add("name", new GUIContent("Sprizzer"));
		creature.Add("description", new GUIContent("Probably easiest to spot near the rabbit-types it’s so fond of."));
		creature.Add("features", new GUIContent("Round, blue. Round eyes and appendages."));
		creature.Add("notes", new GUIContent("Might be easiest to lure his friend and hope he follows."));
		creature.Add("image", new GUIContent(Resources.Load("unknown") as Texture2D));
		creatureInfo.Add("FriendlyCreatureController",creature);

		creature = new Dictionary<string, GUIContent>();
		
		// Watermelon Bear Creature
		creature.Add("name", new GUIContent("Aminal"));
		creature.Add("description", new GUIContent("An oddly friendly creature, once you are near, it will find you."));
		creature.Add("features", new GUIContent("Short, green and black. Cushion-like with stubby legs."));
		creature.Add("notes", new GUIContent("Always underfoot! Guess the odd perspective adds interest?\n It sure likes you! What did you do?\n Watch out or it might get carried away and carry you away!"));
		creature.Add("image", new GUIContent(Resources.Load("unknown") as Texture2D));
		creatureInfo.Add("LovesPlayerCreatureController",creature);

		creature = new Dictionary<string, GUIContent>();

		// Elephandillo Creature
		creature.Add("name", new GUIContent("Muzmuz"));
		creature.Add("description", new GUIContent("Seemingly docile, huge water creature. But that doesn’t make for such an interesting scene. How might you get it moving?"));
		creature.Add("features", new GUIContent("Huge, tall, red. Trunk-like legs and bulbous snout."));
		creature.Add("notes", new GUIContent("Perhaps climbing to higher ground would provide a better vantage point?"));
		creature.Add("image", new GUIContent(Resources.Load("unknown") as Texture2D));
		creatureInfo.Add("WhaleController",creature);

		creature = new Dictionary<string, GUIContent>();

		// Flyer Creature
		creature.Add("name", new GUIContent("Notterfly"));
		creature.Add("description", new GUIContent("These flying creatures may be easier to find if you try to scale their large, water-dwelling friend."));
		creature.Add("features", new GUIContent("Round, orange. Floppy wings/ears and bright eyes."));
		creature.Add("notes", new GUIContent("Going to be hard to get a shot of this one alone, it seems strangely fond of the big creature."));
		creature.Add("image", new GUIContent(Resources.Load("unknown") as Texture2D));
		creatureInfo.Add("RidingCreatureController",creature);

		creature = new Dictionary<string, GUIContent>();

		// Bull Creature
		creature.Add("name", new GUIContent("Ramatam"));
		creature.Add("description", new GUIContent("This beast seems to enjoy bouncing you backwards... Looks fun, but watch yourself, we can’t have our photographer getting injured on the job."));
		creature.Add("features", new GUIContent("Quadrupedal, red. Horns and flat snout."));
		creature.Add("notes", new GUIContent("A real workout to chase this one down! All the effort paid off though for an action-packed shot!"));
		creature.Add("image", new GUIContent(Resources.Load("unknown") as Texture2D));
		creatureInfo.Add("BullGuyController",creature);

		creature = new Dictionary<string, GUIContent>();
		
		// Dragon Creature
		creature.Add("name", new GUIContent("Nottesaur"));
		creature.Add("description", new GUIContent("Giant slumbering beast found high in the hills and peaks. May not even notice it, apart from the motion of its breathing. How mysterious..."));
		creature.Add("features", new GUIContent("Enormous, long, obsidian and beige. Stone-like exterior."));
		creature.Add("notes", new GUIContent("..."));
		creature.Add("image", new GUIContent(Resources.Load("unknown") as Texture2D));
		creatureInfo.Add("BasicCreature",creature);
	}
}
