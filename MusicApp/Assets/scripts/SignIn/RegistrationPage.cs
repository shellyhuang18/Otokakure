using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Firebase.Database;

public class User {
	public string Name;
	public string Last;
	public int Level;

	public User() {
	}

	public User(string f_name, string l_name) {
		this.Name = f_name;
		this.Last = l_name;
		this.Level = 1;
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
				user_table.Child (user.UserId).SetRawJsonValueAsync (json);
			}
		}
	}
}