using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace PitchLine{
	public class DetectGameNote : MonoBehaviour {
		double height;
		double lower_bound;
		double higher_bound;

		public double div_space;
		private string lowest_valid_pitch; 
		private string highest_valid_pitch; 
		private int total_half_steps;

		void Start () {
			height = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
			lower_bound = gameObject.transform.position.y - height / 2;
			higher_bound = height + lower_bound;

			//The parent should be the overall GameWindow object, which contains the PitchLine and Conductor
			//as children
			lowest_valid_pitch = gameObject.GetComponentInParent<GameWindow>().lowest_pitch;
			highest_valid_pitch = gameObject.GetComponentInParent<GameWindow>().highest_pitch;

			//Make sure lowest and highest pitch is defined before total_half_steps
			total_half_steps = Utility.Pitch.getTotalHalfSteps (lowest_valid_pitch, highest_valid_pitch) + 1;

			//make sure total_half_steps is defined before div_space
			div_space = height / total_half_steps;

		}
		
		// Update is called once per frame
		void Update () {
			if(Input.GetKeyDown("space")){
				Debug.Log ("pressed space");
				setPitchLevel (440);
			}
		}
			

		void setPitchLevel(double frequency){
			GameObject arrow = GameObject.Find ("arrow");

			Debug.Log("div space is " + div_space);
			Debug.Log ("arrow is at " + arrow.transform.position.y);
			Debug.Log ("lower bound is " + lower_bound);
			//If pitch level is outside the range of the pitchline, unrender the arrow
//			if (frequency > highest_valid_pitch || frequency < lowest_valid_pitch) {
//				arrow.GetComponent<SpriteRenderer> ().enabled = false;
//			} 
//			else {
//				arrow.GetComponent<SpriteRenderer> ().enabled = true;

				//we add div_space/2 since we want the correct pitch to center on the note bar
				float new_pos = (float)(lower_bound +(div_space/2) + (div_space * 4));
			Debug.Log ("current y is " + arrow.transform.position.y);
			Debug.Log ("new y is " + new_pos);
				arrow.transform.position = new Vector2 (arrow.transform.position.x, new_pos);
//			}
		}
	}
}
