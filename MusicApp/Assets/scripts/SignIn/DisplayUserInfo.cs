using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;

namespace SignIn {
	public class DisplayUserInfo : MonoBehaviour {
		
		public Text testing_display_email;
		Firebase.Auth.FirebaseAuth auth;
		Firebase.Auth.FirebaseUser user;

		void Start () {
			auth = Firebase.Auth.FirebaseAuth.GetAuth (FirebaseAuth.DefaultInstance.App);
			user = auth.CurrentUser;
		}

		void Update () {
			user = auth.CurrentUser;
			if (user != null) {
				testing_display_email.text = user.Email;
			} else {
				testing_display_email.text = "No users right now";
			}
		}

		public void ReturnToEntry (string scene_name){
			if (user != null) {
				auth.SignOut ();
			}
		}

		public void GoBack (string scene_name) {
			SceneManager.LoadScene (scene_name);
		}
	}
}