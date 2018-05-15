using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Firebase.Database;

//Namespace for the sign in and registration authentication code
namespace SignIn {
	//This class displays useful information about the user to the user such as names, range and email.
	//It also gives the user to chance to log out of the account. 
	public class DisplayUserInfo : MonoBehaviour {
		Firebase.Auth.FirebaseAuth auth;
		Firebase.Auth.FirebaseUser user;
		public Text fname;
		public Text lname;
		public Text email;
		public Text hrange;
		public Text lrange;
		public Text overall_accuracy;
		public Text total_score;
		public Text overall_possible;
		//On start, user is retrieved from Firebase and the database is started. 
		void Start () {
			auth = Firebase.Auth.FirebaseAuth.GetAuth (FirebaseAuth.DefaultInstance.App);
			user = auth.CurrentUser;
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://music-learning-capstone-c019b.firebaseio.com");
			RetrieveUserInfo ();
		}

		//This method retrieves information from User table outputs to the screen. 
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
							if(name.Key == "OverallAccuracy"){
								if (name.Value.ToString().Length <= 4) {
									overall_accuracy.text = name.Value.ToString();
								} else {
									overall_accuracy.text = name.Value.ToString().Substring(0, 4);
								}
							}
							if(name.Key == "OverallHits"){
								total_score.text = name.Value.ToString();
							}
							if(name.Key == "OverallPossible"){
								overall_possible.text = name.Value.ToString();
							}
							if(name.Key == "TotalScore"){
								total_score.text = name.Value.ToString();
							}
						}
					}
				});
			}
		}

		//When Sign out is clicked, user is signed out of account and taken to entry page
		public void ReturnToEntry (string scene_name){
			if (user != null) {
				auth.SignOut ();
			}
			SceneManager.LoadScene (scene_name);
		}

		//If go back is clicked, user is taken to previous scene
		public void GoBack (string scene_name) {
			SceneManager.LoadScene (PlayerPrefs.GetString ("lastLoadedScene"));
		}
	}
}

//title: 19213EA7
//button colors:
//Normal : FFBDBDFF
// Highlighted : F98888FF
// Pressed : D27272FF
//textbox :000000FF