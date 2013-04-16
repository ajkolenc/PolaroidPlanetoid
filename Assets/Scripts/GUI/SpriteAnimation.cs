using UnityEngine;
using System.Collections;

public class SpriteAnimation {
	public Texture2D[] frames;
	bool playing, repeat;
	Timer animationTimer;
	float animationTime;
	int framePointer;

	public SpriteAnimation(int numFrames, float time = .1f, bool repeat = false){
		frames = new Texture2D[numFrames];
		this.repeat = repeat;
		playing = false;
		framePointer = 0;
		animationTime = time;
		animationTimer = new Timer(animationTime);
		animationTimer.Stop();
	}
	
	public void AddFrame(Color[] frame, int width, int height){
		frames[framePointer] = new Texture2D(width, height);
		frames[framePointer].SetPixels(frame);
		frames[framePointer].Apply();		
		framePointer++;
	}
	
	public Texture2D PlayAnimation(float secondsPerFrame){
		if (!playing){
			framePointer = 0;
			animationTimer = new Timer(secondsPerFrame);
			playing = true;
		}
		else{
			if (animationTimer.IsFinished()){
				animationTimer.Restart();
				framePointer+= 1;
				if (framePointer >= frames.Length){
					if (repeat)
						framePointer %= (frames.Length);
					else
						framePointer = frames.Length - 1;
				}
			}
		}
		return frames[framePointer];	
	}
	
	public bool IsStopped(){
		return framePointer == (frames.Length - 1) && !repeat;	
	}
	
	public void StopAnimation(){
		playing = false;	
	}
}