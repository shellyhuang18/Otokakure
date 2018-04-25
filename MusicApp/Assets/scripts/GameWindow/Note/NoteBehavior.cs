using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Note = NoteLogic.NoteLogic.Note;

public class NoteBehavior : MonoBehaviour {

	private Note note;
	public string pitch;
	private int birth_beat = -1; //The beat that this note_behavior was generated


	//Sets the Note object's parameters. Acts as a pseudo constructor.
	public void setNoteAttributes(int birth_beat, Note n){
		this.note = n;
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

	public void setVelocityToTempo(float tempo){
		GameObject game_window = GameObject.FindGameObjectWithTag ("GameWindow");
//		float dist_btw_line_and_right_edge = Math.Abs(GameObject.Find("pitch_line").transform.position.x - (game_window.transform.position.x + game_window.GetComponent<SpriteRenderer>().bounds.extents.x));
		float quarter_note_width = gameObject.GetComponent<SpriteRenderer> ().bounds.size.x / 4.0f;
		float speed = (/*dist_btw_line_and_right_edge + */ (tempo * quarter_note_width)) / 60.0f; //Veloctor = Unity Unit/s   (unity unit should be defaulted to meter)

		gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(-1 * speed, 0);
	}
}
