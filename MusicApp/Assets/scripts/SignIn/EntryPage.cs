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

		public void login_submit (string sceneName) {
			FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync (email.text, password.text).ContinueWith((obj) => {

				Firebase.Auth.FirebaseUser user;
				user = obj.Result;
				if (user != null) {
					SceneManager.LoadSceneAsync ("userInfo");
				} else {
					SceneManager.LoadSceneAsync ("test");
				}
			});

		}

		public void register_page (string sceneName) {
			SceneManager.LoadScene (sceneName);
		}

		public void info_page (string sceneName) {
			SceneManager.LoadScene (sceneName);
		}

	
	}
}
