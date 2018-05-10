using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Utility{
	public class Pitch{
		public static string[] SCALE_NOTES =  new string[]{"c","c#", "d", "d#", "e", "f", "f#", "g", "g#", "a", "a#", "b"};
		public static string valid_pitch_expression = @"[a-g]{1}#?[1-9]{1}"; //A regular expression to represent the SCALE_NOTEs. Used for error checking.
		public enum interval{u, m1, M1, m2, M2, m3, M3, P4, P5, m6, M6, m7, M7, o};
		public static int TOTAL_NOTES = 12;

		//Returns the index of a note within SCALE_NOTES
		private static int getScaleIndex(string note){
			for (int i = 0; i < 12; i++) {
				if (SCALE_NOTES [i] == note)
					return i;
			}

			return -1; //Error if return -1
		}

		//Determines the amount of cent change between the lower TO the higher pitch
		public static float hereIsYourChange(string lower_pitch, string higher_pitch){
			float low_freq = Utility.Pitch.toFrequency (lower_pitch);
			float high_freq = Utility.Pitch.toFrequency (higher_pitch); 

			float cents_change = (float)(1200 * Math.Log( high_freq/low_freq, 2));
			return cents_change;

		}
			

		//Returns whether pitch b is greater than pitch a.
		public static bool isHigherPitch(string pitch_a, string pitch_b){
			//The last character of the string has the octave
			int a_octave = (int)Char.GetNumericValue(pitch_a[pitch_a.Length-1]); 
			int b_octave = (int)Char.GetNumericValue(pitch_b[pitch_b.Length-1]); 

			if (a_octave < b_octave) {
				return true;
			}
			else if (a_octave > b_octave) {
				return false;
			}

			//Assertion: Octaves are the same

			//Stripping the octave component of the pitch, leaving behind the note
			int note_a_index = getScaleIndex(pitch_a.Substring (0, pitch_a.Length - 1));
			int note_b_index = getScaleIndex(pitch_b.Substring (0, pitch_b.Length - 1));

			if (note_a_index < note_b_index ) {
				return true;
			}
			else if(note_a_index > note_b_index){
				return false;
			}


			//Assertion: Pitchs must be equivalent
			return false;

		}

		//Returns a string representation of a pitch incremented by n halfsteps.
		public static string incrementPitch(string pitch, int n){

			if (!ErrorCheck.isValidPitchFormat (pitch)) {
				Debug.Log ("Error in incrementPitch. Argument: string pitch");
				Debug.Break ();
			}
				
			int octave = (int)Char.GetNumericValue(pitch [pitch.Length - 1]);
			string note = pitch.Substring(0, pitch.Length-1);

			int note_index = getScaleIndex (note);

			for(int i=0; i<n; i++){
				note_index = (note_index + 1) % 12;
				if (note_index == 0) {
					octave += 1;
				}
			}
			return(SCALE_NOTES[note_index] + octave);

		}

		//Returns a string representation of a pitch decremented by n halfsteps.
		public static string decrementPitch(string pitch, int n){

			if (!ErrorCheck.isValidPitchFormat (pitch)) {
				Debug.Log ("Error in decrementPitch. Argument: string pitch");
				Debug.Break ();
			}

			int octave = (int)Char.GetNumericValue(pitch [pitch.Length - 1]);
			string note = pitch.Substring(0, pitch.Length-1);

			int note_index = getScaleIndex (note);

			for(int i=0; i<n; i++){
				note_index = (note_index - 1);
				if (note_index == -1) {
					note_index = 11;
					octave -= 1;
				}
			}

			return(SCALE_NOTES[note_index] + octave);

		}

		//Returns the total amount of half steps between two pitches. Returns -1 when invalid.
		public static int getTotalHalfSteps(string pitch_a, string pitch_b){
			//NOTE: Every pitch has two components: a note, and an octave. 
			//ex) c#5. c# is the note, 5 is the octave.

			if (!ErrorCheck.isValidPitchFormat (pitch_a) || !ErrorCheck.isValidPitchFormat (pitch_b)) {
				Debug.Log ("Error in get_total_half_steps() arguments");
				Debug.Break ();
			}

			//The last character of the string has the octave
			int start_octave = (int)Char.GetNumericValue(pitch_a[pitch_a.Length-1]); 
			int stop_octave = (int)Char.GetNumericValue(pitch_b[pitch_b.Length-1]); 

			//Stripping the octave component of the pitch, leaving behind the note
			string start_note = pitch_a.Substring (0, pitch_a.Length - 1);
			string stop_note = pitch_b.Substring (0, pitch_b.Length - 1);

			int index_itr = getScaleIndex (start_note);
			int octave_itr = start_octave; 


			if(isHigherPitch(pitch_a, pitch_b)){
				//Find half_steps in incrementing order
				bool found_stopping_pitch = false;
				int total_half_steps = 0;
				while (!found_stopping_pitch) {
					if ((SCALE_NOTES [index_itr] == stop_note) && (octave_itr == stop_octave)) {
						return total_half_steps;
					} 
					else {
						total_half_steps += 1;
						index_itr = (index_itr + 1) % 12;
						if (index_itr == 0) { //If we completed one scale, increase octave
							octave_itr += 1;
						}
					}
				}
			}
			else{	
				//Find half_steps in decrementing order
				bool found_stopping_pitch = false;
				int total_half_steps = 0;
				while (!found_stopping_pitch) {
					if ((SCALE_NOTES [index_itr % 12] == stop_note) && (octave_itr == stop_octave)) {
						return total_half_steps;
					} 
					else {
						total_half_steps -= 1;
						index_itr -= 1;
						if (index_itr == -1) { //If we completed one scale, increase octave
							octave_itr -= 1;
							index_itr = 11;
						}
					}
				}
			}

			return -1;

		}

		//Converts a pitch to frequency
		public static float toFrequency(string pitch){
			if (!ErrorCheck.isValidPitchFormat (pitch)) {
				Debug.Log ("Error in toFrequency(). Argument: string pitch");
				Debug.Break ();
			}

			int n = getTotalHalfSteps ("a4" , pitch);
			double c = 1.05946309436; //constant value, 2^(1/12)
			return 440 * (float)Math.Pow(c , n);
		}

		//Converts a frequeny to closest matching pitch
		public static string getNearestPitch(float pitchInHz) {
			// Base Note is A4 = 440
			float baseNote = 440;
			float pitchinHz = 440;
			// octaves from base -> octaves  =  log(base2)(freq/base).
			//double octavesFromBase = Mathf.Round(Mathf.Log(pitchInHz/baseNote, 2));
			double halfStepsFromBase = Mathf.Round (12 * Mathf.Log(pitchInHz/baseNote, 2));
			//return halfStepsFromBase;
			//double octavesFromBase = Math.round((Math.log((pitchInHz/baseNote))/Math.log(2)));
			// half steps = log2^12 (freq/base)
			//double halfStepsFromBase = (Math.log((pitchInHz/baseNote))/Math.log(a));
			// half steps from base -> half steps  =  12 * log(base2)(freq/base).
			//double halfStepsFromBase = Math.round(12 * (Math.log((pitchInHz/baseNote))/Math.log(2)));
			int letterNoteIndex = ((int) halfStepsFromBase % 12)-3;
			if (letterNoteIndex < 0)
				letterNoteIndex += 12;
			
			int numberNote;
			if (halfStepsFromBase < 3) {
				numberNote = (int)halfStepsFromBase / 12 + 4;
			} else {
				numberNote = (int)halfStepsFromBase / 12 + 5;
			}
			return SCALE_NOTES[letterNoteIndex] + numberNote;
//			/noteText.setText(letterNoteArray[letterNoteIndex] + numberNote);
		}

	}//end of class
}