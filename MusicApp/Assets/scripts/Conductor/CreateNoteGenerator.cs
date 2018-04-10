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

namespace Conductor{
	public class CreateNoteGenerator : MonoBehaviour {
		public GameObject note_spawner; //The child object that spawns notes
	

		float height; //The total absolute height of the conductor.
		float lower_bound; //The lowest point of the conductor
		public float div_space;
		public float tempo;					

		// Use this for initialization
		public void Start () {
			height = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
			lower_bound = gameObject.transform.position.y - height / 2;

			//The parent object should be the GameWindow.
			generateChildren (gameObject.GetComponentInParent<GameWindow>().lowest_pitch, gameObject.GetComponentInParent<GameWindow>().highest_pitch);

		}
		
		// Update is called once per frame
		void Update () {
			if (Input.GetKeyDown ("a")) {
				Song new_song = new Song("4c#4 4d#4 4r 4d4 4d#4");
				StartCoroutine (startSong (new_song));
			}
		}

		public float getTempo(){
			return this.tempo;
		}

		public void setTempo(float tempo){
			this.tempo = tempo;
		}
		 

		IEnumerator startSong(Song new_song){
			int last_note_beat = 0;
			int curr_note_dur = 0;
			int metronome = 0;
			int last_note_dur = curr_note_dur;
			int checkpoint = 0;

			float single_beat_time = (tempo * 4) / 3600; //#16th notes / #minutes / #
			GameObject last_note = null;
			GameObject curr_note = null;

			foreach (Sound item in new_song.score) {
				//output chords
				if (item.is_chord) {

					Chord c = item as Chord;
					//output notes in chord
					foreach (Note i in c.notes) {
						if (i.pitch != "r") {
							triggerPitch (i.pitch, i.duration, metronome, i);
							curr_note_dur = i.duration; 
						}
					}
						

					while (metronome != checkpoint) {
						metronome++;
						yield return new WaitForSeconds (single_beat_time/*amount of time passed for one beat*/);
					}

				//outpuqt single notes
				} else {
					Note n = item as Note;
					curr_note_dur = n.duration; 
					checkpoint += n.duration;

					//keep on same note until amount of time has passed for former note to finish
					while (metronome != checkpoint) {
						metronome++;

						yield return new WaitForSeconds (single_beat_time/*amount of time passed for one beat*/);
					}

					                                                                                                                                                            

					//generate note

					//indicate if note is not a rest
					curr_note = triggerPitch (n.pitch, n.duration, metronome, n);

					if (n.pitch == "r") {
						

						Debug.Log ("rest");
						curr_note.GetComponent<SpriteRenderer> ().enabled = false;
						curr_note.GetComponent<BoxCollider2D> ().enabled = false;

					}

					if (last_note != null) {
						float curr_pos_x = curr_note.GetComponent<SpriteRenderer> ().bounds.min.x;
						float last_pos_x = last_note.GetComponent<SpriteRenderer> ().bounds.max.x;
						float difference = last_pos_x - curr_pos_x;

						//add difference to curr_note pos
						Vector2 move = new Vector2(difference+ curr_note.transform.position.x, curr_note.transform.position.y);
						curr_note.transform.position = move;
					}

				}

				last_note_dur = curr_note_dur;
				//on what beat the last note has generated
				last_note_beat = metronome;
				last_note = curr_note;

				metronome++;
				yield return new WaitForSeconds (single_beat_time);
			}
		}

		public void triggerPitch(string pitch, int duration){
			GameObject note_spawner = GameObject.Find (pitch);
			GameObject generated_note = note_spawner.GetComponent<GenerateNotes>().generateNote(duration);
		}

		public GameObject triggerPitch(string pitch, int duration, int birth_beat, Note note){
			GameObject note_spawner;
			if (pitch != "r") {
				note_spawner = GameObject.Find (pitch);
			} else {
				string default_pitch = GameObject.Find ("game_window").GetComponent<GameWindow> ().lowest_pitch;
				note_spawner = GameObject.Find (default_pitch);
			}



			GameObject generated_note = note_spawner.GetComponent<GenerateNotes>().generateNote(duration);
			generated_note.GetComponent<NoteBehavior> ().setNoteAttributes (birth_beat, note);

			return generated_note;
		}

		//Pauses the conductor from generating it's current song.
		public void pause(){

		}

		//Completely stops the song the conductor was generating
		public void stop(){
		}

		//Resumes what the song the conductor was generating
		public void resume(){

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
				new_child.GetComponent<GenerateNotes> ().color = RandomColor (color, color_choice);
				color += gradient;

				pitch_id = Pitch.incrementPitch (pitch_id, 1);

			}

		}

		Color RandomColor(float color, int color_choice){

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
