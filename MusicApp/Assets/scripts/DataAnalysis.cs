using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Firebase.Database;

namespace DataAnalytics{
	public class DataAnalysis{
		
		private double overall_accuracy;
		private double individual_note_accuracy;
		private int individual_note_hits;
		//private int individual_note_possible;
		private int current_session_hits;
		private int current_session_possible;
		private int overall_hits;
		private int overall_possible;
		Firebase.Auth.FirebaseAuth auth;
		Firebase.Auth.FirebaseUser user;
		DatabaseReference user_table;

		public DataAnalysis(){
			this.overall_accuracy = 0;
			this.individual_note_accuracy = 0;
			auth = Firebase.Auth.FirebaseAuth.GetAuth (FirebaseAuth.DefaultInstance.App);
			user = auth.CurrentUser;
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://music-learning-capstone-c019b.firebaseio.com");
			user_table = FirebaseDatabase.DefaultInstance.GetReference ("User Table");
			if (user != null) {
				user_table.Child (user.UserId).Child ("Overall Accuracy").GetValueAsync ().ContinueWith (task => {
					if (task.IsFaulted) {
						//practice_text.text = "error";
					} else if (task.IsCompleted) {
						DataSnapshot snap = task.Result;
						overall_accuracy = System.Convert.ToDouble(snap.Value);
					}
				});

				user_table.Child (user.UserId).Child ("Overall Hits").GetValueAsync ().ContinueWith (task => {
					if (task.IsFaulted) {
						//practice_text.text = "error";
					} else if (task.IsCompleted) {
						DataSnapshot snap = task.Result;
						overall_hits = System.Convert.ToInt32(snap.Value);
					}
				});

				user_table.Child (user.UserId).Child ("Overall Possible").GetValueAsync ().ContinueWith (task => {
					if (task.IsFaulted) {
						//practice_text.text = "error";
					} else if (task.IsCompleted) {
						DataSnapshot snap = task.Result;
						overall_possible = System.Convert.ToInt32(snap.Value);
					}
				});
			}

		}

		public void CalculateOverall(){
			overall_accuracy = overall_hits / overall_possible;
			
		}

		public void SetCurrentValues(int current_session_hits, int current_session_possible){
			this.current_session_hits = current_session_hits;
			this.current_session_possible = current_session_possible;
			this.overall_hits += this.current_session_hits;
			this.overall_possible += this.current_session_possible;
			this.overall_accuracy = this.overall_hits / this.overall_possible;
		}

		private void AddToDatabase(){
			if (user != null) {
				user_table.Child ("Overall Accuracy").Push ().SetValueAsync (overall_accuracy);
				user_table.Child ("Overall Hits").Push ().SetValueAsync (overall_hits);
				user_table.Child ("Overall Possible").Push ().SetValueAsync (overall_possible);
			}
		}
	}
}
