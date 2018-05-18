//Contributors: Rubaiyat Rashid, Naseeb Gafar, Jack CHen

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProgressBar;
using Song = NoteLogic.NoteLogic.Song;

public class ScoreBoard : MonoBehaviour {
	int score = 0;
	int total_possible_hits= 0; //Total hits possible for the song
	public Text scoreBoard;
	public GameObject gameobj; //Progress Bar Object
	ProgressBarBehaviour BarBehaviour;

	GameObject game_window;
	private int progress_tick = 0; //How many updateProgress's were called

	// Use this for initialization
	void Start () {
		BarBehaviour = gameobj.GetComponent <ProgressBarBehaviour> ();
		BarBehaviour.ProgressSpeed = 1000;
		BarBehaviour.Value = 0;

		game_window = GameObject.FindGameObjectWithTag ("GameWindow");
	}

	//Takes in a song and sets the progress bar's max to the song's length (Not including rests)
	public void setSongToProgressBar(Song song){
		//First find the total distance traversed by the song and the size of the game_window


		//First we get the distance of the song
		GameObject note_prefab = Resources.Load ("prefabs/note") as GameObject;
		float whole_note_width = note_prefab.GetComponent<SpriteRenderer> ().sprite.bounds.size.x;

		int total_16th_notes = 0;
		foreach(NoteLogic.NoteLogic.Note note in song.score){
				total_16th_notes += note.duration;
		}
		float total_whole_notes = total_16th_notes / 16f;

		//The distances of the two parts
		float total_dist_traveled_by_song = total_whole_notes * whole_note_width;
		float total_dist_region = game_window.GetComponent<SpriteRenderer> ().bounds.size.x;

		//Now to compute the total frames needed for the total
		float total_dist = total_dist_region + total_dist_traveled_by_song;
		float quarter_note_width = whole_note_width / 4.0f;
		float dist_per_sec = (game_window.GetComponent<GameWindow>().getTempo() * quarter_note_width) / 60.0f; //Velocity = Unity Unit/s   (unity unit should be defaulted to meter)
		float total_seconds = total_dist/dist_per_sec;
		float frame_per_sec = 1 / Time.fixedDeltaTime;
		float total_frames = total_seconds * frame_per_sec;

		this.total_possible_hits = (int)Math.Floor(total_frames);

	}
		

	public void incrementScore(int value = 1){
		score += value;
		scoreBoard.text = "Score: " + score;
	}


	//amount = how many passed
	//total = how many there are
	public void updateProgress(){
		//Only update if a song has started
		if(game_window.GetComponent<GameWindow>().songPlayingStatus()){
			progress_tick += 1;
			if (total_possible_hits != 0) {
				float progress = (float)progress_tick / (float)total_possible_hits;
				BarBehaviour.Value = (float)progress * 100f; //Sets the value of the progress from 0 to 100
			} 
		}
	}

	public int getScore(){
		return this.score;
	}
}
