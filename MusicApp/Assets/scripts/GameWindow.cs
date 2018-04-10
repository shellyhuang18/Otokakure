using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor;
using PitchLine;
using Utility;
using UnityEngine.SceneManagement;


public class GameWindow : MonoBehaviour {
	//UI Game Objects
	GameObject pitchline;
	GameObject conductor;


	//Variables associated with the entire Game Window
	public string lowest_pitch;
	public string highest_pitch;
	public float tempo;

	// Use this for initialization
	void Start () {
		pitchline = (GameObject)GameObject.Find ("pitch_line");
		conductor = (GameObject)GameObject.Find ("conductor");
	}
		
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("`")) {
			conductor.GetComponent<CreateNoteGenerator>().triggerPitch ("c4", 4);
		}
		if (Input.GetKeyDown ("1")) {
			conductor.GetComponent<CreateNoteGenerator>().triggerPitch ("c#4", 4);
		}
		if (Input.GetKeyDown ("2")) {
			conductor.GetComponent<CreateNoteGenerator>().triggerPitch ("d4", 4);
		}
		if (Input.GetKeyDown ("3")) {
			conductor.GetComponent<CreateNoteGenerator>().triggerPitch ("d#4", 4);
		}
		if (Input.GetKeyDown ("4")) {
			conductor.GetComponent<CreateNoteGenerator>().triggerPitch ("e4", 4);
		}
		if (Input.GetKeyDown ("5")) {
			conductor.GetComponent<CreateNoteGenerator>().triggerPitch ("f4", 4);
		}
		if (Input.GetKeyDown ("6")) {
			conductor.GetComponent<CreateNoteGenerator>().triggerPitch ("f#4", 4);
		}
		if (Input.GetKeyDown ("7")) {
			conductor.GetComponent<CreateNoteGenerator>().triggerPitch ("g4", 4);
		}
		if (Input.GetKeyDown ("8")) {
			conductor.GetComponent<CreateNoteGenerator>().triggerPitch ("g#4", 4);
		}
		if (Input.GetKeyDown ("9")) {
			conductor.GetComponent<CreateNoteGenerator>().triggerPitch ("a4", 4);
		}
		if (Input.GetKeyDown ("0")) {
			conductor.GetComponent<CreateNoteGenerator> ().triggerPitch ("a#4", 4);
		}
		if (Input.GetKeyDown ("-")) {
			conductor.GetComponent<CreateNoteGenerator>().triggerPitch ("b4", 4);
		}
		if (Input.GetKeyDown ("=")) {
			conductor.GetComponent<CreateNoteGenerator>().triggerPitch ("c5", 4);
		}

	}


//====== Variable Mutators and Getters ======
	public void setTempo(float tempo){
		GameObject conductor = (GameObject)GameObject.Find ("conductor");
		conductor.GetComponent<CreateNoteGenerator> ().setTempo (tempo);
	}

	public void setPitchRange(string lower_pitch, string higher_pitch){
		if(Utility.Pitch.isHigherPitch(lower_pitch, higher_pitch)){
			this.lowest_pitch = lower_pitch;
			this.highest_pitch = higher_pitch;
		}
	}




//====== Control Functions ======
	public void pause(){
		//Pause the conductor from generating more music
		GameObject.Find("conductor").GetComponent<CreateNoteGenerator>().pause();

		//Pause all the musical notes on screen
		GameObject[] notes = GameObject.FindGameObjectsWithTag ("MusicalNote");
		foreach (GameObject note in notes) {
			//Pause
			note.GetComponent<Rigidbody2D>().velocity = new Vector2 (0, note.transform.position.y);
		}

		//Stop the pitchline from detecting
		GameObject.Find ("pitch_line").GetComponent<DetectNote> ().disableDetection ();

	}

	public void resume(){
		GameObject pitchline = (GameObject)GameObject.Find ("pitch_line");
		pitchline.GetComponent<DetectNote> ().enableDetection ();
	}

	public void stop(){
		//Stop the conductor from making more music
		GameObject.Find("conductor").GetComponent<CreateNoteGenerator>().stop();

		//Destroy all notes on the screen
		GameObject[] notes = GameObject.FindGameObjectsWithTag ("MusicalNote");
		foreach (GameObject note in notes) {
			Destroy(note);
		}
	}

	public void exitGameWindow(){
		SceneManager.LoadScene ("Home Page");
	}
}
