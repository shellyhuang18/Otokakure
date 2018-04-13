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

	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	public GameObject sample_button;
	public Transform content_panel;
	public Text practice_text;

	void Start () {
		auth = Firebase.Auth.FirebaseAuth.GetAuth (FirebaseAuth.DefaultInstance.App);
		user = auth.CurrentUser;
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://music-learning-capstone-c019b.firebaseio.com");
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
		practice_text.text = "Pitch Practices";
		RetrievePractices ("Pitch");
	}

	public void LoadRhythmPractices () {
		practice_text.text = "Rhythm Practices";
		RetrievePractices ("Rhythm");
	}

	public Button.ButtonClickedEvent Selection;

	void RetrievePractices(string practice_type) {
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
}