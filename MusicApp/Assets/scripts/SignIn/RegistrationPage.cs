using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using Firebase.Unity.Editor;
using Firebase.Database;


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
					AddUser(user.UserId);
					SceneManager.LoadSceneAsync (sceneName);
				}
				});
		}

		void AddUser(string id) {
			DatabaseReference user_table = FirebaseDatabase.DefaultInstance.RootReference;
			if (user != null) {
				string json = JsonUtility.ToJson (id);
				user_table.Child ("User Table").Push ().SetRawJsonValueAsync (json);
			}
		}
	}
}
