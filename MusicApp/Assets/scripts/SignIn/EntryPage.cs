//Contributor: Rubaiyat Rashid, Sacit Gonen, Naseeb Gafar

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Assets.SimpleAndroidNotifications;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

//Namespace for the sign in and registration authentication code
namespace SignIn{
	//This class executes every functionalities in the entry page which includes loggin in users, resetting passwords, if necessary, and leading to different scenes
	//such as the registration page. 
	public class EntryPage : MonoBehaviour {
		public InputField email;
		public InputField password;
		Firebase.Auth.FirebaseAuth auth;
		Firebase.Auth.FirebaseUser user;
		public GameObject window;
		public Text messageField;

		//On start of scene, check whether a user is logged in. If so, take user to a tutorial page. 
		void Start () {
			RegisterForNotif ();
			ScheduleNotification ();
			// Android Notification
			NotificationManager.Send(TimeSpan.FromSeconds(5), "Welcome To Our Music App", "Team 7 All Day Yoo!!!", new Color(1, 0.3f, 0.15f));
			// iOS Notification
			// schedule notification to be delivered in 10 seconds
			#if UNITY_IOS
			var n = new UnityEngine.iOS.LocalNotification();
			n.fireDate = DateTime.Now;
			n.alertAction = "MusicApp";
			n.alertBody = "Team 7 All Day Yooo!!!";
			n.applicationIconBadgeNumber = 99;
			n.hasAction = true;//?
			n.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.GregorianCalendar;
			n.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;
			n.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(n);
			#endif

			Screen.autorotateToLandscapeLeft = true;
			Screen.autorotateToLandscapeRight = true;
			Screen.autorotateToPortrait = false;
			Screen.autorotateToPortraitUpsideDown = false;
			Screen.orientation = ScreenOrientation.AutoRotation;
			auth = Firebase.Auth.FirebaseAuth.GetAuth (FirebaseAuth.DefaultInstance.App);
			user = auth.CurrentUser;

			if (user != null) {
				SceneManager.LoadScene ("Home Page");
			}
		}

		void Update(){
			#if UNITY_IOS
			if (UnityEngine.iOS.NotificationServices.localNotificationCount > 0) {
				Debug.Log(UnityEngine.iOS.NotificationServices.localNotifications[0].alertBody);
				UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
			}
			#endif
		}

		//If Login button is clicked by user, authenticates user email and password. If email and password are valid, user is taken to a tutorial page.
		//Otherwise, display some message
		public void LoginSubmit (string scene_name) {
			FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync (email.text, password.text).ContinueWith(obj => {
				if (obj.IsFaulted){
					Show("Wrong Email and/or Password. Try Again.");
				}
				user = obj.Result;
				if (user != null) {
					SceneManager.LoadSceneAsync ("Home Page"); //home page for now
				} else {
					//Toast something to user saying "Wrong Password/Email. Try Again."
					Show("Wrong Email and/or Password. Try Again.");
				}
			});
		}

		//If Forgot Password button is clicked by user, firebase will send an email to the given email.
		//User is informed to check email for further steps
		public void Reset() {
			auth.SendPasswordResetEmailAsync(email.text.ToString()).ContinueWith(authTask => {
				if (authTask.IsFaulted) {
					//do nothing
				} else if (authTask.IsCompleted) {
					//We will pop a message/toast to user saying "Check your email"
					Show("Check your email to reset password");
				}
			});
		}

		//If user clicks on Sign Up button, they will be taken to Register page where they can sign up with appropriate infos.
		public void RegisterPage (string scene_name) {
			SceneManager.LoadScene (scene_name);
		}

		public void Show (string message){
			messageField.text = message;
			window.SetActive (true);
		}

		public void Hide(){
			window.SetActive (false);
		}

		void RegisterForNotif()

		{
			#if UNITY_IOS
			UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert| UnityEngine.iOS.NotificationType.Badge |  UnityEngine.iOS.NotificationType.Sound);
			#endif


		}

		void ScheduleNotification()

		{

			// schedule notification to be delivered in 24 hours
			#if UNITY_IOS
			UnityEngine.iOS.LocalNotification notif = new UnityEngine.iOS.LocalNotification();

			notif.fireDate = DateTime.Now.AddSeconds(1);

			notif.alertBody = "Youve generated more coins! Come back and play!";

			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notif);

			#endif
		}
	}
}
