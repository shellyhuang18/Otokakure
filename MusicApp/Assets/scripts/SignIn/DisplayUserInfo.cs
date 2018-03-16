using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;

namespace SignIn {
	public class DisplayUserInfo : MonoBehaviour {
		public Text sacit;

		public void return_to_entry (string scenename){
			Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.GetAuth (FirebaseAuth.DefaultInstance.App);
			Firebase.Auth.FirebaseUser user = auth.CurrentUser;

			if (user != null) {
				sacit.text = "User is loggin out";
				auth.SignOut ();
			} else {
				sacit.text = "No users right now";
			}


			//if (user == null) {
				//sacit.text = "User signed out successfully ";
			//} else {
				//Debug.LogFormat (user.Email);
			//}
			//EntryPage entr = new EntryPage ();
			//entr.obj.SignOut();
			//user = user.GetComponent<SignIn.Entry

			//SceneManager.LoadScene (scenename);
		}
	}
}