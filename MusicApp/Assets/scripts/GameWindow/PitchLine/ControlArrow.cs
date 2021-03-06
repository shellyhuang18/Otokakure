﻿//Contributor Jack Chen

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

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

		private float off_pitch_render_threshold = 1200; //The maximum amount of cent you can be off pitch before the arrow unrenders

		private PitchDetector pitch_detector;

		GameWindow game_window_script;



		public void instantiate () {
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

//			StartCoroutine (testLineIsWorking ());
		}
			

		//Just a test function to check that the arrows
		IEnumerator testLineIsWorking(){
			float a = Utility.Pitch.toFrequency ("c1");
			float b = Utility.Pitch.toFrequency ("c6");
			float diff = b - a;

			for (float i = a; i < diff; i++) {
				moveArrow (i);
				yield return new WaitForSeconds (0.01f);
			}

		}

		public void Update(){
			if (game_window_script != null && game_window_script.getMicStatus()) {
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

					//Make the arrow get increasingly transparent the more off pitch you are. (1200, an octave, is the max you can be off);
					float how_off_are_you = Utility.Pitch.hereisYourChange ((float)frequency, (float)lowest_detectable_frequency);
					float transparency_fraction = 1 - how_off_are_you/off_pitch_render_threshold;
					if (transparency_fraction < 0)
						transparency_fraction = 0;

					Color color = arrow.GetComponent<SpriteRenderer> ().material.color;
					color.a = transparency_fraction;
					arrow.GetComponent<SpriteRenderer> ().material.color = color;

				} 
				//If the pitch is too high, just set it to the bottom of the pitch line
				else if(frequency > this.highest_detectable_frequency ){
					arrow.transform.position = frequencyToPitchlinePosition (this.highest_detectable_frequency);

					//Make the arrow get increasingly transparent the more off pitch you are. (1200, an octave, is the max you can be off);
					float how_off_are_you = Utility.Pitch.hereisYourChange ((float)highest_detectable_frequency, (float)frequency);
					float transparency_fraction = 1 - how_off_are_you/off_pitch_render_threshold;
					if (transparency_fraction < 0)
						transparency_fraction = 0;

					Color color = arrow.GetComponent<SpriteRenderer> ().material.color;
					color.a = transparency_fraction;
					arrow.GetComponent<SpriteRenderer> ().material.color = color;
				}

				else {
					arrow.transform.position = frequencyToPitchlinePosition (frequency);
					Color color = arrow.GetComponent<SpriteRenderer> ().material.color;
					color.a = 1f;
					arrow.GetComponent<SpriteRenderer> ().material.color = color;
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
