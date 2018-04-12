using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour {
	int score = 0;
	public Text scoreBoard;
	public Text percentBoard;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void IncrementScore(int value){
		score += value;
		scoreBoard.text = "Score: " + score;
	}

	void PercentageScore(int total){
		double percentHit = score / total;
		percentBoard.text = percentHit + "%";
	}

	void Progress(int amount, int total){
		double progress = amount / total;
	}
}
