using System.Collections;
using System.Collections.Generic;
using System;

namespace utility{
	public class utility{
		public static string[] SCALE_NOTES =  new string[]{"a", "a#", "b", "c","c#", "d", "d#", "e", "f", "f#", "g", "g#"};


		//Returns a string representation of a pitch incremented by n halfsteps.
		public static string incrementPitch(string pitch, int n){

			int octave = (int)Char.GetNumericValue(pitch [pitch.Length - 1]);
			string note = pitch.Substring(0, pitch.Length-1);

			int note_index = 0;
			//Initalize position of index
			for (int i = 0; i < 12; i++) {
				if (SCALE_NOTES [i] == note) {
					note_index = i;
					break;
				}
			}

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

			int octave = (int)Char.GetNumericValue(pitch [pitch.Length - 1]);
			string note = pitch.Substring(0, pitch.Length-1);

			int note_index = 0;
			//Initalize position of index
			for (int i = 0; i < 12; i++) {
				if (SCALE_NOTES [i] == note) {
					note_index = i;
					break;
				}
			}

			for(int i=0; i<n; i++){
				note_index = (note_index - 1);
				if (note_index == -1) {
					note_index = 11;
					octave -= 1;
				}
			}

			return(SCALE_NOTES[note_index] + octave);

		}

		//Returns the total amount of half steps between two pitches
		public static int get_total_half_steps(string lowest_pitch, string highest_pitch){
			//NOTE: Every pitch has two components: a note, and an octave. 
			//ex) c#5. c# is the note, 5 is the octave.


			//The last character of the string has the octave
			int start_octave = (int)Char.GetNumericValue(lowest_pitch[lowest_pitch.Length-1]); 
			int stop_octave = (int)Char.GetNumericValue(highest_pitch[highest_pitch.Length-1]); 

			//Stripping the octave component of the pitch, leaving behind the note
			string start_note = lowest_pitch.Substring (0, lowest_pitch.Length - 1);
			string stop_note = highest_pitch.Substring (0, highest_pitch.Length - 1);

			int total_half_steps = 0;

			//Iterate up the scale, until we find the correct pitch and octave
			bool found_stopping_pitch = false;
			int index_itr = 0;
			for (int i = 0; i < 12; i++) {
				if (SCALE_NOTES [i] == start_note)
					index_itr = i;
			}
			int octave_itr = start_octave; 

			while (!found_stopping_pitch) {
				if ((SCALE_NOTES [index_itr % 12] == stop_note) && (octave_itr == stop_octave)) {
					found_stopping_pitch = true;
				} else {
					total_half_steps += 1;
					index_itr += 1;
					if (index_itr % 12 == 0) { //If we completed one scale, increase octave
						octave_itr += 1;
					}
				}
			}

			return total_half_steps;
		}
	}
}