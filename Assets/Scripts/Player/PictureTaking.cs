using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class PictureTaking : MonoBehaviour {
	//Whether or not the gui used to look at pictures is on
	//Whether or not we should be taking a shot
	private bool guiOn;
	private bool pictureTaken = false;
	//The index we're presently looking at for the gui
	int indexLookingAt;
	
	//Actual photo images
	public ArrayList textures;
	//Meta data for pictures
	public PictureData[] pictures;
	//Current count and the maximum number
	private int count = 0, maxNumberOfShots = 100;
	
	
	//Sound for camera to make
	public AudioClip cameraNoise;
	//The middle of the camera
	public GUITexture cameraMark;
	
	
	
	
	void Start(){
		
		textures = new ArrayList();
		
		//Instantiate our list of the number of pictures that can be taken (100)
		pictures = new PictureData[maxNumberOfShots];
		
	}
	
	void Update(){
		
		
		//array = textures.ToArray() as Texture2D[];
		
		
		Vector3 cameraEnd = transform.position;
		cameraEnd+=transform.forward*100;
		
		//Debug.DrawLine(transform.position,cameraEnd);
		
		 RaycastHit hit;
		//Determine color of indicator, 100.0f is our range for now
		//+ transform.up
		//Debug.DrawLine(transform.position , (transform.forward*(200)+transform.position+transform.up));
		if(Physics.SphereCast(transform.position,1.0f,transform.forward, out hit,200.0f)){
			//print("I sphere raycasted at: "+hit.transform.name);
			
			if(hit.collider.tag=="Creature"){
				cameraMark.color= Color.green;
			
			}
			else{
				cameraMark.color = Color.white;
			}
		}
		else{
			cameraMark.color = Color.white;
		}
		
		
		
		if(!guiOn){
			float zoomAmnt = Input.GetAxis("CamVertical");
			
			if(zoomAmnt!=0 && camera.fieldOfView-Time.deltaTime*zoomAmnt*5.0f>45 && camera.fieldOfView-Time.deltaTime*zoomAmnt*5.0f<100){
				camera.fieldOfView-=Time.deltaTime*zoomAmnt*5.0f;
			}
			
			
			if(Input.GetMouseButtonDown(0) && count<maxNumberOfShots){
				//Figure out the score
				RaycastHit[] hits = Physics.SphereCastAll(transform.position, 10.0f, transform.forward, 100.0f);
				audio.PlayOneShot(cameraNoise);
				CameraGUI.Snap();
				//int scoreForDis=determineScore(hits);
				PictureData p = getData(hits);
				pictures[count] = p;
				//scores[count] = scoreForDis;
				
				// Picture will be stored OnPostRender
				pictureTaken = true;
				
				//StartCoroutine(ScreenshotEncode());
			}
			
		
		
		}
		else{
			//If gui is on
			if(Input.GetKeyDown(KeyCode.A)){
				if(indexLookingAt>0){
					indexLookingAt--;
				}
			}
			else if(Input.GetKeyDown(KeyCode.D)){
				if(indexLookingAt<textures.Count-1){
					indexLookingAt++;
				}
			}
		}
		
		if(Input.GetKeyDown(KeyCode.E)){
			guiOn=!guiOn;
			indexLookingAt=textures.Count-1;
			
		}
	}
	
	//Returns PictureData based on the picture expressed as RaycastHits
	PictureData getData(RaycastHit[] hits){
		float scoreToReturn =0;
		Vector2 middleOfScreen = new Vector2(Screen.width/2,Screen.height/2);
		List<BasicCreature> creaturesInPic = new List<BasicCreature>();
		List<Dictionary<string, float>> criteria = new List<Dictionary<string, float>>();
		
		bool theresANewCreatureInThere=false;
		
		foreach(RaycastHit hitt in hits){			
			
			Debug.Log("Hit name: "+hitt.transform.name);
			if(hitt.collider.tag=="Creature"){
				
				
				//Got up to the tippity top
				GameObject creatah = hitt.transform.gameObject;
				int levelsUp = 0;
				if(creatah!=null ){
					while(creatah.transform.parent!=null && levelsUp<4){
						creatah=creatah.transform.parent.gameObject;
						levelsUp++;
						
						Debug.Log("Name of parent: "+creatah);
					}
				}
				BasicCreature bc = creatah.GetComponent<BasicCreature>();
				
				
				
				
				
				//If it has Basic Creature attached, it's a for real creature and not just a limb or something
				if(bc!=null){
					if(!creaturesInPic.Contains(bc)){						
						//Calculate light value (first see if we've got a flashlight on)
						float lightVal = 0.0f;
						
						Flashlight light = transform.parent.GetComponent<Flashlight>();
						
						if(light!=null && light.myLight!=null && light.myLight.intensity!=0){
							lightVal= 1.0f;
						}
						else{
							//Vector3 lightVector = new Vector3(RenderSettings.ambientLight.r,RenderSettings.ambientLight.g,RenderSettings.ambientLight.b);
							//lightVector.Normalize();
							lightVal = GameObject.Find("DayHandler").GetComponent<DayNightCycle>().restartScene.Percent() + 0.5f;
							lightVal = Mathf.Clamp01(lightVal);
							
							//LightVal is only allowed to be 0.5 at the lowest apparently?
							if(lightVal<0.5f){
								lightVal = 0.5f;
							}
						}
						
						
						//Calculate distance to creature 
						float distanceVal = 0.0f;
						float optimalDistance = 20.0f;
						
						float actualDistance = (transform.position-bc.transform.position).magnitude;
						float closeness = Mathf.Abs((optimalDistance-actualDistance)/optimalDistance);
						distanceVal = Mathf.Clamp01(1.4f-closeness)*100;
						
						//Calculate centered-ness value
						float centeredVal = 0.0f;
						Vector2 screenPos = camera.WorldToScreenPoint(bc.transform.position);
						
						if(screenPos.x>0 && screenPos.x<Screen.width && screenPos.y>0 && screenPos.y<Screen.height){
							//Distance to middle
							float distToMiddle = (screenPos- new Vector2(Screen.width/2, Screen.height/2)).magnitude;
							
							if(distToMiddle!=0){
								float maxPossibleDistanceOnScreen = Mathf.Min(Screen.width/2, Screen.height/2);
								
								centeredVal = 1f - 0.6f * Mathf.Clamp01((distToMiddle/maxPossibleDistanceOnScreen));
								
							}
							else{
								centeredVal = 1.0f;
							}
						}
						
						//Grab the behavior val
						float behaviorVal = bc.currentState;
						
						//Facing you value 
						float facingVal = 0.0f;
						Vector3 lineToPlayer = transform.position-bc.transform.position;
						lineToPlayer.Normalize();
						
						
						float angleBetween = Vector3.Angle(lineToPlayer, bc.transform.forward);
						
						if(angleBetween<15){
							facingVal = 1.5f;
						}
						else if(angleBetween>90){
							facingVal = 0.5f;
						}
						else{
							facingVal = 0.5f + 2*Mathf.Clamp(105*(90 - Mathf.Abs(angleBetween)/90), 0, 90) / 180;
						}
						
						
						//Calculate the rareness valure
						float rarenessVal = bc.rareness;
						
						Dictionary<string, float> tempCriteria = new Dictionary<string, float>();
						tempCriteria.Add("facing", facingVal);
						tempCriteria.Add("center", centeredVal);
						tempCriteria.Add("rareness", rarenessVal);
						tempCriteria.Add("distance", distanceVal);
						tempCriteria.Add("behavior", behaviorVal);
						tempCriteria.Add("light", lightVal);
						
//						scoreToReturn += lightVal*centeredVal * facingVal * rarenessVal *(behaviorVal + distanceVal);
						
						//If they weren't off screen
						if(centeredVal!=0){
							creaturesInPic.Add(bc);
							criteria.Add(tempCriteria);
						}
						else{
							Debug.Log("Creature "+bc.name +" was offscreen");
						}
					}
				}
			}
		}		
		PictureData data = new PictureData(creaturesInPic, criteria);
		
		return data;
		//return scoreToReturn;
	}
	
	void OnPostRender(){
		if (pictureTaken){
			// create a texture to pass to encoding
    	    Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
 	
    	    // put buffer into texture
        	texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
	        texture.Apply();
			textures.Add(texture);
			count++;
			pictureTaken = false;
		}
	}
	
	/**
	IEnumerator ScreenshotEncode()
    {
        // wait for graphics to render
        yield return new WaitForEndOfFrame();
 
        // create a texture to pass to encoding
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
 
        // put buffer into texture
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();
 
		textures.Add(texture);
        // split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;
 
        byte[] bytes = texture.EncodeToPNG();
 
        // save our test image (could also upload to WWW)
        System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/-" + count + ".png", bytes);
        count++;
 
        // Added by Karl. - Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
        DestroyObject( texture );
 
        //Debug.Log( Application.dataPath + "/../testscreen-" + count + ".png" );
    }
	*/
	
	void OnGUI(){
		if(guiOn){
			
			
			//print("Got to here");
			//Texture2D tex = Resources.Load("Screenshot") as Texture2D;
			Texture2D tex = textures.ToArray()[indexLookingAt] as Texture2D;
			
			
			if(tex!=null){
				GUI.Box (new Rect (10,10,Screen.width-20,Screen.height-20), tex);
				
				//GUI.TextArea( new Rect(0,0,Screen.width/4, 30), "Score: "+scores[indexLookingAt]);
			}
			else{
				GUI.Box(new Rect (10,10,100,50), "Nothing Found");
			}
		}
	}
}
