using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Firebase.Database;

public class Practice : MonoBehaviour {

	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	public Text result;
	public GameObject sample_button;
	public Transform content_panel;


	void Start () {
		auth = Firebase.Auth.FirebaseAuth.GetAuth (FirebaseAuth.DefaultInstance.App);
		user = auth.CurrentUser;
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://music-learning-capstone-c019b.firebaseio.com");
	}

	public void GoToHomePage (string scene_name) {
		SceneManager.LoadScene (scene_name);
	}

	public void GoToUserInfoPage (string scene_name) {
		PlayerPrefs.SetString ("lastLoadedScene", SceneManager.GetActiveScene ().name);
		SceneManager.LoadScene (scene_name);
	}

	public void LoadPitchPractices () {
		RetrievePractices ("Pitch");
	}

	public void LoadRhythmPractices () {
		RetrievePractices ("Rhythm");
	}

	void RetrievePractices(string practice_type) {
		DatabaseReference practice_table = FirebaseDatabase.DefaultInstance.GetReference ("SFSs");
		if (user != null) {
			practice_table.Child (practice_type).GetValueAsync ().ContinueWith (task => {
				if (task.IsFaulted) {
					result.text = "error";
				} else if (task.IsCompleted) {
					result.text = "Success";
					DataSnapshot snap = task.Result;
					foreach (DataSnapshot variable in snap.Children) {
						GameObject newButton = Instantiate(sample_button) as GameObject;
						ButtonTemplate button_script = newButton.GetComponent<ButtonTemplate>();
						button_script.button_text.text = practice_type + " " + variable.Key;
						newButton.transform.SetParent(content_panel);
					}
				}
			});
		} else
			result.text = "There is no user";
	}
}