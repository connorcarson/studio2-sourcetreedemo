//This script handles scoring and resetting the game when the player dies

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreManager : MonoBehaviour {
	public int score;
	public int timesScored;
	public TextMesh scoreText;
	public AudioClip OneSound;
	public AudioClip TwoSound;
	public AudioClip FourSound;
	public AudioClip SixSound;
	public AudioClip OutSound;
	private GameObject ballGenerator;


	// Use this for initialization
	void Start () {
		score = 0;
		timesScored = 0;
		ballGenerator = GameObject.Find("BallThrower");
	}

	//This function is called by the ball to increase the score
	public void IncreaseScore(int scoreToAdd){
		score += scoreToAdd; //increment the score
		timesScored ++; //we want to keep a running average, so we need to know how many times the player scored
	
		//play a sound depending on how much the score increased by
		switch (scoreToAdd){
			case 1:
				GetComponent<AudioSource>().PlayOneShot(OneSound,1.0f);
				break;
			case 2:
				GetComponent<AudioSource>().PlayOneShot(TwoSound,1.0f);
				break;
			case 4:
				GetComponent<AudioSource>().PlayOneShot(FourSound,1.0f);
				break;
			case 6:
				GetComponent<AudioSource>().PlayOneShot(SixSound,1.0f);
				break;
		}

		//change the score text at the bottom of the screen.
		float averageScore = score/timesScored;
		float totalScore = score * averageScore;
		//in C#, numbers have a .ToString() method where you can specify how many decimal places to show
		scoreText.text = "Score: " + score.ToString() + " runs x " + averageScore.ToString("F1") + " average = " + totalScore.ToString("F0") + " points";
	}

	//This function is called by the ball or by the stumps to end the game, if the ball hits a deadly object.
	public void EndGame(){
		GetComponent<AudioSource>().PlayOneShot(OutSound,1.0f); //play the game over sound
		ballGenerator.SendMessage("StopThrowing"); //don't want balls appearing after game ends, to tell the ball generator this
		Invoke("ResetGame", 2.0f);//wait a bit and then call the ResetGame() function (below)
	}

	public void ResetGame(){
		//just reload the scene - easier than writing a complicated reset function (but slower too)
		SceneManager.LoadScene ("Main");
	}
}
