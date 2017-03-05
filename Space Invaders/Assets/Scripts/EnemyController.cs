using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour 
{
	GameObject scoreUITextGo;//Reference to the text UI game object

	public GameObject ExplosionGo;//this is our explosion prefab

	float speed; //for the enemy speed
	// Use this for initialization
	void Start () 
	{
		speed = 16f; //set speed

		//Get the score text UI
		scoreUITextGo = GameObject.FindGameObjectWithTag("ScoreTextTag");
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Get the enemy current position
		Vector2 position = transform.position;

		//Compute the enemy new position
		position = new Vector2 (position.x, position.y - speed * Time.deltaTime);

		//Update the enemy position
		transform.position = position;

		//this is the bottom-left point of the screen
		Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));

		//if the enemy went outside the screen on the bottom, then destroy the enemy
		if (transform.position.y < min.y) {
			Destroy (gameObject);
		}
	}
		void OnTriggerEnter2D(Collider2D col)
		{
			//Detect collision of the enemy ship with the player ship, or with a player's bullet
			if ((col.tag == "PlayerShipTag") || (col.tag == "PlayerBulletTag")) 
			{
				PlayExplosion ();

				//add 100 points to the score
				scoreUITextGo.GetComponent<GameScore>().Score += 100;

				//Destroy this enemy ship
				Destroy (gameObject);
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
