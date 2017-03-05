using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour 
{

	public GameObject GameManagerGo;//refernce to our game manager

	public GameObject PlayerBulletGo; //This is our player's bullet prefab
	public GameObject bulletPosition01;
	public GameObject bulletPosition02;
	public GameObject ExplosionGo;//this is our explosion prefab

	//Reference to the lives ui text
	public Text LivesUIText;

	const int MaxLives = 3;//maximum player lives
	int lives;//current player lives

	public float speed;

	public void Init()
	{
		lives = MaxLives;

		//update the lives UI text
		LivesUIText.text = lives.ToString ();

		//Reset the player position to the center of the screen
		transform.position = new Vector2 (0, 0);

		//set this player game object to active
		gameObject.SetActive(true);
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//fire bullets when the spacebar is pressed
		if (Input.GetKeyDown ("space")) 
		{
			//play the laser sound effect
			gameObject.GetComponent<AudioSource>().Play();

			//instantiate the first bullet
			GameObject bullet01 = (GameObject)Instantiate (PlayerBulletGo);
			bullet01.transform.position = bulletPosition01.transform.position; //set the bullet initial position

			//instantiante the second bullet
			GameObject bullet02 = (GameObject)Instantiate (PlayerBulletGo);
			bullet02.transform.position = bulletPosition02.transform.position; //set the bullet initial position
		}

		float x = Input.GetAxisRaw ("Horizontal"); //the value will be -1, 0, or 1 (for left, no input, and right)
		float y = Input.GetAxisRaw ("Vertical"); //the value will be -1, 0, or 1 (for down, no input, and up)

		//now based on the input we compute a direction vector, and we normalize it to get a unit vector
		Vector2 direction = new Vector2 (x, y).normalized;

		//now we call the function that computes and sets the player's position
		Move (direction);
	}
	void Move(Vector2 direction)
	{
		//Find the screen limits to the player's movement (left, right, top and bottom edges of the screen)
		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2 (0, 0)); //This is the bottom-left point (corner) of the screen
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2 (1, 1)); //This is the top-right point (corner) of the screen

		max.x = max.x - 0.225f; //Subtract the player sprite half width
		min.x = min.x + 0.225f; //add the player sprite half width

		max.y = max.y - 0.285f;	//Subtract the player sprite half width
		min.y = min.y + 0.285f; //add the player sprite half width

		//Get the player's current position
		Vector2 pos = transform.position;

		//Calculate the new position
		pos += direction * speed * Time.deltaTime;

		//Make sure the new position is not outside the screen
		pos.x = Mathf.Clamp (pos.x, min.x, max.x);
		pos.y = Mathf.Clamp (pos.y, min.y, max.y);

		//Update the player's position
		transform.position = pos;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		//Detect collision of the player ship with an enemy ship, or with an enemy bullet
		if ((col.tag == "EnemyShipTag") || (col.tag == "EnemyBulletTag")) 
		{
			PlayExplosion ();

			lives--;//substract one live
			LivesUIText.text = lives.ToString();//update lives UI text

			if(lives == 0) //if our player is dead
			{
				//Change game manager state to game over state
				GameManagerGo.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
			
				//hide the player's ship
				gameObject.SetActive(false);
			}
		}
	}

	//Function to instantiate an explosion
	void PlayExplosion()
	{
		GameObject explosion = (GameObject)Instantiate (ExplosionGo);

		//set the position of the explosion
		explosion.transform.position = transform.position;
	}
}
