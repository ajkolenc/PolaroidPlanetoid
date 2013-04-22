using UnityEngine;
using System.Collections;

public class ClipTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI(){
		GUI.BeginGroup(new Rect(Screen.width/2f - 100,Screen.height/2f - 100,200,200));
		GUI.Box(new Rect(-200,-200,400,400),"");
		GUI.EndGroup();
	}
}
