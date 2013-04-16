using UnityEngine;
using System.Collections;

public class DimLightsGUI : GUIScreen {

	static DimLightsGUI dimLights;
	Texture2D mask;
	
	protected override void Awake () {
		base.Awake();
		mask = new Texture2D(1,1);
		mask.SetPixel(0,0,new Color(0,0,0,.5f));
		mask.Apply();
		DimLightsGUI.dimLights = this;
	}
	
	protected override void DrawGUI (){
		GUI.DrawTexture(new Rect(0,0,targetWidth, targetHeight), mask);
	}
	
	public static void LightsOn(bool on, int depth = 0){
		if (DimLightsGUI.dimLights == null){
			GameObject.Find("GUIHandler").AddComponent<DimLightsGUI>();	
		}
		DimLightsGUI.dimLights.depth = depth;
		if (on){
			DimLightsGUI.dimLights.StartCoroutine(DimLightsGUI.dimLights.FadeOut());	
		}
		else{
			DimLightsGUI.dimLights.StartCoroutine(DimLightsGUI.dimLights.FadeIn());			
		}
	}
}
