using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor;
using PitchLine;
using Utility;
using UnityEngine.SceneManagement;
using Song = NoteLogic.NoteLogic.Song;
using Manager = Communication.Manager;
using HintLine;
using UnityEngine.UI;
using DataAnalytics;

public class GameWindow : MonoBehaviour {
	//UI Game Objects
	private GameObject pitchline;
	private GameObject conductor;
	private GameObject hintline;

	public static Rect pause_window;
	[SerializeField]
	private bool isPaused;
	private bool window_enabled;
	private bool songBeingPlayed;

	//Variables associated with the entire Game Window
	[SerializeField]
	private string lowest_pitch;
	[SerializeField]
	private string highest_pitch;
	[SerializeField]
	private float tempo;

	[SerializeField]
	private bool micEnabled;

	[SerializeField]
	private bool hintLineEnabled;

	[SerializeField]
	private bool exitWhenSongFinished;

	[SerializeField]
	private bool debug_tutorial_mode;

	[SerializeField]
	private bool read_user_data;

	Song current_song;

	// Use this for initialization
	void Start () {
		StartCoroutine (initalize ());
	}

	private IEnumerator initalize(){

		if (/*!debug_tutorial_mode &&*/ read_user_data) {
			DataAnalytics.QuerySearch low_pitch_search = new DataAnalytics.QuerySearch ("LowerRange");
			DataAnalytics.QuerySearch high_pitch_search = new DataAnalytics.QuerySearch ("HigherRange");


			//While the database is still querying
			while (low_pitch_search.querying && high_pitch_search.querying) {
				yield return new WaitForSeconds (0.01f);
			}

			lowest_pitch = low_pitch_search.query_result;
			highest_pitch = high_pitch_search.query_result;
		
		} else {
			lowest_pitch = "c2";
			highest_pitch = "c6";
		}


		Screen.orientation = ScreenOrientation.Landscape;
		pitchline = (GameObject)GameObject.Find ("pitch_line");
		conductor = (GameObject)GameObject.Find ("conductor");
		hintline = (GameObject)GameObject.Find ("hint_line");


		conductor.GetComponent<CreateNoteGenerator> ().instantiate ();
		pitchline.GetComponent<ControlArrow> ().instantiate ();

		pitchline.GetComponent<AudioSource> ().enabled = micEnabled;
		hintline.GetComponent<HintLineBehavior> ().setEnabled(hintLineEnabled);

		setSongPlayingStatus (false);

		if (Manager.getTutorialStatus () || debug_tutorial_mode) {
			string tutorial_sfs = "!!welcome !!sound !!hitline !!matchnote";
			Song tutorial = new Song (tutorial_sfs);
			current_song = tutorial;
			startSong (current_song);

			//			Manager.tutorialStatus = false;
		} else {
			current_song = Manager.generateSong ();
			startSong (current_song);
		}
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
			conductor.GetComponent<ConductorBehavior>().triggerPitch ("a4", 16);
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

			isPaused = true;
			window_enabled = true;
		}
		if (Input.GetKeyDown ("m")) {
			Song new_song = new Song ("4c#4 4d#4 4r 4d4 4d#4 !alert");
			conductor.GetComponent<ConductorBehavior>().startSong (new_song);
		}
		if (Input.GetKeyDown ("n")) {
			Song new_song = new Song ("4c#4 4d#4 4r 4d4 4d#4 !!alertEx");
			conductor.GetComponent<ConductorBehavior>().startSong (new_song);
		}
		if (Input.GetKeyDown ("s")) {
			stop ();
		}
		if (Input.GetKeyDown ("r")) {
			Debug.Log ("resume");
			resume ();
		}
		if (Input.GetKeyDown ("space")) {
			GameObject n = Instantiate (Resources.Load ("LoadingScreen/SceneTransition")) as GameObject;
			n.GetComponent<TransitionScene> ().startTransition ("main");
		}
	}
//
//	private IEnumerator startTutorial(){
//		string tutorial_sfs = "!!welcome !!sound";
//		Song tutorial = new Song (tutorial_sfs);
//		current_song = tutorial;
//		startSong (current_song);
//
//		//Wait for first song to finish
//
//		micEnabled = true;
//
//		Start
//
//
//	}


//====== Variable Mutators and Getters ======
	public string getLowestPitch(){
		return this.lowest_pitch;
	}

	public string getHighestPitch(){
		return this.highest_pitch;
	}

	public bool getMicStatus(){
		return this.micEnabled;
	}

	public void setHintLineActive(bool val){
		hintline.GetComponent<HintLineBehavior> ().setEnabled (val);
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
		
	public Song getCurrentSong(){
		return this.current_song;
	}

	public bool willExitOnCompletition(){
		return this.exitWhenSongFinished;
	}
		
	public bool songPlayingStatus(){
		return this.songBeingPlayed;
	}

	public void setSongPlayingStatus(bool val){
		this.songBeingPlayed = val;
	}

//====== Control Functions ======

	public void startSong(Song song){
		conductor.GetComponent<ConductorBehavior>().startSong (song);
	}

	public void pause(){
		isPaused = true;

		//Stop the pitchline from detecting
		pitchline.GetComponent<DetectNote> ().disableDetection ();

		//Pause the conductor from generating more music
		conductor.GetComponent<ConductorBehavior>().pause();
	}

	public void openPauseWindow(){
		isPaused = true;
		window_enabled = true;
	}

	//gui function- anything gui related implement here
	void OnGUI(){
		if (isPaused && window_enabled) {
			pause ();

			//change button's sprite to play
			GameObject pause_button = GameObject.Find ("Home Button");
			pause_button.GetComponent<Image> ().sprite = Resources.Load ("Buttons/play_button", typeof(Sprite)) as Sprite;

			//implement pause window
			GameObject canvas = GameObject.Find ("Canvas");
			Vector2 canvas_coords = canvas.transform.position;

			pause_window = new Rect((float)(canvas_coords.x/2), (float)(canvas_coords.y/2), 300, 200);

			GUIContent content = new GUIContent ();
			content.text = "Pause Menu";
			pause_window = GUI.ModalWindow (0, pause_window, WindowAction, content);
		}
	}
	//operations on pop up window
	void WindowAction(int windowID){
		
		Rect button = new Rect (100, 50, 100, 35);
		Rect home = new Rect (100, 100, 100, 35);
		//GUIContent butt = new GUIContent ();
		//butt.image = GameObject.Find ("arrow").GetComponent<SpriteRenderer> ().sprite.texture;
		if (GUI.Button (button, "Resume") ) {
			isPaused = false;
			window_enabled = false;
			resume ();
			//change button's sprite to pause
			GameObject pause_button = GameObject.Find ("Home Button");
			pause_button.GetComponent<Image> ().sprite = Resources.Load ("Buttons/pause_button", typeof(Sprite)) as Sprite;
		}
		if (GUI.Button (home, "Home") ) {
			window_enabled = false;

			exitSession ();
		}

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


	//Call this when preemptively leaving the game window. This resets stuff
	public void exitSession(){

		if (willExitOnCompletition ()) {
			Manager.clearQueue ();
			Manager.transitionTo ("Practice");
		}
	}
}
