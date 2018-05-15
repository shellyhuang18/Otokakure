//Contributers
// - Sacit Gonen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Firebase.Database;
using Module;
using Manager = Communication.Manager;

//This class sets up the practice page of the app. In this page different exercises are generated based on user's level. 
public class Practice : MonoBehaviour {
	[SerializeField]
	private bool read_user_data; 

	public GameObject sample_button;
	public Button temporary;
	public Transform content_panel;
	public GameObject header;
	public Text practice_text;
	public string input = "";
	public string title;
	List<string> interval_list = new List<string> (new string[] {"Unison", "Minor 2nd", "Major 2nd","Minor 3rd", "Major 3rd",
		"Perfect 4th", "Perfect 5th", "Minor 6th", "Major 6th", "Minor 7th", "Major 7th", "Octave"});

	void Start () {
		header.SetActive (false);
		GetMicrophone ();
	}

	public static Rect pause_window;
	[SerializeField]
	private bool isClicked;
	private bool window_enabled;
	//gui function- anything gui related implement here
	void OnGUI(){
		if (isClicked && window_enabled) {
			//implement pause window
			GameObject canvas = GameObject.Find ("Canvas");
			Vector2 canvas_coords = canvas.transform.position;


			pause_window = new Rect ((float)(canvas_coords.x/2), (float)(canvas_coords.y/2), canvas_coords.x, canvas_coords.y);
			pause_window.center = new Vector2(Screen.width/2,Screen.height/2);
			GUIContent content = new GUIContent ();
			content.text = title;
			pause_window = GUI.ModalWindow (0, pause_window, WindowAction, content);
		}
	}
	//operations on pop up window
	void WindowAction(int windowID){
		Rect input_location = new Rect (pause_window.x/2, pause_window.y/2, pause_window.width/2, pause_window.height/5);
		Rect done_button_location = new Rect (input_location.x, input_location.y + 130, pause_window.width/2, pause_window.height/5);
		input = GUI.TextField (input_location, input);
		GUI.skin.textField.fontSize = 40;
		GUI.skin.textField.alignment = TextAnchor.MiddleCenter;
		GUI.skin.button.fontSize = 40;

		//OnClick listener 
		if (GUI.Button (done_button_location, "Done") ) {
			parseFormAndStartSession ();
		}
	}

	//On Ios applications, this method is needed to ask the user permission to use the microphone. 
	IEnumerator GetMicrophone()
	{
		yield return Application.RequestUserAuthorization (UserAuthorization.Microphone);
		if (Application.HasUserAuthorization (UserAuthorization.Microphone)) {
			Debug.Log ("We received the mic");
			//StartRecording
		} else {
			Debug.Log ("We encountered an error");
			//Error
		}
	}

	//Button for home page
	public void GoToHomePage (string scene_name) {
		SceneManager.LoadScene (scene_name);
	}

	//Button for user info
	public void GoToUserInfoPage (string scene_name) {
		PlayerPrefs.SetString ("lastLoadedScene", SceneManager.GetActiveScene ().name);
		SceneManager.LoadScene (scene_name);
	}

	public void LoadPitchPractices () {
		header.SetActive (false);
		MapButtons ("Pitch");
	}

	public void LoadIntervalPractices () {
		header.SetActive (true);
		practice_text.text = "Multiple Interval Practices";
		MapButtons ("Intervals");
	}

	//Switches interval selection from single to multiple or vice versa
	public void SingleOrMultiple () {
		int amount = content_panel.childCount;
		bool set_to2 = content_panel.GetChild (0).GetComponent<ButtonTemplate> ().button.IsActive();
		bool set_to = !set_to2;
		Debug.Log ("set_to " + set_to);
		Debug.Log ("set_to2" + set_to2);
		if (set_to2) {
			temporary.gameObject.SetActive(true);
			practice_text.text = "Single Interval Practice";
		} else {
			temporary.gameObject.SetActive(false);
			practice_text.text = "Multiple Interval Practice";
		}
		for (int i = 0; i < amount-1; i++) {
			content_panel.GetChild (i).GetComponent<ButtonTemplate> ().button.gameObject.SetActive (set_to);
			content_panel.GetChild (i).GetComponent<ButtonTemplate> ().check.gameObject.SetActive (set_to2);

		}
	}

	public void Done(string id) {
		isClicked = true;
		window_enabled = true;
		title = id;
	}
		
	//Creates buttons for intervals
	void MapButtons(string practice_type) {
		content_panel.DetachChildren ();
		if (practice_type == "Pitch") {
			isClicked = true;
			window_enabled = true;
			title = "Pitch";
		} else if (practice_type == "Intervals") {
			foreach (var practices in interval_list) {
				GameObject newButton = Instantiate(sample_button) as GameObject;
				ButtonTemplate button_script = newButton.GetComponent<ButtonTemplate>();
				button_script.text.text = practices;
				button_script.button.onClick.AddListener (delegate {Done(practices);} );
				button_script.check.isOn = true;
				button_script.check.gameObject.SetActive (false);
				newButton.transform.SetParent(content_panel);
			}
			temporary.transform.SetParent (content_panel);
			temporary.gameObject.SetActive(false);
		} else {
		}
	}

	private void parseFormAndStartSession(){
		StartCoroutine (parsingFormCoroutine ());
	}

	private IEnumerator parsingFormCoroutine(){
		string user_lowest_pitch = "c3"; //Let these be default values
		string user_highest_pitch = "c4";

		if (read_user_data) {
			DataAnalytics.QuerySearch low_pitch_search = new DataAnalytics.QuerySearch ("LowerRange");
			DataAnalytics.QuerySearch high_pitch_search = new DataAnalytics.QuerySearch ("HigherRange");

			//While the database is still querying
			while (low_pitch_search.querying && high_pitch_search.querying) {
				yield return new WaitForSeconds (0.01f);
			}

			user_lowest_pitch = low_pitch_search.query_result;
			user_highest_pitch = high_pitch_search.query_result;
		}

		int reps = 0;
		BaseModule module = new Module.PitchModule (lowest_pitch: user_lowest_pitch, highest_pitch:user_highest_pitch); //Let this be the default



		//Determining which module they selected
		if (title == "Pitch")
			module = new Module.PitchModule (lowest_pitch: user_lowest_pitch, highest_pitch: user_highest_pitch);
		int itr = 0;
		foreach (var practices in interval_list) {
			if (title == practices) {
				int[] array = {itr};
				List<int> interval_selection = new List<int>(array);
				module = new Module.IntervalModule(intervals: interval_selection, lowest_pitch: user_lowest_pitch, highest_pitch: user_highest_pitch);
				break;
			}
			itr += 1;
		}


		if(int.TryParse(input, out reps)){
			//if pressed on pitch
			window_enabled = false;
			Manager.addExercise (module, reps);
			Manager.transitionTo ("test");
		}
	}
}