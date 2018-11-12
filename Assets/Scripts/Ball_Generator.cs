//This script generates new balls and throws them at the player at random intervals.

using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

public class Ball_Generator : MonoBehaviour {
	public GameObject ballPrefab;
	private float timeUntilNextBall;
	private bool shouldThrow;

	//these tuning values are set in the inspector
	[Header("Delay before firing first ball")]
	[Range(0.0f, 10.0f)]
	public float START_DELAY; //wait before firing first ball
	[Header("Delay between balls getting fired")]
	[Range(1.0f, 10.0f)]
	public float MIN_DELAY; //minimum time between firing balls
	[Range(1.0f, 10.0f)]
	public float MAX_DELAY;	//maximum time between firing balls
	[Header("Random variation in ball starting height")]
	[Range(1.0f, 5.0f)]
	public float HEIGHT_MIN; //maximum spatial offset for random ball position
	[Range(1.0f, 5.0f)]
	public float HEIGHT_MAX; //maximum spatial offset for random ball position
	
	[Header("Minimum and maximum landing position of balls")]
	[Range(6f, 13.0f)]
	public float MIN_RANGE; //minimum starting force for balls
	[Range(6f, 13.0f)]
	public float MAX_RANGE; //maximum starting force for balls
	[Header("Minimum and maximum starting force for balls")]
	[Range(5.0f, 20.0f)]
	public float MIN_STARTSPEED; //minimum starting force for balls
	[Range(5.0f, 20.0f)]
	public float MAX_STARTSPEED; //maximum starting force for balls
	[Header("Maximum spin speed for balls")]
	[Range(0.0f, 200.0f)]
	public float MAX_SPIN; //maximum spin [torque] force for balls


	// Use this for initialization
	void Start () {
		shouldThrow = true;
		timeUntilNextBall = START_DELAY; //give the player time to get ready
	}
	
	// Update is called once per frame
	void Update () {
		if (shouldThrow==true){ //don't do anything if we've been told not to throw
			timeUntilNextBall -= Time.deltaTime;

			if (timeUntilNextBall <= 0){ //time is up
				//throw a new ball
				//first create a random position offset so the ball doesn't always appear at same height
				float rand_y = Random.Range(HEIGHT_MIN, HEIGHT_MAX);
				Vector3 position = new Vector3(transform.position.x, rand_y, transform.position.z);
				GameObject newBall = Instantiate(ballPrefab, position , Quaternion.identity) as GameObject;
				Rigidbody2D newBallRB = newBall.GetComponent<Rigidbody2D> ();

				//apply forces to move and spin the ball
				//first create a random starting speed
				float v = Random.Range(MIN_STARTSPEED,MAX_STARTSPEED);

				//now choose an angle which will make the balls bounce in roughly the same place regardless of power
				float g = Physics2D.gravity.y;
				//choose a randomized landing point
				float x = Random.Range(MIN_RANGE,MAX_RANGE);
				
				//i simply googled how to figure out the angle of a trajectory
				//based on a starting speed, range and gravity value
				//don't worry about it if the equation doesn't make sense.
				
				//first pick a good angle to use in case we don't have enough power to reach the target
				float initialAngle = Random.Range(40f * Mathf.Deg2Rad, 50f * Mathf.Deg2Rad);
				float innerTerm = v * v * v * v + g * (g*x * x + 2 * rand_y * v * v);
				//then choose the correct angle if we do have enough power
				if (innerTerm > 0) //can't take a sqrt of a negative number
					initialAngle = Mathf.Atan((v * v - Mathf.Sqrt(innerTerm))/(g * x));
				
				//convert angle and power to a vector
				Vector2 initialVelocity = new Vector2(Mathf.Cos(initialAngle)*v, Mathf.Sin(initialAngle)*v);

				//apply the vector as the velocity of the ball 
				newBallRB.velocity = initialVelocity;

				//apply a random spin to the ball
				float initialSpin = Random.Range(-MAX_SPIN, MAX_SPIN);
				newBallRB.AddTorque(initialSpin);

				//set the timer to a random amount
				timeUntilNextBall = Random.Range(MIN_DELAY,MAX_DELAY);
			}
		}
	}


	public void StopThrowing() {
		shouldThrow = false;
	}
}
