using UnityEngine;
using System.Collections;

public class CreatureSpawner : MonoBehaviour {
	//THE MAX NUMBER ALLOWABLE FOR EACH CREATURE
	public int rabbitMax=7, friendlyMax = 6, bullMax = 3, lovesPlayerMax = 4, dragonMax = 1, whaleMax = 2;
	
	
	//Prefabs for various things
	public GameObject rabbit;
	public Transform rabbitPositions;
	
	public GameObject friendly;
	public Transform friendlyPositions;
	
	public GameObject bull;
	public Transform bullPositions;
	
	public GameObject lovesPlayer;
	public Transform lovesPlayerPositions;
	
	public GameObject dragon;
	public Transform dragonPositions;
	
	public GameObject flying;
	
	public GameObject whale;
	public Transform whalePositions;
	
	
	// Use this for initialization
	void Awake () {
		SpawnCreature(rabbit, Random.Range(4, rabbitMax+1), rabbitPositions);
		SpawnCreature(friendly, Random.Range(2, friendlyMax+1), friendlyPositions);
		SpawnCreature(bull, Random.Range(1, bullMax+1), bullPositions);
		SpawnCreature(lovesPlayer, Random.Range(1, lovesPlayerMax+1), lovesPlayerPositions);
		SpawnCreature(dragon, Random.Range(0, dragonMax+1), dragonPositions);
		
		SpawnWhale(whale, Random.Range(1, whaleMax+1), whalePositions);
		
	}
	
	
	
	//Whale Creature Spawn (has to worry about spawning flying dudes)
	public void SpawnWhale(GameObject creatureToSpawn, int numberToSpawn, Transform spawnPointHolder){
		Transform[] spawnPoints = spawnPointHolder.GetComponentsInChildren<Transform>();
		int[] usedPoints = new int[spawnPoints.Length];
		for(int i=0; i<usedPoints.Length; i++){
			usedPoints[i]=-1;
		}
		
		int numPlaced = 0;
		
		while(numPlaced<numberToSpawn){
			int placeToPut = Random.Range(0, spawnPoints.Length);
			
			bool alreadyPlacedThere = false;
			
			for(int i =0; i<usedPoints.Length; i++){
				if(usedPoints[i]==placeToPut){
					alreadyPlacedThere=true;
				}
			}
			
			
			if(!alreadyPlacedThere){
				GameObject creature = Instantiate(creatureToSpawn, spawnPoints[placeToPut].position,spawnPoints[placeToPut].rotation) as GameObject;
				
				creature.name = creatureToSpawn.name;
				
				usedPoints[numPlaced] = placeToPut;
				
				//NOW SPAWN FLYING CREATURES
				WhaleController whale = creature.GetComponent<WhaleController>();
				
				SpawnFlyingCreature(flying,Random.Range(0,5), whale.flyingGuySpawnPos);
				
				
				numPlaced++;
				
				
				
			}
		}
		
	}
	
	//Flying Creature Spawner
	public void SpawnFlyingCreature(GameObject creatureToSpawn, int numberToSpawn, Transform[] spawnPoints){
		int[] usedPoints = new int[spawnPoints.Length];
		for(int i=0; i<usedPoints.Length; i++){
			usedPoints[i]=-1;
		}
		
		int numPlaced = 0;
		
		while(numPlaced<numberToSpawn){
			int placeToPut = Random.Range(0, spawnPoints.Length);
			
			bool alreadyPlacedThere = false;
			
			for(int i =0; i<usedPoints.Length; i++){
				if(usedPoints[i]==placeToPut){
					alreadyPlacedThere=true;
				}
			}
			
			
			if(!alreadyPlacedThere){
				GameObject creature = Instantiate(creatureToSpawn, spawnPoints[placeToPut].position,spawnPoints[placeToPut].rotation) as GameObject;
				
				creature.name = creatureToSpawn.name;
				
				creature.transform.parent = spawnPoints[placeToPut];
				usedPoints[numPlaced] = placeToPut;
				
				numPlaced++;
				
				
				
			}
		}
		
	}
	
	
	//Regular Creature Spawn
	public void SpawnCreature(GameObject creatureToSpawn, int numberToSpawn, Transform spawnPointHolder){
		Transform[] spawnPoints = spawnPointHolder.GetComponentsInChildren<Transform>();
		int[] usedPoints = new int[spawnPoints.Length];
		for(int i=0; i<usedPoints.Length; i++){
			usedPoints[i]=-1;
		}
		
		int numPlaced = 0;
		
		while(numPlaced<numberToSpawn){
			int placeToPut = Random.Range(0, spawnPoints.Length);
			
			bool alreadyPlacedThere = false;
			
			for(int i =0; i<usedPoints.Length; i++){
				if(usedPoints[i]==placeToPut){
					alreadyPlacedThere=true;
				}
			}
			
			
			if(!alreadyPlacedThere){
				GameObject creature = Instantiate(creatureToSpawn, spawnPoints[placeToPut].position,spawnPoints[placeToPut].rotation) as GameObject;
				
				creature.name = creatureToSpawn.name;
				
				usedPoints[numPlaced] = placeToPut;
				
				numPlaced++;
				
				
				
			}
		}
		
	}
}
