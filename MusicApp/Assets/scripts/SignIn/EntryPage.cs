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

		public Firebase.Auth.FirebaseAuth obj;

		public void login_submit (string sceneName) {
			FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync (email.text, password.text).ContinueWith((obj) => {

				//FirebaseAuth auth = FirebaseAuth.unitypackage.GetAuth(obj);
				//auth.SignOut();

				if (obj.IsCanceled){
					return;
				}

				if (obj.IsFaulted){
					SceneManager.LoadSceneAsync ("test");
				}

				if (obj.IsCompleted){
					SceneManager.LoadScene ("userInfo");
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
