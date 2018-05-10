using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using PitchLine; //This namespace hold's Naseeb's pitch tracker stuff

namespace PitchLine{
	public class ControlArrow : MonoBehaviour {
		private GameObject arrow; //The arrow that is being manipulated
		
		private double height;
		private double lower_bound;
	
		private string lowest_valid_pitch;  
		private string highest_valid_pitch; 
		private float lowest_valid_frequency; //These are the frequencies of the SPECIFIED PITCHES
		private float highest_valid_frequency;

		private float lowest_detectable_frequency; //The max and min frequency that the PITCHLINE can detect
		private float highest_detectable_frequency;

		private PitchDetector pitch_detector;

		GameWindow game_window_script;

		public void Start () {
			arrow = GameObject.Find ("arrow");
			height = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
			lower_bound = gameObject.transform.position.y - height / 2;	

			//The parent of the gameObject that controlarrow is connected to, should be GameWindow
			lowest_valid_pitch = gameObject.GetComponentInParent<GameWindow>().getLowestPitch();
			highest_valid_pitch = gameObject.GetComponentInParent<GameWindow>().getHighestPitch();

			lowest_valid_frequency = Pitch.toFrequency (lowest_valid_pitch);
			highest_valid_frequency = Pitch.toFrequency (highest_valid_pitch);

			//Frequency of the lowest and highest pitch, +/- the frequency of a 50 cent offset.
			lowest_detectable_frequency = (float)(lowest_valid_frequency/ Math.Pow(2, 50.0/1200));
			highest_detectable_frequency = (float)(highest_valid_frequency/ Math.Pow(2, -50.0/1200));

			pitch_detector = gameObject.GetComponent <PitchDetector>();

			game_window_script = GameObject.FindGameObjectWithTag ("GameWindow").GetComponent<GameWindow>();

		}
			

		public void Update(){
			if (game_window_script.getMicStatus()) {
				moveArrow (pitch_detector.pitch);
			} else {
				moveArrow(440);
			}
		}
			

			
		public void moveArrow(double frequency){

			//The pitch detector returns a -1 when the volume is too low. 
			//Unrender the arrow when that happens
			if (frequency == -1) {
				arrow.GetComponent<SpriteRenderer> ().enabled = false;
				arrow.GetComponent<Collider2D> ().enabled = false;
			}
			else{
				arrow.GetComponent<SpriteRenderer> ().enabled = true;
				arrow.GetComponent<Collider2D> ().enabled = true;


				//If pitch is too low, just set it to the bottom of the pitch line
				if (frequency < this.lowest_detectable_frequency){
					arrow.transform.position = frequencyToPitchlinePosition (this.lowest_detectable_frequency);
				} 
				//If the pitch is too high, just set it to the bottom of the pitch line
				else if(frequency > this.highest_detectable_frequency ){
					arrow.transform.position = frequencyToPitchlinePosition (this.highest_detectable_frequency);
				}
				else {
					arrow.transform.position = frequencyToPitchlinePosition (frequency);
				}
			}
		}

		//Converts a frequency and determines it's position on the pitch line.
		private Vector2 frequencyToPitchlinePosition(double frequency){
			
			// Add + 1 for the half offset on top and half offset on bottom
			// 100 cents per half step.
			int total_cents = 100 * (Pitch.getTotalHalfSteps (lowest_valid_pitch, highest_valid_pitch) + 1);

			float pixel_per_cents = (float)height / total_cents;

			// An octave has 1200 cents 
			//This equation determines the offset in cents between the LOWEST FREQUENCY ON THE LINE, to the frequency given
			float cents_change = (float)(1200 * Math.Log( frequency/this.lowest_detectable_frequency , 2));


			float new_pos = (float)lower_bound + (pixel_per_cents * cents_change);


			return new Vector2 (arrow.transform.position.x, new_pos);


		}
			
			
	}
}
