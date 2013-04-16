using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarGUI : GUIScreen {
	
	int width = 300, height = 300;
	Texture2D radarScreen, radarDot, radarCircle, radarCenter;
	Collider radarDetector;
	Transform center, centerA, centerB;
	Dictionary<Transform, float> detectables;
	List<KeyValuePair<Transform, float>> addQueue;
	List<Transform> removeQueue;
	Timer radarTimer;
	float radarTime = 2f;
	float percent = 0;
	float angle = 0;
	float radarScale = .5f;
	bool toggle = true;
	bool looping = false;
	
	void Start(){
		localBounds = new Rect(targetWidth-width-30, targetHeight-height-30, width,height);
		StartCoroutine(FadeIn());
		addQueue = new List<KeyValuePair<Transform, float>>();
		removeQueue = new List<Transform>();
		detectables = new Dictionary<Transform, float>();
		center = GameObject.FindGameObjectWithTag("MainCamera").transform;
		centerA = center;
		centerB = center.GetChild(0);
		
		radarTimer = new Timer(radarTime, true);
		radarTimer.Repeat();
		
		radarCenter = new Texture2D(1,1);
		radarCenter.SetPixel(0,0,new Color(1,1,1,1f));
		radarCenter.Apply();
		
		radarScreen = Resources.Load("radarscreen") as Texture2D;
		radarDot = Resources.Load("radardot") as Texture2D;
		radarCircle = Resources.Load("radarcircle") as Texture2D;
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "CreatureCore")
			addQueue.Add(new KeyValuePair<Transform, float>(other.transform, 0f));
	}
	
	void OnTriggerExit(Collider other){
		if (detectables.ContainsKey(other.transform))
			removeQueue.Remove(other.transform);
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.Q)){
			if (toggle){
				center = centerB;
				toggle = false;
			}
			else{
				center = centerA;
				toggle = true;	
			}
		}
	}
	
	protected override void DrawGUI (){
		GUI.DrawTexture(new Rect(0,0,width,height), radarScreen);
		percent = radarTimer.Percent();
		GUI.color = new Color(1,1,1,1-percent);
		GUI.DrawTexture(new Rect(150*(1-percent),150*(1-percent),300*percent,300*percent),radarCircle);
		angle = RadarAngle();
		GUI.color = Color.white;
		GUI.DrawTexture(new Rect(143, 143, 14, 14), radarCenter);
 		foreach (Transform t in detectables.Keys){
			Vector2 convertedDistance = GetRadarPosition(t);
			if (convertedDistance.magnitude < 150){
				convertedDistance = rotate(convertedDistance, angle);
				GUI.color = RadarBlink(t, convertedDistance);
				GUI.DrawTexture(new Rect((143 + convertedDistance.x), (143 - convertedDistance.y), 15, 15), radarDot);
			}
		}
		foreach (KeyValuePair<Transform, float> pair in addQueue){
			detectables.Add(pair.Key, pair.Value);
			addQueue.Remove(pair);
		}
		foreach (Transform k in removeQueue){
			removeQueue.Remove(k);
			detectables.Remove(k);
		}
	}
	
	Vector3 GetRadarPosition(Transform t){
		return radarScale*new Vector2(t.position.x-center.position.x, t.position.z - center.position.z);
	}
	
	float RadarAngle(){
		Vector2 standard = new Vector2(0,1);
		Vector2 vec = new Vector2(center.forward.x, center.forward.z);
		float angle = Vector2.Angle(standard, vec)*Mathf.PI/180f;
		angle *= -Mathf.Sign(standard.x * vec.y - standard.y * vec.x);
		return angle;
	}
	
	Color RadarBlink(Transform t, Vector2 vec){
		detectables[t] -= Time.deltaTime/radarTime;
		if (Mathf.Pow(vec.magnitude - 150*percent, 2) < 25){
			detectables[t] = 1;
		}
		return new Color(1,1,1,detectables[t]);
	}
	
	Vector3 rotate(Vector2 vec, float angle){
		float x = vec.x * Mathf.Cos(angle) - vec.y * Mathf.Sin(angle);
		float y = vec.x * Mathf.Sin(angle) + vec.y * Mathf.Cos(angle);
		return new Vector2(x,y);
	}
}
