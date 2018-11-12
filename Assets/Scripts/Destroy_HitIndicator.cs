//This script is used to destroy a hit indicator after its animation plays

using UnityEngine;
using System.Collections;

public class Destroy_HitIndicator : MonoBehaviour {

	//This function is called by the animation when it comes to an end
	//The animation has an 'animation event' which calls this function
	public void DestroySelf(){
		//destroy this hit indicator
		Destroy(gameObject);
	}
}
