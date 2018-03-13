using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using UnityEngine;


namespace SignIn{
	public class EntryPage : MonoBehaviour {

		public InputField email;
		public InputField password;


		public void login_submit (string sceneName) {
			FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync (email.text, password.text).ContinueWith((obj) => {
				SceneManager.LoadScene (sceneName);
			});
		}

		public void register_page (string sceneName) {
			SceneManager.LoadScene (sceneName);
		}

	
	}
}
