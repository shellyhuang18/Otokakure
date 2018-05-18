//Contributor: Shelly Huang, Jack Chen

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System.Threading;
using Spawner;
using NoteLogic;
using Note = NoteLogic.NoteLogic.Note;
using Sound = NoteLogic.NoteLogic.Sound;
using Chord = NoteLogic.NoteLogic.Chord;
using Song = NoteLogic.NoteLogic.Song;
using UnityEngine.SceneManagement;

namespace Conductor{
	
	public class CreateNoteGenerator : MonoBehaviour {
		public GameObject note_spawner; //The child object that spawns notes
	

		float height; //The total absolute height of the conductor.
		float lower_bound; //The lowest point of the conductor
		public float div_space;


	 

		// Use this for initialization
		public void instantiate () {
			height = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
			lower_bound = gameObject.transform.position.y - height / 2;

			//The parent object should be the GameWindow.
			generateChildren (gameObject.GetComponentInParent<GameWindow>().getLowestPitch(), gameObject.GetComponentInParent<GameWindow>().getHighestPitch());

		}


		void generateChildren(string lowest_pitch, string highest_pitch){
			//Determining how many children to create
			int total_children_to_create = Pitch.getTotalHalfSteps(lowest_pitch, highest_pitch) + 1;

			//Create children and reposition them to fit note generator(rectangle)
			div_space = height / total_children_to_create;
			float interval = (float)0.5*div_space; //we want the first position to spawn at 1/2th a divspace

			string pitch_id = lowest_pitch;
			float color = 0;
			//how much the color will change between each note depending on how many pitches there are
			float gradient = 255/total_children_to_create;

			//chooses random color 
			System.Random rand = new System.Random ();
			int color_choice = rand.Next (1, 6);

			for (int i = 0; i < total_children_to_create; ++i) {
				GameObject new_child = (GameObject)Instantiate (note_spawner);
				//Designates what pitch the generated spawner is responsible for.
				new_child.GetComponent<GenerateNotes> ().setAssociatedPitch (pitch_id);
				new_child.name = pitch_id;


				//Reposition of associated game objects
				new_child.transform.parent = this.transform;
				new_child.transform.position = new Vector2 (this.transform.position.x, lower_bound + interval);
				interval += div_space;

				//changes color of notes being generated
				new_child.GetComponent<GenerateNotes> ().setColor(RandomColor (color, color_choice));
				color += gradient;

				pitch_id = Pitch.incrementPitch (pitch_id, 1);

			}

		}

		//A function that generates random colors for the notes.
		private Color RandomColor(float color, int color_choice){

			switch (color_choice) {
			case 1:
				return new Color (color / 255, 0, 25);//ok
			case 2:
				return new Color (25, color / 255, 0); //ok
			case 3:
				return new Color (color / 255, 0, 0);//ok
			case 4:
				return new Color (0, color / 255 , 20);//ok
			case 5:
				return new Color (30, color / 255, 20);//ok
			}

			return new Color (0, 0, 0);

		}
	}
}
