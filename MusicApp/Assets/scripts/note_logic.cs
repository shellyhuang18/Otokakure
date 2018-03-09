using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class note_logic : MonoBehaviour {
	
	public class Chord : Sound{

	}

	public class Note : Sound{
		public Note(string pitch, int duration){
			
			this.pitch = pitch;
			this.duration = duration;
		}

		string pitch;
	}

	public class Sound{
		public int duration;
	}

	public class Song{
		public Song(string sfs){
			//parses string and puts associated values into respective variables.
			//TODO: make documentation on dynamics and how they're represented in string score format
			//ex: 4c 4c# 4e 4g <16c 16e 16g>
			//

			string[] notes;
			notes = sfs.Split (' ');
		
			for(int i = 0; i < notes.Length; ++i){
				Note new_note;
				//if the first parameter is a char value (i.e. dynamic or other)
				if(Char.IsNumber (notes[0][0] )){
					

				}else{
					int end_point;
					for(int j = 0; j < notes[i].Length; ++j){
						if(!Char.IsNumber(notes[i][j])){
							string note = notes[i].Substring (0, j);
							Debug.Log(note);
						}
					}	

				}

			}
		}

		//time signature
		float time_sig;
		float tempo;
		List<Sound> score;
	}

	void Start(){
		Song k = new Song ("4c 4c# 4e 4g <16c 16e 16g>");
	}
}
