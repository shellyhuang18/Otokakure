using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PitchLine{
	//The script is used to check if the arrow is interacting with a MusicalNote.
	public class DetectNote : MonoBehaviour {

		//Colliders of the arrow and line gameobjects
		Collider2D arrow_collider;
		Collider2D line_collider; 

		private bool detection_enabled = true; //A bool to check if you want to detect notes

		private int hit = 0;
		private int miss = 0;


		// Use this for initialization
		void Start () {
			arrow_collider = (Collider2D)GameObject.Find ("arrow").GetComponent<PolygonCollider2D>();
			line_collider = (Collider2D)GameObject.Find ("pitch_line").GetComponent<BoxCollider2D> ();
		}
		
		// Update is called once per frame
		void Update () {
			checkOnPitch ();
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
			

		private void checkOnPitch(){
			if (detection_enabled) {
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

		//Checks if the line's collider is touching any of the note's collider
		private bool isLineTouchingNote(){
			GameObject[] spawned_notes = GameObject.FindGameObjectsWithTag ("MusicalNote");
			foreach (GameObject note in spawned_notes) {
				if (line_collider.IsTouching (note.GetComponent<BoxCollider2D> ())){
					return true;
				}
			}

			// If the loop didn't find break yet, then it must be a miss.

			return false;
		}

		//Checks if the arrow'collider is touching any of the note's collider
		private bool isArrowTouchingNote(){
			//Checks if the arrow is touching atleast one of the spawned notes

			GameObject[] spawned_notes = GameObject.FindGameObjectsWithTag ("MusicalNote");
			foreach (GameObject note in spawned_notes) {
				BoxCollider2D note_collider = note.GetComponent<BoxCollider2D> ();
				if (arrow_collider.IsTouching (note_collider) && line_collider.IsTouching(note_collider)) {
					return true;
				}
			}
			// If the loop didn't find break yet, then it must be a miss.
			return false;

		}
			
		public void enableDetection(){
			this.detection_enabled = true;
		}

		public void disableDetection(){
			this.detection_enabled = false;
		}
	}
}
