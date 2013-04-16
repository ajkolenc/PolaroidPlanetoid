using UnityEngine;
using System.Collections;
using UnityEditor;

public class ForceUpdate {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public static void Force () {
		#if UNITY_EDITOR
	      AssetDatabase.Refresh();
	    #endif
	}
}
