//This script is used to prevent the 'shine' sprite of the ball from
//rotating along with the base sprite

using UnityEngine;
using System.Collections;

public class BallShine_KeepUpright : MonoBehaviour {
	
	// Update is called once per frame, AFTER the Update is called but before everything gets drawn
	void LateUpdate () {
		//keep ball shine from rotating with the ball
		transform.eulerAngles = new Vector3(0,0, 0);
	}
}
