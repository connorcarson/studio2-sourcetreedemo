//This script does three things:
//1) It checks to see if the ball hit something in the world
//2) After it hits something in the world, it checks to see if it enters a scoring zone
//3) If a ball stops moving, it stops it from colliding with other balls, and fades it away

using UnityEngine;
using System.Collections;

public class BallControl : MonoBehaviour {

	public GameObject OneIndicatorPrefab;
	public GameObject TwoIndicatorPrefab;
	public GameObject FourIndicatorPrefab;
	public GameObject SixIndicatorPrefab;
	public GameObject OutIndicatorPrefab;

	private GameObject scoreManager;

	bool canBeDeleted;
	Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		scoreManager = GameObject.Find("ScoreManager");
		canBeDeleted = false;
		rb = GetComponent<Rigidbody2D> ();
	}
	
	//Fixed update is called at a speed independent of the frame rate
	//Try to check and change physics values in fixedUpdate
	void FixedUpdate() {

		//QUADRATIC DRAG EXAMPLE
		float drag = -0.04f; //must be negative to push against the movement of the ball
		//add force proportional to square of velocity
		rb.AddForce (rb.velocity * rb.velocity.magnitude * drag);


		//check to see if a ball has stopped moving, and if so, fade it away.
		if (rb.velocity.magnitude < 0.1f){
			
			if (gameObject.layer ==LayerMask.NameToLayer("Ball")){ //layer for regular balls
				
				//change the collision layer to the ghost ball layer,
				//so that the ball won't collide with other balls
				gameObject.layer = LayerMask.NameToLayer("GhostBall");

				//start fading animation using the Fade animation trigger
				Animator myAnimator = GetComponent<Animator>();
				myAnimator.SetTrigger("Fade");
			}
		}
	}

	//This function is called by the Rigidbody2D component, when the ball touches a collider set to 'Trigger'
	void OnTriggerEnter2D(Collider2D thisCollider){
		
		//Check to see if we touched a Scoring Zone, then delete the ball and add to the score
		if (canBeDeleted){ //don't process a ball that hasn't touched anything in the world yet

			if (thisCollider.gameObject.layer == LayerMask.NameToLayer("ScoreZones")){ //touched a scoreZone
				
				//see which scoring zone we hit, and add the appropriate indicator, and
				//then increment the score
				switch (thisCollider.transform.tag){
					case "One":
						Instantiate(OneIndicatorPrefab, transform.position, Quaternion.identity); //create the indicator graphic
						scoreManager.SendMessage("IncreaseScore", 1); //the score manager gets a message to increase the score
						break;
					case "Two":
						Instantiate(TwoIndicatorPrefab, transform.position, Quaternion.identity);
						scoreManager.SendMessage("IncreaseScore", 2);
						break;
					case "Four":
						Instantiate(FourIndicatorPrefab, transform.position, Quaternion.identity);
						scoreManager.SendMessage("IncreaseScore", 4);
						break;
					case "Six":
						Instantiate(SixIndicatorPrefab, transform.position, Quaternion.identity);
						scoreManager.SendMessage("IncreaseScore", 6);
						break;
					case "Out":
						Instantiate(OutIndicatorPrefab, transform.position, Quaternion.identity);
						scoreManager.SendMessage("EndGame");
						break;
				}
				
				Destroy(gameObject); //destroy the ball

			}
		}
		
	}

	//This function is called by the Rigidbody2D component, when the ball collides with something
	void OnCollisionEnter2D(Collision2D thisCollision){
		//activate the ball after it hits any object that isn't a scoreZone
		if (thisCollision.gameObject.layer != LayerMask.NameToLayer("ScoreZones")){
			canBeDeleted = true; 
		}
	}

	//This function is called by the fading animation, as an animation event.
	public void DeleteMe(){
		//delete the ball
		Destroy(gameObject);
	}

}
