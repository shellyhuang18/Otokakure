using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Firebase.Database;

namespace SignIn {
	public class DisplayUserInfo : MonoBehaviour {
		Firebase.Auth.FirebaseAuth auth;
		Firebase.Auth.FirebaseUser user;
		public Text fname;
		public Text lname;
		public Text email;
		public Text hrange;
		public Text lrange;

		void Start () {
			auth = Firebase.Auth.FirebaseAuth.GetAuth (FirebaseAuth.DefaultInstance.App);
			user = auth.CurrentUser;
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://music-learning-capstone-c019b.firebaseio.com");
		}

		void Update() {
			user = auth.CurrentUser;
			RetrieveUserInfo ();
		}

		void RetrieveUserInfo() {
			DatabaseReference user_table = FirebaseDatabase.DefaultInstance.GetReference ("User Table");
			if (user != null) {
				email.text = user.Email;
				user_table.Child(user.UserId).GetValueAsync ().ContinueWith (task => {
					if (task.IsFaulted) {
						fname.text = "error";
						lname.text = "error";
					} else if (task.IsCompleted) {
						DataSnapshot snap = task.Result;
						foreach (DataSnapshot name in snap.Children){
							if (name.Key == "FirstName") {
								fname.text = name.Value.ToString();
							}
							if (name.Key == "LastName"){
								lname.text = name.Value.ToString();
							}
							if (name.Key == "HigherRange"){
								hrange.text = name.Value.ToString();
							}
							if (name.Key == "LowerRange"){
								lrange.text = name.Value.ToString();
							}
						}
					}
				});
			}
		}

		public void ReturnToEntry (string scene_name){
			if (user != null) {
				auth.SignOut ();
			}
			SceneManager.LoadScene (scene_name);
		}

		public void GoBack (string scene_name) {
			SceneManager.LoadScene (PlayerPrefs.GetString ("lastLoadedScene"));
		}
	}
}