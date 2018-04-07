using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PitchLine{
	public class DetectNote : MonoBehaviour {
		static GameObject arrow; 
		static Collider2D arrow_collider;

		static Collider2D line_collider; //You need this to know if you missed

		private static int hit = 0;
		private static int miss = 0;

		private static bool enabled = false; //A bool to check if you want to detect notes

		// Use this for initialization
		void Start () {
			arrow = GameObject.Find ("arrow");
			arrow_collider = (Collider2D)arrow.GetComponent<PolygonCollider2D>();

			line_collider = (Collider2D)GameObject.Find ("pitch_line").GetComponent<BoxCollider2D> ();
		}
		
		// Update is called once per frame
		void Update () {
			checkOnPitch ();
		}


		public void checkOnPitch(){
			if (enabled) {
				if (!isLineTouchingNote ()) {
					//Do nothing if the note isnt up to the line yet
				} else {
					if (isArrowTouchingNote ()) {
						onHit ();

					} else {
						onMiss ();
					}

				}
			}
		}


		//Write what you want specifically to happen when there is a hit or miss here in this zone
		//=============================================================================
		private void onHit(){
			hit += 1;
			Debug.Log ("hit");
		}

		private void onMiss(){
			miss += 1;
			Debug.Log ("miss");
		}

		//=============================================================================
			
		private bool isLineTouchingNote(){
			GameObject[] spawned_notes = GameObject.FindGameObjectsWithTag ("MusicalNote");
			foreach (GameObject note in spawned_notes) {
				if (line_collider.IsTouching (note.GetComponent<BoxCollider2D> ())){
					return true;
				}
			}

			// If the foreach loop didn't find anything yet and break the function, then it must be a miss.

			return false;
		}

		private bool isArrowTouchingNote(){
			//Checks if the arrow is touching atleast one of the spawned notes

			GameObject[] spawned_notes = GameObject.FindGameObjectsWithTag ("MusicalNote");
			foreach (GameObject note in spawned_notes) {
				BoxCollider2D note_collider = note.GetComponent<BoxCollider2D> ();
				if (arrow_collider.IsTouching (note_collider) && line_collider.IsTouching(note_collider)) {
					return true;
				}
			}
			// If the foreach loop didn't find anything yet and break the function, then it must be a miss.

			return false;

		}
			
		public void enableDetection(){
			enabled = true;
		}

		public void disableDetection(){
			enabled = false;
		}
	}
}
