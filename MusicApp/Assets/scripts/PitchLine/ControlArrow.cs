using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using PitchLine; //This namespace hold's Naseeb's pitch tracker stuff

namespace PitchLine{
	public class ControlArrow : MonoBehaviour {
		static GameObject arrow; //The arrow that is being manipulated
		
		static double height;
		static double lower_bound;
	
		public static string lowest_valid_pitch;  
		public static string highest_valid_pitch; 
		private static float lowest_valid_frequency;
		private static float highest_valid_frequency;
		private static float pitchV;

		public static float lowest_detected_frequency; //This is the lowest frequency on the pitch line.

		private static float frequency_range;

		public void Start () {
			arrow = GameObject.Find ("arrow");
			height = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
			lower_bound = gameObject.transform.position.y - height / 2;	

			//The parent should be the overall GameWindow object, which contains the PitchLine and Conductor
			//as children
			lowest_valid_pitch = gameObject.GetComponentInParent<GameWindow>().lowest_pitch;
			highest_valid_pitch = gameObject.GetComponentInParent<GameWindow>().highest_pitch;

			lowest_valid_frequency = Pitch.toFrequency (lowest_valid_pitch);
			highest_valid_frequency = Pitch.toFrequency (highest_valid_pitch);
			frequency_range = highest_valid_frequency - lowest_valid_frequency;

			lowest_detected_frequency = (float)(lowest_valid_frequency/ Math.Pow(2, 50/1200));

		}
			

		public void Update(){
			//		float pitch = (arrow.GetComponent<PitchTracker> ().AnalyzeSound ());
			PitchLine.ControlArrow.moveArrow (440);
			//			moveArrow (arrow.GetComponent<PitchTracker> ().AnalyzeSound ());
			//Debug.Log (arrow.GetComponent<PitchTracker> ().AnalyzeSound ());
			//			if (pitch > 0) {
			//				pitchV = pitch;
			//				moveArrow (pitchV);
			//				Debug.Log (pitchV);
			//			}

		}
			

			
		public static void moveArrow(double frequency){

			//If pitch level is outside the range of the pitchline, unrender the arrow
			if (frequency > Pitch.toFrequency(highest_valid_pitch) || frequency < Pitch.toFrequency(lowest_valid_pitch)) {
				arrow.GetComponent<SpriteRenderer> ().enabled = false;
			} 
			else {
				arrow.GetComponent<SpriteRenderer> ().enabled = true;


				// + 1 to reflect the div_space offset
				int total_cents = 100 * (Pitch.getTotalHalfSteps (lowest_valid_pitch, highest_valid_pitch) + 1);

				float pixel_per_cents = (float)height / total_cents;

				// An octave has 1200 cents. 100 cents per half step
				float cents_change = (float)(1200 * Math.Log( frequency/lowest_detected_frequency , 2));


				//We add pixels_per_cents*50 to reflect the half div_space offset of the notes
				float new_pos = (float)lower_bound + (pixel_per_cents * cents_change) + (pixel_per_cents * 50);


				arrow.transform.position = new Vector2 (arrow.transform.position.x, new_pos);
			}
		}
	}
}
