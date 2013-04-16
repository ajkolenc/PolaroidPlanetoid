using UnityEngine;
using System.Collections;

public class Timer {
	float startTime, timerInterval, percentDone;
	public bool stopped, repeating;
	bool gameTime = false;
	
	public Timer(float timerInterval, bool useGameTime = false){
		gameTime = useGameTime;
		this.timerInterval = timerInterval; //time between timer going off (in seconds)
		this.Restart();
		repeating = false;
	}
	
	public bool IsFinished(){
		if (!stopped){
			float time = Time.realtimeSinceStartup;
			if (gameTime)
				time = Time.time;
			if (time > startTime + timerInterval){
				if (repeating){
					Restart(); //automatically restart timer when it's found to be over
				}
				else {
					Stop();
				}
				return true;
			}
			else{
				return false;
			}
		}
		else{
			return true;	
		}
	}
	
	public void Repeat(){
		repeating = true;	
	}
	
	public float Percent(){
		float time = Time.realtimeSinceStartup;
		if (gameTime)
			time = Time.time;
		percentDone = Mathf.Clamp01((time - startTime)/timerInterval);
		if (percentDone >= 1 && repeating)
			Restart();
		return percentDone;
	}	
	
	public void Restart(){
		startTime = Time.realtimeSinceStartup;
		if (gameTime)
			startTime = Time.time;
		Start();
	}
	
	public bool IsStopped(){
		return stopped;	
	}

	public void Stop(){
		stopped = true;
	}

	public void Start(){
		stopped = false;
	}
}
