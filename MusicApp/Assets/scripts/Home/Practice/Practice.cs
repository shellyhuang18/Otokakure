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
	public GameObject sample_button;
	public Transform content_panel;
	public Text result;
	public Text action;
	public Text practice_text;

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
		practice_text.text = "Practice Pitch";
		RetrievePractices ("Pitch");
	}

	public void LoadRhythmPractices () {
		practice_text.text = "Practice Rhythm";
		RetrievePractices ("Rhythm");
	}

	public void OnPracticeClicked (){
		
	}

	public Button.ButtonClickedEvent Selection;

	void RetrievePractices(string practice_type) {
		content_panel.DetachChildren ();
		DatabaseReference practice_table = FirebaseDatabase.DefaultInstance.GetReference ("User Table");
		if (user != null) {
			practice_table.Child (user.UserId).Child (practice_type).GetValueAsync ().ContinueWith (task => {
				if (task.IsFaulted) {
					result.text = "error";
				} else if (task.IsCompleted) {
					result.text = "Successfull retrievel";
					DataSnapshot snap = task.Result;
					foreach (DataSnapshot variable in snap.Children) {
						GameObject newButton = Instantiate(sample_button) as GameObject;
						ButtonTemplate button_script = newButton.GetComponent<ButtonTemplate>();
						button_script.button_text.text = variable.Value.ToString(); //practice_type + " " + variable.Key;
						button_script.to_do = variable.Value.ToString();
						button_script.button.onClick = Selection;
						newButton.transform.SetParent(content_panel);
					}
				}
			});
		} else
			result.text = "There is no user";
	}
}