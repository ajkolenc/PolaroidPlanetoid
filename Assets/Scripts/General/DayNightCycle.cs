using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DayNightCycle : MonoBehaviour {

	public Timer restartScene;
	public enum TimeScale {SECONDS, MINUTES, HOURS};
	public TimeScale scale = TimeScale.MINUTES;
	public int timeToRestart = 5;
	
	public Color dayAmbience, nightAmbience;
	public Light sun, moon;
	Quaternion start, end;
	public Material skyBox;
	public Color dayColor, nightColor;
	
	bool lateNotified = false;
	
	void Start () {
		start = sun.transform.rotation;
		end = Quaternion.Euler(Vector3.zero);
		switch (scale){
		case TimeScale.MINUTES:
			timeToRestart *= 60;
			break;
		case TimeScale.HOURS:
			timeToRestart *= 3600;
			break;
		}
		restartScene = new Timer(timeToRestart, true);
		skyBox.color = dayColor;
	}
	
	public static void ToReview(){
		PictureTaking picture = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<PictureTaking>();
		DataHolder.AddPictures(picture.textures, picture.pictures);
		picture.StartCoroutine(TransitionGUI.SwitchLevel("review"));		
	}
	
	void Update(){
		if (restartScene.IsFinished()){
			DayNightCycle.ToReview();
			this.enabled = false;
		}
		TimeNotifications();
		TransitionSky();
		ReduceVisibility();
		MoveSun();
	}
	
	void TimeNotifications(){
		if (restartScene.Percent() > .9f && !lateNotified){
			gameObject.AddComponent<LeaveToShipGUI>();
			StartCoroutine(Notification.Notify("It's getting late, you better wrap up soon!", false, 4f));
			lateNotified = true;
		}
	}
	
	void ReduceVisibility(){
		RenderSettings.ambientLight = Color.Lerp(dayAmbience, nightAmbience, restartScene.Percent());
	}
	
	void MoveSun(){
		sun.transform.rotation = Quaternion.Lerp(start, end, restartScene.Percent());
		sun.intensity = 0.5f * (1 - restartScene.Percent());
	}
	
	void TransitionSky () {
		skyBox.SetColor("_Tint", Color.Lerp(dayColor, nightColor, restartScene.Percent()));
		skyBox.SetFloat("_Blend", restartScene.Percent());
	}
}
