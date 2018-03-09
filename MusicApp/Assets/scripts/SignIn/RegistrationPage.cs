using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using UnityEngine;


namespace SignIn{
	public class RegistrationPage : MonoBehaviour {

		public InputField email;
		public InputField password;
		public InputField fname;
		public InputField lname;
			
		public void register_submit (string sceneName) {
			FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith((obj) => {
				SceneManager.LoadSceneAsync (sceneName);
				});
		}
	}
}
