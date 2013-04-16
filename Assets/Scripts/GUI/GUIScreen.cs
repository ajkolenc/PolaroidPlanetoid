using UnityEngine;
using System.Collections;

public abstract class GUIScreen : MonoBehaviour {
	
	bool fadeOut, fadeIn, scaling, wrapUp, wrapDown, moving;
	protected static float targetWidth = 1920;
	protected static float targetHeight = 1080;
	protected bool displayed = false;
	protected bool persist = false;
	protected static bool letterBoxing = false;
	float transitionTime = .3f;
	float alpha = 1f;
	public int depth = int.MaxValue-100;
	Matrix4x4 screenTransform;
	protected static Vector3 resolutionTransform;
	Vector3 scalingTransform, wrappingTransform, movingTransform, currentMove, moveGoal;
	Quaternion rotatingTransform;
	Rect bounds;
	protected static Rect screenBounds, letterBox1, letterBox2;
	protected static Texture2D letterBox;
	protected Rect localBounds = new Rect(0,0,targetWidth, targetHeight);
	protected Rect movingBounds = new Rect(0,0,targetWidth,targetHeight);
	Color guiColor;
	public GUISkin skin;

	protected virtual void Awake (){
		depth = int.MaxValue-100;
		guiColor = Color.white;
		scalingTransform = Vector3.one;
		wrappingTransform = Vector3.one;
		rotatingTransform = Quaternion.identity;
		screenTransform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, 1f, 1f));
		if (!letterBoxing){
			screenBounds = new Rect(0,0, GUIScreen.targetWidth, GUIScreen.targetHeight);
			GUIScreen.resolutionTransform = new Vector3(Screen.width/GUIScreen.targetWidth, Screen.height/GUIScreen.targetHeight, 1f);
		}
	}
	
	protected static void useLetterBox(bool yes){
		if (yes){
			GUIScreen.letterBox = new Texture2D(1,1);
			GUIScreen.letterBox.SetPixel(0,0, Color.black);
			GUIScreen.letterBox.Apply();
			float ratio = Screen.width/GUIScreen.targetWidth;
			bool width = true;
			if (ratio > Screen.height/GUIScreen.targetHeight){
				width = false;
				ratio = Screen.height/GUIScreen.targetHeight;
			}
			GUIScreen.screenBounds = new Rect((Screen.width/ratio - GUIScreen.targetWidth)/2f,(Screen.height/ratio - GUIScreen.targetHeight)/2f,GUIScreen.targetWidth, GUIScreen.targetHeight);
			if (!width){
				GUIScreen.letterBox1 = new Rect(0,0,(Screen.width - GUIScreen.targetWidth*ratio)/2f, Screen.height);
				GUIScreen.letterBox2 = new Rect((Screen.width - GUIScreen.targetWidth*ratio)/2f + targetWidth*ratio,0,(Screen.width - GUIScreen.targetWidth*ratio)/2f, Screen.height);
			}
			else{
				GUIScreen.letterBox1 = new Rect(0,0,Screen.width, (Screen.height - GUIScreen.targetHeight*ratio)/2f+1);
				GUIScreen.letterBox2 = new Rect(0,(Screen.height - GUIScreen.targetHeight*ratio)/2f + GUIScreen.targetHeight*ratio,Screen.width, (Screen.height - GUIScreen.targetHeight*ratio)/2f);
			}
			GUIScreen.resolutionTransform = new Vector3(ratio, ratio, 1f);
			GUIScreen.letterBoxing = true;
		}
		else{
			screenBounds = new Rect(0,0, GUIScreen.targetWidth, GUIScreen.targetHeight);
			resolutionTransform = new Vector3(Screen.width/targetWidth, Screen.height/targetHeight, 1f);
			GUIScreen.letterBoxing = false;			
		}
	}
			

	void OnGUI(){
		if (GUIScreen.letterBoxing){
			GUI.DrawTexture(GUIScreen.letterBox1, GUIScreen.letterBox);
			GUI.DrawTexture(GUIScreen.letterBox2, GUIScreen.letterBox);
		}
		if (displayed){
			GUI.skin = skin;
			GUI.depth = depth;
			GUI.matrix = screenTransform;
			GUI.color = guiColor;
			screenTransform = Matrix4x4.TRS(new Vector3(movingTransform.x * resolutionTransform.x, movingTransform.y * resolutionTransform.y, 1f), rotatingTransform, new Vector3(scalingTransform.x * wrappingTransform.x * resolutionTransform.x, scalingTransform.y * wrappingTransform.y * resolutionTransform.y, 1f));
			GUI.BeginGroup(GUIScreen.screenBounds);
			GUI.BeginGroup(localBounds);
			GUI.BeginGroup(movingBounds);
			DrawGUI();
			GUI.EndGroup();
			GUI.EndGroup();
			GUI.EndGroup();
		}
	}
	
	protected abstract void DrawGUI();
	
	public void Reset(){
		scalingTransform = Vector3.one;
		wrappingTransform = Vector3.one;
		movingTransform = Vector3.zero;
	}
	
	public void Persist(bool persist){
		this.persist = persist;
	}
	
	public IEnumerator FadeIn(float transitionTime = .3f){
		Timer transitionTimer = new Timer(transitionTime);
		displayed = true;
		while (!transitionTimer.IsFinished()){
			alpha = transitionTimer.Percent();
			guiColor = new Color(1f, 1f, 1f, alpha);
			yield return 0;
		}
		guiColor = new Color(1f, 1f, 1f, 1f);
	}

	public IEnumerator FadeOut(float transitionTime = .3f){
		Timer transitionTimer = new Timer(transitionTime);
		float alpha = 1f;
		while (!transitionTimer.IsFinished()){
			alpha = transitionTimer.Percent();
			guiColor = new Color(1f, 1f, 1f, 1f-alpha);
			yield return 0;
		}
		guiColor = new Color(1f, 1f, 1f, 0f);
	}
		
	public IEnumerator ScaleOut(float scaleTime = .3f){
		StartCoroutine(FadeOut (scaleTime));
		Timer transitionTimer = new Timer(scaleTime);
		while (!transitionTimer.IsFinished()){
			wrappingTransform = new Vector3(1f-(transitionTimer.Percent()/4f),1f - (transitionTimer.Percent()/4f), 1f);
			yield return 0;
		}
		displayed = false;
	}
	
	public IEnumerator ScaleIn(float scaleTime = .3f){
		StartCoroutine(FadeIn (scaleTime));
		displayed = true;
		Timer transitionTimer = new Timer(scaleTime);
		while (!transitionTimer.IsFinished()){
			wrappingTransform = new Vector3(.75f+(transitionTimer.Percent()/4f),.75f + (transitionTimer.Percent()/4f), 1f);
			yield return 0;
		}
		wrappingTransform = new Vector3(1f,1f,1f);
	}
	
	public IEnumerator WrapUp(Rect bounds, float wrapTime = .3f){
		Timer transitionTimer = new Timer(wrapTime);
		displayed = true;
		while (!transitionTimer.IsFinished()){
			wrappingTransform = new Vector3(1f, transitionTimer.Percent(), 1f);
			movingTransform = new Vector3(0f, (bounds.y+bounds.height/2f)*(1-transitionTimer.Percent()), 0f);
			yield return 0;
		}
		movingTransform = Vector3.zero;
		wrappingTransform = new Vector3(1f,1f,1f);
	}

	public IEnumerator WrapDown(Rect bounds, float wrapTime = .3f){
		Timer transitionTimer = new Timer(wrapTime);
		this.bounds = bounds;
		displayed = true;
		while (!transitionTimer.IsFinished()){
			wrappingTransform = new Vector3(1f, 1-transitionTimer.Percent(), 1f);
			movingTransform = new Vector3(0f, (bounds.y+bounds.height/2f)*(transitionTimer.Percent()), 0f);
			yield return 0;
		}
		displayed = false;
	}
	
	public IEnumerator MoveRight(Rect bounds, float moveTime = 1f){
		Vector3 currentMove = Vector3.zero;
		Vector3 moveGoal = Vector3.zero;
		
		if (!displayed){
			currentMove = new Vector3(-(bounds.x + bounds.width), 0,0);
		}
		else{
			moveGoal = new Vector3(localBounds.width-bounds.x, 0, 0);
		}
		
		yield return StartCoroutine(Move(bounds, currentMove, moveGoal, moveTime));
	}

	public IEnumerator MoveLeft(Rect bounds, float moveTime = .3f){
		Vector3 currentMove = Vector3.zero;
		Vector3 moveGoal = Vector3.zero;
		
		if (!displayed){
			currentMove = new Vector3((localBounds.width-bounds.x), 0,0);
		}
		else{
			moveGoal = new Vector3(-(bounds.x + bounds.width), 0, 0);
		}
		
		yield return StartCoroutine(Move(bounds, currentMove, moveGoal, moveTime));
	}
	
	public IEnumerator MoveUp(Rect bounds, float moveTime = 1f){
		Vector3 currentMove = Vector3.zero;
		Vector3 moveGoal = Vector3.zero;
		
		if (!displayed){
			currentMove = new Vector3(0, localBounds.height-bounds.height,0);
		}
		else{
			moveGoal = new Vector3(0, -(bounds.y + bounds.height), 0);
		}

		yield return StartCoroutine(Move(bounds, currentMove, moveGoal, moveTime));
	}	
	
	public IEnumerator MoveDown(Rect bounds, float moveTime = 1f){
		Vector3 currentMove = Vector3.zero;
		Vector3 moveGoal = Vector3.zero;
		
		if (!displayed){
			currentMove = new Vector3(0, -(bounds.y + bounds.height),0);
		}
		else{
			moveGoal = new Vector3(0, localBounds.height-bounds.height, 0);
		}
		yield return StartCoroutine(Move(bounds, currentMove, moveGoal, moveTime));
	}
		
	public IEnumerator Move(Rect bounds, Vector3 startPosition, Vector3 endPosition, float moveTime = 1f){
		Timer transitionTimer = new Timer(moveTime);			
		bool moveIn = true;
		
		if (!displayed){
			displayed = true;	
		}
		else{
			moveIn = false;	
		}

		float translationAmount, translationX, translationY;
		while (!transitionTimer.IsFinished()){
			translationAmount = Mathf.Sin(Mathf.PI*transitionTimer.Percent()/2f);
			movingBounds.x = Mathf.Lerp(startPosition.x, endPosition.x, translationAmount); 
			movingBounds.y = Mathf.Lerp(startPosition.y, endPosition.y, translationAmount);
			yield return 0;
		}
		movingBounds.Set(moveGoal.x, moveGoal.y, movingBounds.width, movingBounds.height);		
		if (!moveIn){
			if (!persist)
				displayed = false;	
		}
	}
}