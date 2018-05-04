using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Firebase.Database;

//This class sets up the practice page of the app. 
public class Practice : MonoBehaviour {
	public GameObject sample_button;
	public Transform content_panel;
	public GameObject header;
	public Text practice_text;
	List<string> interval_list = new List<string> (new string[] {"Unison", "Minor 2nd", "Major 2nd","Minor 3rd", "Major 3rd",
													"Perfect 4th", "Perfect 5th", "Minor 6th", "Major 6th", "Minor 7th", "Major 7th", "Octave"});

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

	public Button.ButtonClickedEvent Selection;

	void MapButtons(string practice_type){
		content_panel.DetachChildren ();
		if (practice_type == "Pitch") {
			//alert dialog which leads to game window page
		} else if (practice_type == "Intervals") {
			foreach (var practices in interval_list) {
				GameObject newButton = Instantiate(sample_button) as GameObject;
				ButtonTemplate button_script = newButton.GetComponent<ButtonTemplate>();
				button_script.button_text.text = practices;
				button_script.button.onClick = Selection;
				newButton.transform.SetParent(content_panel);
			}
		} else {
		}
	}
	/*
	void MapButtons(string practice_type) {
		content_panel.DetachChildren ();
		DatabaseReference practice_table = FirebaseDatabase.DefaultInstance.GetReference ("User Table");
		if (user != null) {
			practice_table.Child (user.UserId).Child (practice_type).GetValueAsync ().ContinueWith (task => {
				if (task.IsFaulted) {
					practice_text.text = "error";
				} else if (task.IsCompleted) {
					DataSnapshot snap = task.Result;
					foreach (DataSnapshot variable in snap.Children) {
						GameObject newButton = Instantiate(sample_button) as GameObject;
						ButtonTemplate button_script = newButton.GetComponent<ButtonTemplate>();
						button_script.button_text.text = variable.Value.ToString();
						button_script.button.onClick = Selection;
						newButton.transform.SetParent(content_panel);
					}
				}
			});
		} else
			practice_text.text = "There is no user";
	}
	*/
}