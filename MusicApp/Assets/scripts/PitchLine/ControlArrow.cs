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



		public void Start () {
			arrow = GameObject.Find ("arrow");
			height = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
			lower_bound = gameObject.transform.position.y - height / 2;	

			//The parent of the gameObject that controlarrow is connected to, should be GameWindow
			lowest_valid_pitch = gameObject.GetComponentInParent<GameWindow>().lowest_pitch;
			highest_valid_pitch = gameObject.GetComponentInParent<GameWindow>().highest_pitch;

			lowest_valid_frequency = Pitch.toFrequency (lowest_valid_pitch);
			highest_valid_frequency = Pitch.toFrequency (highest_valid_pitch);

			//Frequency of the lowest and highest pitch, +/- the frequency of a 50 cent offset.
			lowest_detectable_frequency = (float)(lowest_valid_frequency/ Math.Pow(2, 50.0/1200));
			highest_detectable_frequency = (float)(highest_valid_frequency/ Math.Pow(2, -50.0/1200));

		}
			

		public void Update(){
			//GameObject go = GameObject.Find("pitch_line");
			moveArrow (gameObject.GetComponent<PitchDetector>().pitch);
			Debug.Log("The frequency is " + gameObject.GetComponent<PitchDetector>().pitch);
		}
			

			
		public void moveArrow(double frequency){

			//If pitch level is outside the range of the pitchline, unrender the arrow
			if (frequency < this.lowest_detectable_frequency || this.highest_detectable_frequency < frequency ) {
				arrow.GetComponent<SpriteRenderer> ().enabled = false;
			} 
			else {
				arrow.GetComponent<SpriteRenderer> ().enabled = true;


				// + 1 to reflect the div_space offset. (Half offset on top and half offset on bottom)
				// 100 cents per half step.
				int total_cents = 100 * (Pitch.getTotalHalfSteps (lowest_valid_pitch, highest_valid_pitch) + 1);

				float pixel_per_cents = (float)height / total_cents;

				// An octave has 1200 cents 
				//This equation determines the offset in cents between the lowest freqency in the line, to the frequency
				float cents_change = (float)(1200 * Math.Log( frequency/this.lowest_detectable_frequency , 2));


				float new_pos = (float)lower_bound + (pixel_per_cents * cents_change);


				arrow.transform.position = new Vector2 (arrow.transform.position.x, new_pos);
			}
		}
	}
}
