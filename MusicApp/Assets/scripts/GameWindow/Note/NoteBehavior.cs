using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Note = NoteLogic.NoteLogic.Note;

public class NoteBehavior : MonoBehaviour {

	private Note note;
	[SerializeField]
	private string pitch;
	[SerializeField]
	private int duration = -1;
	private int birth_beat = -1; //The beat that this note_behavior was generated


	void OnMouseDown(){
		//Play audio file as a hint. 
		playAudio(this.pitch, 1f); //By default, the hint plays the note for 1 second
	}

	//Plays the audio of a certain pitch for n seconds. Returns an reference to the source
	//if you need it.
	public AudioSource playAudio(string pitch, float seconds){
		string clip_id = pitch;
		string file_name = "Sounds/" + clip_id;
		AudioClip audio_clip = Resources.Load(file_name) as AudioClip;
		AudioSource audio_source = gameObject.GetComponent<AudioSource> ();

		//Load the clip into the audio source
		audio_source.clip = audio_clip;
//		audio_source.SetScheduledEndTime (seconds);

		//source now plays the assigned clip
		audio_source.Play ();

		return audio_source;
	}
		


	//When you want to set the attributes, but dont care about the birth beat
	public void setNoteAttributes(Note n){
		this.birth_beat = -1;
		this.note = n;
		this.pitch = n.pitch;
		this.duration = n.duration;
	}

	//Sets the Note object's parameters. Acts as a pseudo constructor.
	public void setNoteAttributes(int birth_beat, Note n){
		this.note = n;
		this.pitch = n.pitch;
		this.duration = n.duration;
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
		
		float quarter_note_width = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 4.0f;
		float speed = (tempo * quarter_note_width) / 60.0f; //Velocity = Unity Unit/s   (unity unit should be defaulted to meter)

		gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(-1 * speed, 0);
	}
}
