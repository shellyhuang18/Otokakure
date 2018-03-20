using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NoteLogic{
	public class NoteLogic : MonoBehaviour {

		public class Chord : Sound{
			public List<Note> notes;
			public Chord(){
				is_chord = true;
				notes = new List<Note>();
			}

		}

		public class Note : Sound{
			public string pitch;

			public Note(string pitch, int duration){
				this.pitch = pitch;
				this.duration = duration;
			}

		}

		public class Sound{
			//for dynamic casting-
			public bool is_chord = false;
			public int duration;
			public char dynamic;
		}

		public class Song{
			float time_sig;
			float tempo;
			public List<Sound> score; //add


			public Song(string sfs){
				//parses string and puts associated values into respective variables.
				//TODO: make documentation on dynamics and how they're represented in string score format
				//ex: 4c 4c# 4e 4g <16c 16e 16g>
				score = new List<Sound>();

				string[] notes;
				notes = sfs.Split (' ');

				bool is_chord = false;

				string str_dur;
				int duration;
				string pitch;

				//TODO: dynamically make chords. Problem is that if new Chord object is declared 
				//every iteration of the loop, too much garbage. Create correct number of Chord 
				//objects that need to be made whilst using that same object and filling it with
				//notes until end of chord '>'. (Right now it just makes one and that's dumb)

				//TODO: error check so that sfs is in the correct format
				Chord new_chord = null;
				for(int i = 0; i < notes.Length; ++i){
					Note new_note = null;
					//iterate through rest of string
					for(int j = 0; j < notes[i].Length; ++j){
						//start of chord
						if(notes[i][0].Equals ('<')){
							new_chord = new Chord();
							new_chord.is_chord = true;
							is_chord = true;

							if(!Char.IsNumber (notes[i][j+1])){
								//get duration of note, since start of chord omit first char
								str_dur = notes[i].Substring (1, j);
								duration = Int32.Parse(str_dur);

								pitch = notes[i].Substring (j+1, notes[i].Length-str_dur.Length-1);

								new_note = new Note(pitch, duration);
								new_chord.notes.Add (new_note);
								//since no broken chords, each note in chord has same duration
								new_chord.duration = duration;

								//Debug.Log("note: " + duration + " " + pitch);

								break;
							}

						}
						//end of chord
						else if(notes[i][notes[i].Length-1].Equals ('>')){
							is_chord = false;

							if(!Char.IsNumber (notes[i][j])){
								str_dur = notes[i].Substring (0, j);
								duration = Int32.Parse(str_dur);

								pitch = notes[i].Substring (j, notes[i].Length-str_dur.Length-1);

								new_note = new Note(pitch, duration);
								new_chord.notes.Add (new_note);

								//add that shit to the score
								score.Add (new_chord);
								//Debug.Log("note: " + duration + " " + pitch);

								break;
							}

						}
						//note has dynamic
						/*else if(Char.IsNumber (notes[i][0])){
					
						}*/
						else{
							if(!Char.IsNumber (notes[i][j])){
								str_dur = notes[i].Substring (0, j);		
								duration = Int32.Parse(str_dur);

								pitch = notes[i].Substring (j, notes[i].Length-str_dur.Length);
								new_note = new Note(pitch, duration);

								//to distinguish if notes are contained within a chord
								if(is_chord){
									new_chord.notes.Add (new_note);
								}else{
									score.Add (new_note);
									//Debug.Log(new_note.duration + " " + new_note.pitch);
								}
								break;
							}
								
						}
		
					}	

				}


			}//song constructor

			public void PrintScore(){
				foreach (Sound item in score) {
					if (item.is_chord) {
						Chord d = item as Chord;
						foreach (Note i in d.notes) {
							Debug.Log ("yessss: " + i.duration + " " + i.pitch);
						}
					} else {
						Note n = item as Note;
						Debug.Log ("aaawww yisss " + n.duration + " " + n.pitch);
					}
				}
			}

		}

		void Start(){
			//Song sng = new Song ("3c4 <6g#4 6c4 6d#4>");
			//sng.PrintScore ();
		}
	}
}