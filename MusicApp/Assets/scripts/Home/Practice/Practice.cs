using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Firebase.Database;

//This class sets up the practice page of the app. In this page different exercises are generated based on user's level. 
public class Practice : MonoBehaviour {
	public GameObject sample_button;
	public Button temporary;
	public Transform content_panel;
	public GameObject header;
	public Text practice_text;
	public string input = "";
	List<string> interval_list = new List<string> (new string[] {"Unison", "Minor 2nd", "Major 2nd","Minor 3rd", "Major 3rd",
		"Perfect 4th", "Perfect 5th", "Minor 6th", "Major 6th", "Minor 7th", "Major 7th", "Octave"});


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

			pause_window = new Rect((float)(canvas_coords.x/2), (float)(canvas_coords.y/2), 900, 600);
			GUIContent content = new GUIContent ();
			content.text = "Enter Amount";
			pause_window = GUI.ModalWindow (0, pause_window, WindowAction, content);
		}
	}
	//operations on pop up window
	void WindowAction(int windowID){
		Rect input_location = new Rect (100, 100, 700, 80);
		Rect done_button_location = new Rect (100, 500, 700, 80);
		input = GUI.TextField (input_location, input);
		GUI.skin.textField.fontSize = 40;
		GUI.skin.textField.alignment = TextAnchor.MiddleCenter;
		GUI.skin.button.fontSize = 40;
		if (GUI.Button (done_button_location, "Done") ) {
			SceneManager.LoadScene ("Daily");
		}
	}


	void Start () {
		header.SetActive (false);
		GetMicrophone ();
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


	public void GoToHomePage (string scene_name) {
		SceneManager.LoadScene (scene_name);
	}

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

	public void SingleOrMultiple () {
		int amount = content_panel.childCount;
		bool set_to2 = content_panel.GetChild (0).GetComponent<ButtonTemplate> ().button.IsActive();
		bool set_to = !set_to2;
		Debug.Log ("set_to " + set_to);
		Debug.Log ("set_to2" + set_to2);
		if (set_to2) {
			temporary.gameObject.SetActive(true);
		} else {
			temporary.gameObject.SetActive(false); 
		}
		for (int i = 0; i < amount-1; i++) {
			content_panel.GetChild (i).GetComponent<ButtonTemplate> ().button.gameObject.SetActive (set_to);
			content_panel.GetChild (i).GetComponent<ButtonTemplate> ().check.gameObject.SetActive (set_to2);

		}
	}

	public void Done() {
		isClicked = true;
		window_enabled = true;
	}
		

	public Button.ButtonClickedEvent Selection;

	void MapButtons(string practice_type){
		content_panel.DetachChildren ();
		if (practice_type == "Pitch") {
			isClicked = true;
			window_enabled = true;
		} else if (practice_type == "Intervals") {
			foreach (var practices in interval_list) {
				GameObject newButton = Instantiate(sample_button) as GameObject;
				ButtonTemplate button_script = newButton.GetComponent<ButtonTemplate>();
				button_script.text.text = practices;
				button_script.button.onClick = Selection;
				button_script.check.isOn = true;
				button_script.check.gameObject.SetActive (false);
				newButton.transform.SetParent(content_panel);
			}
			temporary.transform.SetParent (content_panel);
			temporary.gameObject.SetActive(false);
		} else {
		}
	}
}