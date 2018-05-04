using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor;
using PitchLine;
using Utility;
using UnityEngine.SceneManagement;
using Song = NoteLogic.NoteLogic.Song;


public class GameWindow : MonoBehaviour {
	//UI Game Objects
	private GameObject pitchline;
	private GameObject conductor;

	[SerializeField]
	private bool isPaused;

	//Variables associated with the entire Game Window
	[SerializeField]
	private string lowest_pitch;
	[SerializeField]
	private string highest_pitch;
	[SerializeField]
	private float tempo;

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.Landscape;
		pitchline = (GameObject)GameObject.Find ("pitch_line");
		conductor = (GameObject)GameObject.Find ("conductor");

		string test_score;
		test_score = "";
		for (int i = 0; i < 45; i++) {
			test_score += "4a4 4b4 ";
		}
		Song test = new Song (test_score);
//
//
//		Song test = new Song("16a4 17a4");
		//conductor.GetComponent<ConductorBehavior> ().startSong(test);
//			conductor.GetComponent<ConductorBehavior>().triggerPitch ("c4", 4*120);
	}
		
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("`")) {
			conductor.GetComponent<ConductorBehavior>().triggerPitch ("c4", 4);
		}
		if (Input.GetKeyDown ("1")) {
			conductor.GetComponent<ConductorBehavior>().triggerPitch ("c#4", 4);
		}
		if (Input.GetKeyDown ("2")) {
			conductor.GetComponent<ConductorBehavior>().triggerPitch ("d4", 4);
		}
		if (Input.GetKeyDown ("3")) {
			conductor.GetComponent<ConductorBehavior>().triggerPitch ("d#4", 4);
		}
		if (Input.GetKeyDown ("4")) {
			conductor.GetComponent<ConductorBehavior>().triggerPitch ("e4", 4);
		}
		if (Input.GetKeyDown ("5")) {
			conductor.GetComponent<ConductorBehavior>().triggerPitch ("f4", 4);
		}
		if (Input.GetKeyDown ("6")) {
			conductor.GetComponent<ConductorBehavior>().triggerPitch ("f#4", 4);
		}
		if (Input.GetKeyDown ("7")) {
			conductor.GetComponent<ConductorBehavior>().triggerPitch ("g4", 4);
		}
		if (Input.GetKeyDown ("8")) {
			conductor.GetComponent<ConductorBehavior>().triggerPitch ("g#4", 4);
		}
		if (Input.GetKeyDown ("9")) {
			conductor.GetComponent<ConductorBehavior>().triggerPitch ("a4", 4);
		}
		if (Input.GetKeyDown ("0")) {
			conductor.GetComponent<ConductorBehavior> ().triggerPitch ("a#4", 4);
		}
		if (Input.GetKeyDown ("-")) {
			conductor.GetComponent<ConductorBehavior>().triggerPitch ("b4", 4);
		}
		if (Input.GetKeyDown ("=")) {
			conductor.GetComponent<ConductorBehavior>().triggerPitch ("c5", 4);
		}

		if (Input.GetKeyDown ("a")) {
			Song new_song = new Song("4c#4 4d#4 4r 4d4 4d#4");
			conductor.GetComponent<ConductorBehavior>().startSong (new_song);
		}
		if (Input.GetKeyDown ("p")) {
			Debug.Log ("pause");
			pause ();
		}
		if (Input.GetKeyDown ("s")) {
			stop ();
		}
		if (Input.GetKeyDown ("r")) {
			Debug.Log ("resume");
			resume ();
		}
	}


//====== Variable Mutators and Getters ======
	public string getLowestPitch(){
		return this.lowest_pitch;
	}

	public string getHighestPitch(){
		return this.highest_pitch;
	}

	//Sets the tempo for the conductor
	public void setTempo(float tempo){
		this.tempo = tempo;
		conductor.GetComponent<ConductorBehavior> ().setTempo (tempo);
	}

	//Sets the entire range of pitches for this game window.
	public void setPitchRange(string lower_pitch, string higher_pitch){
		if(Utility.Pitch.isHigherPitch(lower_pitch, higher_pitch)){
			this.lowest_pitch = lower_pitch;
			this.highest_pitch = higher_pitch;
		}
	}

	public float getTempo(){
		return this.tempo;
	}

	public void setPauseStatus(bool status){
		this.isPaused = status;
	}

	public bool getPauseStatus(){
		return this.isPaused;
	}


//====== Control Functions ======

	public void pause(){
		isPaused = true;

		//Stop the pitchline from detecting
		pitchline.GetComponent<DetectNote> ().disableDetection ();

		//Pause the conductor from generating more music
		conductor.GetComponent<ConductorBehavior>().pause();
	}

	public void resume(){
		isPaused = false;

		//Pitchline continues detecting notes
		pitchline.GetComponent<DetectNote> ().enableDetection ();

		//Conductor continues generating it's song
		conductor.GetComponent<ConductorBehavior> ().resume ();
	}

	public void stop(){
		isPaused = false;

		//Stop the conductor from making more music
		conductor.GetComponent<ConductorBehavior>().stop();

		//Pitchline continues detecting notes
		pitchline.GetComponent<DetectNote> ().enableDetection ();
	}

	public void exitGameWindow(){
		//change data in database
		//call info page 
		SceneManager.LoadScene ("Home Page");
	}
}
