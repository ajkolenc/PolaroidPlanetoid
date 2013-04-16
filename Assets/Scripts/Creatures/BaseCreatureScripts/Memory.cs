using UnityEngine;
using System.Collections;

//An information holder for a specific memory of a specific object for a creature
public class Memory{
	//The object this memory is focused on
	public GameObject memoryObject;
	//The strategy the creature has for the enemy
	public int memoryStrategy;
	//For how much longer the creature will remember this memory;
	public float memoryTimer;
	
	
	
	public Memory(GameObject _memoryObject, int _memoryStrategy, float _memoryTimer){
		memoryObject=_memoryObject;
		memoryStrategy=_memoryStrategy;
		memoryTimer=_memoryTimer;
		
	}
	
	//Reset memoryTimer to something new
	public void setTimer(float _memoryTimer){
		memoryTimer=_memoryTimer;
	}
	
	//Decrements this memory timer, returns the current value of the memory
	public float decrementTimer(){
		memoryTimer-=Time.deltaTime;
		
		return memoryTimer;
	}
}
