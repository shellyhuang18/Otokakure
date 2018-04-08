using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Firebase.Database;


public class Levels {
	public List<string> pitch;
	public List<string> rhythm;
};

public class User {
	public string FirstName;
	public string LastName;
	public string LowerRange;
	public string HigherRange;

	public User() {
	}

	public User(string f_name, string l_name) {
		this.FirstName = f_name;
		this.LastName = l_name;
		this.LowerRange = "a3";
		this.HigherRange = "a5";
	}
}

namespace SignIn{
	public class RegistrationPage : MonoBehaviour {
		Firebase.Auth.FirebaseAuth auth;
		Firebase.Auth.FirebaseUser user;
		public InputField email;
		public InputField password;
		public InputField fname;
		public InputField lname;
		public Levels levels;

		void Start() {
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://music-learning-capstone-c019b.firebaseio.com");
		}

		public void RegisterSubmit (string sceneName) {
			FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(obj => {
				if (obj.IsCompleted) {
					auth = Firebase.Auth.FirebaseAuth.GetAuth (FirebaseAuth.DefaultInstance.App);
					user = auth.CurrentUser;
					AddUser(fname.text, lname.text);
					SceneManager.LoadSceneAsync (sceneName);
				}
			});
		}
			
		private void AddUser(string f_name, string l_name) {
			User new_user = new User(f_name, l_name);
			DatabaseReference user_table = FirebaseDatabase.DefaultInstance.GetReference ("User Table");
			if (user != null) {
				string json = JsonUtility.ToJson(new_user);
				user_table = user_table.Child (user.UserId);
				user_table.SetRawJsonValueAsync (json);

			
				List<string> pitch = new List<string>(new string[] { "Intervals", "Undecided"});
				pitch.Add ("Undecided 2");

				foreach (var exercises in pitch) {
					user_table.Child ("Pitch").Push ().SetValueAsync (exercises);
				}

				List<string> rhythm = new List<string> (new string[] { "Undecided" });

				foreach (var exercises in rhythm) {
					user_table.Child ("Rhythm").Push ().SetValueAsync (exercises);
				}

			}
		}
	}
}