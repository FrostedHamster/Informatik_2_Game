using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{
	public GameObject EnemyGo; //this is our enemy prefab

	float maxSpawnRateInSeconds = 5f;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	//Function to spawn an enemy
	void SpawnEnemy()
	{
		//this is the bottom-left point of the screen
		Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));

		//this is the top-right point of the screen
		Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));

		//instantiate an enemy
		GameObject anEnemy = (GameObject)Instantiate (EnemyGo);
		anEnemy.transform.position = new Vector2 (Random.Range (min.x, max.x), max.y);

		//Schedule when to spawn next enemy
		ScheduleNextEnemySpawn();
	}

	void ScheduleNextEnemySpawn()
	{
		float spawnInNSeconds;

		if (maxSpawnRateInSeconds > 1f) {
			//pixk a number between 1 and maxSpawnRateInSeconds
			spawnInNSeconds = Random.Range (1f, maxSpawnRateInSeconds);
		} else
			spawnInNSeconds = 1f;

		Invoke ("SpawnEnemy", spawnInNSeconds);
	}

	//Function to increase the difficulty of the game
	void IncreaseSpawnRate()
	{
		if (maxSpawnRateInSeconds > 1f)
			maxSpawnRateInSeconds--;

		if (maxSpawnRateInSeconds == 1f)
			CancelInvoke("IncreaseSpawnRate");
	}

	//Function to start enemy spawner
	public void ScheduleEnemySpawner()
	{
		//reset max spawn rate
		float maxSpawnRateInSeconds = 5f;

		Invoke ("SpawnEnemy", maxSpawnRateInSeconds);

		//increase spawn rate every 30 seconds
		InvokeRepeating ("IncreaseSpawnRate", 0f, 30f);
	}

	//Function to stop enemy spawner
	public void UnscheduleEnemySpawner()
	{
		CancelInvoke ("SpawnEnemy");
		CancelInvoke ("IncreaseSpawnRate");
	}
}
