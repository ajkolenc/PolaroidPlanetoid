using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroScriptManager : MonoBehaviour {
	
	GameObject camera, ship;
	Notification n;
	float pauseTime;
	public GUISkin skin;
	
	void Start () {
		camera = GameObject.Find("Main Camera");
		ship = GameObject.Find("Spaceship");
		pauseTime = 3f;
		StartCoroutine(StartSpeaking());
	}
	
	IEnumerator StartSpeaking(){
		yield return new WaitForSeconds(1f);
		n = gameObject.AddComponent<Notification>();
		n.skin = skin;
		yield return StartCoroutine(n.FadeIn(.5f));
		yield return StartCoroutine(SpeakLine("Welcome to Noterra! Cy Borg’s the name, picture taking’s the game. You can call me Cy."));
		yield return StartCoroutine(SpeakLine("Let’s get straight to the good stuff, sport! Here’s a brand-new camera. Go out and explore and use it to take pictures of the creatures you see."));
		yield return StartCoroutine(SpeakLine("You’ll earn reputation for the good pictures you take. If you managed to get promoted, I’ll get you some great items for your camera. What do I mean by “a good picture”? Well, that’s up to you to find out!"));
		StartCoroutine(SpeakLine("Just warning you, I’m one picky robot! Now, get out there and make me proud! See you at night-time. Good luck!"));
		yield return new WaitForSeconds(3f);
		ship.animation.Play("FlyToPlanet");
		yield return new WaitForSeconds(6f);
		yield return new WaitForSeconds(2f);
		StartCoroutine(n.Close());
		camera.animation.Play("planetzoom");
		StartCoroutine(TransitionGUI.SwitchLevel("spaceship", 2f));
	}
	
	IEnumerator SpeakLine(string line){
		n.content = line;
		n.displayedContent = "";
		yield return StartCoroutine(n.TypeInContent());
		yield return new WaitForSeconds(pauseTime);
	}	
}
