using UnityEngine;
using System.Collections;


//A leg is a limb that allows for movement, and has an animation attached for potentially lots of states
public class Leg : MonoBehaviour {
	//The animation for walk (NOTE: THE LEG HAS TO HAVE AT LEAST THIS OR ELSE EVERYTHING WILL BREAK)
	public string walkAnimation;
	//The animation for running (moving fast) (ususally just use walkAnimation sped up)
	public string runAnimation;
	//Other animations on this leg
	public string[] animations;
	//The amount to speed the walk animation by to make it look like it's running
	public float walkAnimationSpedUpAmount = 2.0f;
	
	
	//plays the default animation on this leg
	public void PlayAnimation(){
		animation.Play();
		
	}
	
	//Plays the specified animation
	public void PlayAnimation(string animationName){
		
		animation.Play(animationName);
			
	}
	
	
	//Stops all Animations
	public void StopAnimation(){
		//print("I know you want me to stop");
		animation.Stop();
	}
	
	
	//Stops specified animation
	public void StopAnimation(string animationName){
		
		animation.Stop(animationName);
	}
	
	//Set to walking (normal movement)
	public void setWalking(){
		animation[walkAnimation].speed=1.0f;
		animation.Play(walkAnimation);
	}
	
	//Set to running (fast movement)
	public void setRunning(){
		//If run animation does not exist
		if(runAnimation==""){
			animation[walkAnimation].speed=walkAnimationSpedUpAmount;
			animation.Play(walkAnimation);
		}
		else{
			
			animation.Play(runAnimation);
		}
	}
	
	//Returns whether or not the leg is presently animating
	public bool isPlaying(){
		return animation.isPlaying;
	}
	
	//Returns whether or not the leg is playing a specific animation
	public bool isPlaying(string specificAnimation){
		return animation.IsPlaying(specificAnimation);
	}
	
	//Returns whether or not we're doing the walk animation
	public bool isWalking(){
		return animation.IsPlaying(walkAnimation);
	}
	
	//Return true if we are running, false otherwise
	public bool isRunning(){
		//if we have a run animation
		if(runAnimation!=""){
			return animation.IsPlaying(runAnimation);
		}
		//If we don't
		else{
			if(animation[walkAnimation].speed==walkAnimationSpedUpAmount && isWalking()){
				return true;
			}
			else{
				return false;
			}
			
		}
		
		
	}
	
	//Returns true if the passed in animation exists in the list of animations available to this leg
	//Returns false otherwise
	public bool hasAnimation(string animation){
		bool contains=false;
		
		foreach(string animationName in animations){
			if(animationName.Equals(animation)){
				contains=true;
			}
		}
		
		return contains;
	}
	
	
}
