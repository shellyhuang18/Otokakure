using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProgressBar;

public class ScoreBoard : MonoBehaviour {
	int score = 0;
	public Text scoreBoard;
	public Text percentBoard;
	public GameObject gameobj;
	ProgressBarBehaviour BarBehaviour;
	// Use this for initialization
	void Start () {
		BarBehaviour = gameobj.GetComponent <ProgressBarBehaviour> ();
		BarBehaviour.ProgressSpeed = 1000;
		BarBehaviour.Value = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//How to use the functions
		/*
		Progress (prog, 100);
		IncrementScore (1);
		PercentageScore (100);
		prog += 1;
		*/
	}

	void IncrementScore(int value){
		score += value;
		scoreBoard.text = "Score: " + score;
	}

	void PercentageScore(int total){
		double percentHit = score / total;
		percentBoard.text = percentHit + "%";
	}

	//amount = how many passed
	//total = how many there are
	void Progress(int amount, int total){
		double progress = amount / total;
		BarBehaviour.Value = (float)progress * 100;
		//BarBehaviour.SetFillerSizeAsPercentage((float)progress * 100);
	}
}
