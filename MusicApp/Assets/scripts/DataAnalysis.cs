using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Firebase.Database;

namespace DataAnalytics{
	public class DataAnalysis{
		public int hits;
		public int misses;
		private double overall_accuracy;
		//private double individual_note_accuracy;
		//private int individual_note_hits;
		private int current_session_hits;
		private int current_session_possible;
		private int overall_hits;
		private int overall_possible;
		Firebase.Auth.FirebaseAuth auth;
		Firebase.Auth.FirebaseUser user;
		DatabaseReference user_table;


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

		/*
		public void CalculateOverall(){
			overall_accuracy = overall_hits / overall_possible;
			
		}
		*/

		public void SetCurrentValues(){
			this.current_session_hits = hits;
			this.current_session_possible = hits + misses;
			this.overall_hits += this.current_session_hits;
			this.overall_possible += this.current_session_possible;
			this.overall_accuracy = this.overall_hits / this.overall_possible;
			AddToDatabase ();
		}

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

		public string getFromDatabase(string key){
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
	}
}
