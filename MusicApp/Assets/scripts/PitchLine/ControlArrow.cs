using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace PitchLine{
	public class ControlArrow : MonoBehaviour {
		double height;
		double lower_bound;
	
		public string lowest_valid_pitch; 
		public string highest_valid_pitch; 
		private float lowest_valid_frequency;
		private float highest_valid_frequency;

		private float frequency_range;

		public void Start () {
			height = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
			lower_bound = gameObject.transform.position.y - height / 2;	

			//The parent should be the overall GameWindow object, which contains the PitchLine and Conductor
			//as children
			lowest_valid_pitch = gameObject.GetComponentInParent<GameWindow>().lowest_pitch;
			highest_valid_pitch = gameObject.GetComponentInParent<GameWindow>().highest_pitch;

			lowest_valid_frequency = Pitch.toFrequency (lowest_valid_pitch);
			highest_valid_frequency = Pitch.toFrequency (highest_valid_pitch);
			frequency_range = highest_valid_frequency - lowest_valid_frequency;

		}
		
		// Update is called once per frame
		void Update () {
			if(Input.GetKeyDown("space")){
				Debug.Log ("pressed space");
				moveArrow (440);
			}
			if(Input.GetKeyDown("b")){
				string pitch = "a1";
				while (true) {
					Debug.Log (pitch);
					moveArrow (Pitch.toFrequency (pitch));
					pitch = Pitch.incrementPitch (pitch, 1);
					if (pitch == "a5")
						break;
				}
			}
	
		}
			

		void moveArrow(double frequency){
			GameObject arrow = GameObject.Find ("arrow");

			//If pitch level is outside the range of the pitchline, unrender the arrow
			if (frequency > Pitch.toFrequency(highest_valid_pitch) || frequency < Pitch.toFrequency(lowest_valid_pitch)) {
				arrow.GetComponent<SpriteRenderer> ().enabled = false;
			} 
			else {
				arrow.GetComponent<SpriteRenderer> ().enabled = true;

				float frequency_fraction = (float) (1 + Math.Log ((frequency + (0.5*110))/highest_valid_frequency ,12) );
				Debug.Log (frequency + " " + frequency_fraction);

				float new_pos = (float)(frequency_fraction * height + lower_bound);
				arrow.transform.position = new Vector2 (arrow.transform.position.x, new_pos);
			}
		}
	}
}
