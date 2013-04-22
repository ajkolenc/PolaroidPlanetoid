using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndingScriptManager : MonoBehaviour {
	
	GameObject camera, ship;
	Notification n;
	float pauseTime;
	public GUISkin skin;
	
	void Start () {
		camera = GameObject.Find("Main Camera");
		ship = GameObject.Find("Spaceship");
		pauseTime = 1.5f;
		StartCoroutine(StartSpeaking());
	}
	
	IEnumerator StartSpeaking(){
		yield return new WaitForSeconds(1f);
		n = gameObject.AddComponent<Notification>();
		n.skin = skin;
		n.typing = true;
		n.isSal = false;
		yield return StartCoroutine(n.FadeIn(.5f));
		yield return StartCoroutine(SpeakLine("Well, you've taken some great pics, sport. It's up to you if you want to stay on this planet or move on. So what'll it be?"));
		gameObject.AddComponent<EndChoiceGUI>();
	}
	
	IEnumerator SpeakLine(string line){
		n.content = line;
		n.displayedContent = "";
		yield return StartCoroutine(n.TypeInContent());
		yield return new WaitForSeconds(pauseTime);
	}	
}
