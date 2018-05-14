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
		private int current_session_hits;
		private int current_session_misses;

		private int overall_score;

		private int overall_hits;
		private int overall_possible;

		Firebase.Auth.FirebaseAuth auth;
		Firebase.Auth.FirebaseUser user;
		DatabaseReference user_table;

		//gets reference to database and grabs corresponding values
		public DataAnalysis(){
			this.current_session_hits = 0;
			this.current_session_misses = 0;

			this.overall_score = 0;

			this.overall_hits = 0;
			this.overall_possible = 0;


			auth = Firebase.Auth.FirebaseAuth.GetAuth (FirebaseAuth.DefaultInstance.App);
			user = auth.CurrentUser;
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://music-learning-capstone-c019b.firebaseio.com");
			user_table = FirebaseDatabase.DefaultInstance.GetReference ("User Table");
			if (user != null) {
				user_table.Child (user.UserId).Child ("Total Score").GetValueAsync ().ContinueWith (task => {
					if (task.IsFaulted) {
						//error
					} else if (task.IsCompleted) {
						DataSnapshot snap = task.Result;
						overall_score = System.Convert.ToInt32(snap.Value);
					}
				});

				user_table.Child (user.UserId).Child ("Overall Hits").GetValueAsync ().ContinueWith (task => {
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
			int new_overall_hit = this.current_session_hits + this.overall_hits;
			int new_overall_possible = this.current_session_hits + this.current_session_misses + this.overall_possible;
			double new_overall_accuracy = (double)new_overall_hit / (double)new_overall_possible;
			int new_overall_score = GameObject.Find ("game_window_UI").GetComponent<ScoreBoard> ().getScore () + overall_score;

			if (user != null) {
				user_table.Child(user.UserId).Child ("Overall Hits").SetValueAsync (System.Convert.ToInt64(new_overall_hit));

				user_table.Child(user.UserId).Child ("Overall Possible").SetValueAsync (System.Convert.ToInt64(new_overall_possible));

				user_table.Child(user.UserId).Child ("Overall Accuracy").SetValueAsync (System.Convert.ToDouble(new_overall_accuracy));


				user_table.Child (user.UserId).Child ("Total Score").SetValueAsync (System.Convert.ToInt64(new_overall_score));
			}
		}
			
			
		public void IncrementHits(){
			current_session_hits += 1;
		}

		public void IncrementMisses(){
			current_session_misses += 1;
		}

		public int GetHits(){
			return this.current_session_hits;
		}

		public int GetMisses(){
			return this.current_session_misses;
		}

		public void updateDatabase(string key, string value){
			if(user != null)
				user_table.Child(user.UserId).Child (key).SetValueAsync (value);
		}

		//Naseebs attempt to getfromdatabase
		public string getFromDatabaseAsync(string key){
			string val = "";
			if (user != null) {
				user_table.Child (user.UserId).Child (key).GetValueAsync ().ContinueWith (task => {
					if (task.IsFaulted) {
						//error
					} else if (task.IsCompleted) {
						DataSnapshot snap = task.Result;
						val = System.Convert.ToString(snap.Value);
					}
				});
			}
			return val;
		}



	} //End of DataAnalytics class

	//Acts as a wrapper for an ongoing operation. To use it, you should constantly check
	//to see if the QuerySearch.querying is done. Then you can access the QuerySearch.query_result directly.
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
				this.querying = false;
				Debug.Log ("The user is null");
			}

		}
	} //End of QuerySearch class
}
