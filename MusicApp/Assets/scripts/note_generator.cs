using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class note_generator : MonoBehaviour {
	string[] SCALE_NOTES =  new string[]{"a", "a#", "b", "c","c#", "d", "d#", "e", "f", "f#", "g", "g#"};
	float height; //The total absolute height of the thing.
	float lower_bound; //The lowest point of the note generating thing.
//	float higher_bound;
	float center; //The absolute center position of the note generating module

	// Use this for initialization
	void Start () {
		height = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
		lower_bound = gameObject.transform.position.y - height / 2;
		center = gameObject.transform.position.y;


	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (get_total_half_steps("g4","g4"));
		
	}

	void generateChildren(string lowest_pitch, string highest_pitch){
		//Determining how many children to create
		int total_children_to_create = get_total_half_steps(lowest_pitch, highest_pitch);

	}


	//Returns the total amount of half steps between two pitches
	int get_total_half_steps(string lowest_pitch, string highest_pitch){
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
