//This script detects if the ball hit the stumps, and it breaks their joints holding
//them in place, and tells the scoremanager that the game ended.
//It also tells any other stump elements to ignore collisions after one collision happens,
//because we don't want the game to end more than once.

using UnityEngine;
using System.Collections;

public class StumpControl : MonoBehaviour {

	private GameObject scoreManager;
	private bool ignoreCollisions;
	
	// Use this for initialization
	void Start () {
		scoreManager = GameObject.Find("ScoreManager");
		ignoreCollisions = false;
	}

	//This function is called by the Rigidbody2D if an object hits the attached collider
	void OnCollisionEnter2D(Collision2D thisCollision){
	
		if (ignoreCollisions == false){ //we only want to call this function one time
			//player is out if ball hits either stump, so tell the score manager to end the game
			scoreManager.SendMessage("EndGame");

			//play the sound
			GetComponent<AudioSource>().Play();

			//kill all the joints holding this structure together
			//first get all the hingejoint2Ds belonging to this object's parent
			Component[] hingeJoints = transform.parent.gameObject.GetComponentsInChildren<HingeJoint2D>();
			foreach (HingeJoint2D thisJoint in hingeJoints){
				Destroy(thisJoint);
			}

			//having destroyed the joint, reapply the force of the collision to this object
			Vector2 contactPoint = thisCollision.contacts[0].point;
			GetComponent<Rigidbody2D>().AddForceAtPosition(thisCollision.relativeVelocity*-0.1f, contactPoint, ForceMode2D.Impulse);

			//to make sure this can't be called again on ANY stump component
			//we will send a message to every child of the container object
			//telling it to ignore future collisions
			transform.parent.gameObject.BroadcastMessage("IgnoreCollisions");
			
		}
	}

	void IgnoreCollisions(){
		ignoreCollisions = true; //make sure we don't call the collision function again
	}
}
