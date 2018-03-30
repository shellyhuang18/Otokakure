using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using Firebase.Unity.Editor;


namespace SignIn{
	public class EntryPage : MonoBehaviour {

		public InputField email;
		public InputField password;
		Firebase.Auth.FirebaseAuth auth;
		Firebase.Auth.FirebaseUser user;

		public Text testing_for_reset;

		void Start () {
			auth = Firebase.Auth.FirebaseAuth.GetAuth (FirebaseAuth.DefaultInstance.App);
		}

		public void LoginSubmit (string scene_name) {
			FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync (email.text, password.text).ContinueWith(obj => {
				user = obj.Result;
				if (user != null) {
					SceneManager.LoadSceneAsync ("userInfo");
				} else {
					SceneManager.LoadSceneAsync ("test");
				}
			});
		}

		public void Reset() {
			auth.SendPasswordResetEmailAsync(email.text.ToString()).ContinueWith(authTask => {
				if (authTask.IsFaulted) {
					testing_for_reset.text = "Error Occured!";
				} else if (authTask.IsCompleted) {
					testing_for_reset.text = "Check Your email!";
				}
			});
		}

		public void RegisterPage (string scene_name) {
			SceneManager.LoadScene (scene_name);
		}

		public void InfoPage (string scene_name) {
			SceneManager.LoadScene (scene_name);
		}

	
	}
}
