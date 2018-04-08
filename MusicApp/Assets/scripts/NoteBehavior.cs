using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Note = NoteLogic.NoteLogic.Note;

public class NoteBehavior : MonoBehaviour {

	private Note note;
	public string pitch;
	private int birth_beat = -1; //The beat that this note_behavior was generated

	//Sets the Note object's parameters. Acts as a pseudo constructor.
	public void setNoteAttributes(string pitch, int duration, int birth_beat){
		this.note = new Note (pitch, duration);
		this.pitch = this.note.pitch;
	}

	public int getDuration(){
		return this.note.duration;
	}

	public string getPitch(){
		return this.note.pitch;
	}

	public int getBirthBeat(){
		return birth_beat;
	}

	public void setBirthBeat(int birth_beat){
		this.birth_beat = birth_beat;
	}

	//Plays the audio file of the note object associated in this script.
	public void playAudio(){

	}
}
