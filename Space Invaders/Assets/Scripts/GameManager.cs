using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	//Reference to our game objects
	public GameObject playButton;
	public GameObject playerShip;
	public GameObject enemySpawner;//reference to our enemy spawner
	public GameObject GameOverGo;//reference to the  game over image
	public GameObject scoreUITextGo;//Reference to the score text UI game
	public GameObject GameTitleGo;//Rreference to title UI

	public enum GameManagerState
	{
		Opening,
		Gameplay,
		GameOver,
	}

	GameManagerState GMState;

	// Use this for initialization
	void Start () 
	{
		GMState = GameManagerState.Opening;
	}
	
	//Function to update the game manager state
	void UpdateGameManagerState()
	{
		switch (GMState) 
		{
		case GameManagerState.Opening:

			//Hide game over
			GameOverGo.SetActive(false);

			//Display the game title
			GameTitleGo.SetActive(true);

			//Set play button  visible(active)
			playButton.SetActive(true);

			break;
		case GameManagerState.Gameplay:

			//Reset the score
			scoreUITextGo.GetComponent<GameScore>().Score = 0;

			//hide play button on game play state
			playButton.SetActive(false);

			//hide game title
			GameTitleGo.SetActive(false);

			//set the player visible(active) and init player lives
			playerShip.GetComponent<PlayerControl>().Init();

			//Start enemy spawner
			enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();

			break;

		case GameManagerState.GameOver:

			//Stop enemy spawner
			enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();

			//Display game over
			GameOverGo.SetActive(true);

			//Change game manager state to opening state
			Invoke("ChangeToOpeningState", 8f);

			break;
		}
	}

	//Function to set the game manager state
	public void SetGameManagerState(GameManagerState state)
	{
		GMState = state;
		UpdateGameManagerState ();
	}

	//our play button will call this functio
	//when the user clicks the button
	public void StartGamePlay ()
	{
		GMState = GameManagerState.Gameplay;
		UpdateGameManagerState ();
	}
	//Function to change game manager state to opening state
	public void ChangeToOpeningState()
	{
		SetGameManagerState (GameManagerState.Opening);
	}
}
