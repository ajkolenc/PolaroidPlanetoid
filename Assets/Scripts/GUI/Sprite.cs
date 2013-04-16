using UnityEngine;
using System.Collections;

public class Sprite {
	
	public string filename;
	public int spriteWidth, spriteHeight;
	public SpriteAnimation[] animations;
	public Texture2D spriteSheet;
		
	// Assumes a sprite sheet with each row as an animation
	public Sprite (string filename, int spriteWidth, int spriteHeight){
		spriteSheet = Resources.Load(filename) as Texture2D;
		this.spriteWidth = spriteWidth;
		this.spriteHeight = spriteHeight;
		BreakIntoAnimations();
	}
	
	void BreakIntoAnimations(){
		int numAnimations = spriteSheet.height/spriteHeight;
		int numFrames = spriteSheet.width/spriteWidth;
		animations = new SpriteAnimation[numAnimations];
		for (int i = 0; i < numAnimations; i++){
			animations[numAnimations - 1 - i] = new SpriteAnimation(numFrames);
			for (int j = 0; j < numFrames; j++){
				animations[numAnimations - 1 - i].AddFrame(spriteSheet.GetPixels(j*spriteWidth, i*spriteHeight, spriteWidth, spriteHeight),spriteWidth, spriteHeight);
			}
		}
	}
}
