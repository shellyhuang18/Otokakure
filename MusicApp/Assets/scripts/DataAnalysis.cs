using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Firebase.Database;

namespace DataAnalytics{
	//Class DataAnalysis analyzes the user performance during a session and stores scores on to the database
	public class DataAnalysis: MonoBehaviour{
		public int hits;
		public int misses;
		private double overall_accuracy;
		private int current_session_hits;
		private int current_session_possible;
		private int overall_hits;
		private int overall_possible;
		Firebase.Auth.FirebaseAuth auth;
		Firebase.Auth.FirebaseUser user;
		DatabaseReference user_table;

		//gets reference to database and grabs corresponding values
		public DataAnalysis(){
			this.hits = 0;
			this.misses = 0;
			this.overall_accuracy = 0;
			this.overall_hits = 0;
			this.overall_possible = 0;

			auth = Firebase.Auth.FirebaseAuth.GetAuth (FirebaseAuth.DefaultInstance.App);
			user = auth.CurrentUser;
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://music-learning-capstone-c019b.firebaseio.com");
			user_table = FirebaseDatabase.DefaultInstance.GetReference ("User Table");
			if (user != null) {
				user_table.Child (user.UserId).Child ("Overall Accuracy").GetValueAsync ().ContinueWith (task => {
					if (task.IsFaulted) {
						//error
					} else if (task.IsCompleted) {
						DataSnapshot snap = task.Result;
						overall_accuracy = System.Convert.ToDouble(snap.Value);
					}
				});

				user_table.Child (user.UserId).Child ("Total Score").GetValueAsync ().ContinueWith (task => {
					if (task.IsFaulted) {
						//error
					} else if (task.IsCompleted) {
						DataSnapshot snap = task.Result;
						overall_hits = System.Convert.ToInt32(snap.Value);
					}
				});

				user_table.Child (user.UserId).Child ("Overall Possible").GetValueAsync ().ContinueWith (task => {
					if (task.IsFaulted) {
						//error
					} else if (task.IsCompleted) {
						DataSnapshot snap = task.Result;
						overall_possible = System.Convert.ToInt32(snap.Value);
					}
				});
			}

		}

		//sets values for storing to database
		public void SetCurrentValues(){
			this.current_session_hits = hits;
			this.current_session_possible = hits + misses;
			this.overall_hits += this.current_session_hits;
			this.overall_possible += this.current_session_possible;
			this.overall_accuracy = this.overall_hits / this.overall_possible;
			Debug.Log ("Current session hits" + this.current_session_hits);
			Debug.Log ("current session possible" + this.current_session_possible);
			Debug.Log ("Overall hits" + this.overall_hits);
			Debug.Log ("Current session accuracy" + (double)current_session_hits / (double)current_session_possible);
			Debug.Log ("overall possible" + this.overall_possible);
			Debug.Log ("overall accuracy" + this.overall_accuracy);
			AddToDatabase ();
		}

		//gets reference to database and stores corresponding data to dataabse
		private void AddToDatabase(){
			if (user != null) {
				user_table.Child ("Overall Accuracy").Push ().SetValueAsync (overall_accuracy);
				user_table.Child ("Total Score").Push ().SetValueAsync (overall_hits);
				user_table.Child ("Overall Possible").Push ().SetValueAsync (overall_possible);
			}
		}
			
			
		public void IncrementHits(){
			hits += 1;
		}

		public void IncrementMisses(){
			misses += 1;
		}

		public int GetHits(){
			return this.hits;
		}

		public int GetMisses(){
			return this.misses;
		}

		public void updateDatabase(string key, string value){
			user_table.Child (key).Push ().SetValueAsync (value);
		}

		//Naseebs attempt to getfromdatabase
//		public string getFromDatabase(string key){
//			string val = "red";
//			if (user != null) {
//				user_table.Child (user.UserId).Child (key).GetValueAsync ().ContinueWith (task => {
//					if (task.IsFaulted) {
//						//error
//					} else if (task.IsCompleted) {
//						DataSnapshot snap = task.Result;
//						val = System.Convert.ToString(snap.Value);
//					}
//				});
//			}
//			return val;
//		}



	} //End of DataAnalytics class

	//Acts as a wrapper for an ongoing operation. To use it, you should constantly check
	//to see if the querying is done. Then you can access the query_result directly.
	public class QuerySearch: MonoBehaviour{
		public bool querying = false;
		public string query_result = "";

		Firebase.Auth.FirebaseAuth auth;
		Firebase.Auth.FirebaseUser user;
		DatabaseReference user_table;

		public QuerySearch(string key){
			this.auth = Firebase.Auth.FirebaseAuth.GetAuth (FirebaseAuth.DefaultInstance.App);
			this.user = auth.CurrentUser;
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://music-learning-capstone-c019b.firebaseio.com");
			this.user_table = FirebaseDatabase.DefaultInstance.GetReference ("User Table");

			//Start the query
			this.querying = true;
			if (user != null) {
				user_table.Child (user.UserId).GetValueAsync ().ContinueWith ((task) => {
					if (task.IsFaulted) {
					} else if (task.IsCompleted) {
						DataSnapshot snap = task.Result;
						foreach (DataSnapshot name in snap.Children) {
							if (name.Key.Equals (key)) {
								this.query_result = name.Value.ToString ();
								this.querying = false;
								break;
							}
						}
					}
				});
			} else {
				Debug.Log ("The user is null");
			}

		}
	} //End of QuerySearch class
}
