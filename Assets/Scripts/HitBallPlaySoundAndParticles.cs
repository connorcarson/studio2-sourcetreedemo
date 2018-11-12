//This sprite plays the attached AudioSource whenever an object with the tag
//"Ball" touches it. We use it for the ground 'thud' and the bat sound.

using UnityEngine;
using System.Collections;

public class HitBallPlaySoundAndParticles : MonoBehaviour
{

	public ParticleSystem particles; //keep a reference to the particle system in the inspector
	
	//this function is called by the Rigidbody2D component whenever another collider
	//touches this object's collider.
	void OnCollisionEnter2D(Collision2D thisCollision){
		if (thisCollision.collider.tag == "Ball"){ //only play a sound if a ball touched this object
			//set the volume of the sound depending on how fast the collision was
			GetComponent<AudioSource>().volume = Mathf.Clamp01(thisCollision.relativeVelocity.magnitude * 0.05f);
			//play the attached default audio clip (set in the AudioSource component)
			GetComponent<AudioSource>().Play();

			if (particles == null) return;//the bat doesn't have a particle system, so just stop the function here
			
			//move the particle emitter to the point of contact
			particles.transform.position = thisCollision.GetContact(0).point;
			//emit some particles (more if the collision is fast)
			particles.Emit((int)thisCollision.relativeVelocity.magnitude);
		}
	}
}
