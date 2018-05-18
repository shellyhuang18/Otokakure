//Contributor: Jack Chen

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

	private float max_opacity = 0.6f; //The max opacity when the note hasn't been hit

	void Awake(){
		startNoteFadeIn ();
		startOpaqueWhenHit ();
	}
		
	private void startOpaqueWhenHit(){
		StartCoroutine (opaqueWhenHitCoroutine ());
	}

	//A coroutine meant to be called within startOpaqueWhenHit();
	private IEnumerator opaqueWhenHitCoroutine(){
		while (true) {
			if (gameObject.GetComponent<Collider2D> ().IsTouching (GameObject.Find ("arrow").GetComponent<Collider2D> ())) {
				makeSolid ();
			} else {
				makeGhost ();
			}
			yield return new WaitForSeconds (0.01f);
		}
	}

	public void makeSolid(){
		Color color = this.gameObject.GetComponent<SpriteRenderer> ().material.color;
		color.a = 1.0f;
		this.gameObject.GetComponent<SpriteRenderer> ().material.color = color;
	}

	public void makeGhost(){
		Color color = gameObject.GetComponent<SpriteRenderer> ().material.color;
		color.a = this.max_opacity;
		gameObject.GetComponent<SpriteRenderer> ().material.color = color;
	}

	void OnMouseDown(){
		//Play audio file as a hint. 
		playAudio(); //By default, the hint plays the note for 1 second
	}

	//Plays the audio of the note
	public AudioSource playAudio(){
		string clip_id = this.pitch;
		string file_name = "Sounds/" + clip_id;
		AudioClip audio_clip = Resources.Load(file_name) as AudioClip;
		AudioSource audio_source = gameObject.GetComponent<AudioSource> ();


		//Load the clip into the audio source
		audio_source.clip = audio_clip;

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

	public void setVelocityToTempo(float tempo){
		
		float quarter_note_width = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 4.0f;
		float speed = (tempo * quarter_note_width) / 60.0f; //Velocity = Unity Unit/s   (unity unit should be defaulted to meter)

		gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(-1 * speed, 0);
	}

	//A function used to make the notes fade into the game window
	public void startNoteFadeIn(){
		StartCoroutine (transparencyToOpaqueCoroutine());
	}

	//A coroutine meant to be called within startNoteFadeIn();
	private IEnumerator transparencyToOpaqueCoroutine(){
		GameObject game_window = GameObject.FindGameObjectWithTag ("GameWindow");
		float alpha_per_wait = 0.025f; //How much the opaqueness should increase for each coroutine call.
		float start_alpha_value = 0.05f; //The transparency value that the note starts with.

		Color color = gameObject.GetComponent<SpriteRenderer> ().color;
		color.a = start_alpha_value;
		gameObject.GetComponent<SpriteRenderer> ().material.color = color;

		float extent = game_window.GetComponent<SpriteRenderer> ().bounds.extents.x;
		float left_bound = game_window.transform.position.x + extent;
		float right_bound = game_window.transform.position.x - extent;

		//Since we have a buffer, wait until the note is actually on screen
		while (!(right_bound < gameObject.transform.position.x && gameObject.transform.position.x < left_bound)) {
			yield return new WaitForSeconds (0.01f);
		}

		while (color.a < this.max_opacity) {
			color.a += alpha_per_wait;
			gameObject.GetComponent<SpriteRenderer> ().material.color = color;
			yield return new WaitForSeconds (0.01f);
		}
	}


}
