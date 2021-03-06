﻿//Contributor: Jack Chen, Shelly Huang

using UnityEngine;
using System.Collections;
using NoteLogic;
using Note = NoteLogic.NoteLogic.Note;
using Sound = NoteLogic.NoteLogic.Sound;
using Chord = NoteLogic.NoteLogic.Chord;
using Song = NoteLogic.NoteLogic.Song;
using GameElements = NoteLogic.NoteLogic.GameElements;
using Alert = NoteLogic.NoteLogic.Alert;
using UnityEngine.SceneManagement;
using Manager = Communication.Manager;

namespace Conductor{
	public class ConductorBehavior : MonoBehaviour
	{
		private float tempo;
		private bool isComposing; //Whether the conductor is busy creating a song
		private GameObject game_window;

		void Start(){
			isComposing = false;
			game_window = GameObject.Find ("game_window");
			tempo = game_window.GetComponent<GameWindow> ().getTempo ();

		}

		//Finds the corresponding child and commands it to create a pitch. The reference to the 
		//pitch is returned. Also associates the returned reference with it's musical information.
		public GameObject triggerPitch(string pitch, int duration, int birth_beat, Note note){
			GameObject note_spawner;
			if (pitch != "r") {
				note_spawner = GameObject.Find (pitch);
			} else {
				string default_pitch = game_window.GetComponent<GameWindow> ().getLowestPitch();
				note_spawner = GameObject.Find (default_pitch);
			}

			GameObject generated_note = note_spawner.GetComponent<Spawner.GenerateNotes>().generateNote(duration);
			generated_note.GetComponent<NoteBehavior> ().setNoteAttributes (birth_beat, note);

			return generated_note;
		}

		//Simple version. Just triggers the pitch and does not associate anything.
		public void triggerPitch(string pitch, int duration){
			GameObject note_spawner = GameObject.Find (pitch);
			GameObject generated_note = note_spawner.GetComponent<Spawner.GenerateNotes>().generateNote(duration);
			generated_note.GetComponent<NoteBehavior> ().setNoteAttributes (n: new Note (pitch, duration));
		}

		public float getTempo(){
			return this.tempo;
		}

		public void setTempo(float tempo){
			this.tempo = tempo;
		}
			
		//Pauses the conductor from generating it's current song.
		public void pause(){
			GameObject[] notes_on_screen = GameObject.FindGameObjectsWithTag ("MusicalNote");
			//store old velocity to use on resume
			if (notes_on_screen.Length > 0) {
				foreach (GameObject o in notes_on_screen) {
					o.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
				}
			}


		}

		//Completely stops the song the conductor was generating
		public void stop(){
			//We are not calling onSongFinish cause technically, the song didnt finish if you called stop.
			isComposing = false;

			//destroys all notes on screen
			GameObject[] notes_on_screen = GameObject.FindGameObjectsWithTag ("MusicalNote");
			foreach (GameObject o in notes_on_screen) {
				Destroy (o);
			}

			//Reset scene
			string curr_scene = SceneManager.GetActiveScene ().name;
			SceneManager.LoadScene(curr_scene);
		}

		//Resumes what the song the conductor was generating
		public void resume(){
			GameObject[] notes_on_screen = GameObject.FindGameObjectsWithTag ("MusicalNote");

			foreach (GameObject o in notes_on_screen) {
				o.GetComponent<NoteBehavior> ().setVelocityToTempo (game_window.GetComponent<GameWindow>().getTempo());
			}
		}

		//Starts a song.
		public void startSong(Song new_song){
			StartCoroutine (coroutineStartSong (new_song));

		}
			

		private IEnumerator coroutineStartSong(Song new_song){
			onSongStartComposing ();

			int metronome = 0;
			int checkpoint = 0; //The time of the next expected note in the song

			float buffer = 1.5f; //Jack's cheating hacky solution
			if (60 <= tempo && tempo < 100)
				buffer = 1.0f;
			else if (100 <= tempo && tempo < 140)
				buffer = 1.5f;
			else if (140 <= tempo)
				buffer = 1.75f;
			else
				buffer = 2.0f;

			float single_beat_time = 60/((tempo * buffer) * 4); //#16th notes / #minutes / #
			GameObject last_note = null;
			GameObject curr_note = null;


			foreach (GameElements item in new_song.score) {

				if (item.is_alert) {
					Alert alert = item as Alert;
					AlertBehavior alert_canvas = GameObject.Find ("AlertCanvas").GetComponent<AlertBehavior>();

					//retrieve alert from list of alerts(based on id)
					if (alert.multiple) {
						StartCoroutine(alert_canvas.DisplayAlertSlides (alert.id));

					} else {
						alert_canvas.DisplayAlert (alert.id);
					}
						
					while (!alert_canvas.getEndStatus ()) {
						yield return new WaitForSeconds (.1f);
					}

					alert_canvas.setEndStatus (false);
					continue;
				}

				Note n = item as Note;
				checkpoint += n.duration;

				GameWindow script = game_window.GetComponent<GameWindow> ();

				//keep on same note until amount of time has passed for former note to finish
				while ((metronome != checkpoint) || script.getPauseStatus()) {
					if(metronome != checkpoint && !script.getPauseStatus())
						metronome++;
					yield return new WaitForSeconds (single_beat_time/*amount of time passed for one beat*/);
				}                                                                                                                                                 

				//generate note

				//indicate if note is not a rest
				curr_note = triggerPitch (n.pitch, n.duration, metronome, n);

				if (n.pitch == "r") {
					//disable sprite and collider to 'hide' note object
					curr_note.GetComponent<SpriteRenderer> ().enabled = false;

					//Set the collider off so the pitch line doesn't detect the hidden note.
					curr_note.GetComponent<BoxCollider2D> ().enabled = false;

				}

				//last_note is the previously generated note from the one generated now.
				if (last_note != null) {
					float curr_pos_x = curr_note.GetComponent<SpriteRenderer> ().bounds.min.x;
					float last_pos_x = last_note.GetComponent<SpriteRenderer> ().bounds.max.x;
					float difference = last_pos_x - curr_pos_x;

					//add offset to curr_note pos
					Vector2 move = new Vector2(difference+ curr_note.transform.position.x, curr_note.transform.position.y);
					curr_note.transform.position = move;
				}
			
//				}

				//on what beat the last note has generated
				last_note = curr_note;

				metronome++;

				yield return new WaitForSeconds (single_beat_time);
			}

			//We are done generating music.
			onSongFinishComposing();
		}
			

		//Called when a Song starts
		private void onSongStartComposing(){
			game_window.GetComponent<GameWindow> ().setSongPlayingStatus (true);
			isComposing = true;
			GameObject game_window_UI = GameObject.Find ("game_window_UI");
			game_window_UI.GetComponent<ScoreBoard> ().setSongToProgressBar (game_window.GetComponent<GameWindow>().getCurrentSong());
		}

		//Called when the Song is finished composing.
		private void onSongFinishComposing(){
			StartCoroutine (waitForSongToLeaveScreen());
		}

		//Use this function when you want stuff to happen in the event that the song is off screen
		private IEnumerator waitForSongToLeaveScreen(){
			//Wait until everything is off screen.
			int total_notes = GameObject.FindGameObjectsWithTag ("MusicalNote").Length;
			while(total_notes > 0){
				yield return new WaitForSeconds (1f);
				total_notes = GameObject.FindGameObjectsWithTag ("MusicalNote").Length;
			} 

			//After this point, the song is off screen.
			onSongCompletelyDone ();
		}


		//When the song is done and off screen
		private void onSongCompletelyDone(){
			game_window.GetComponent<GameWindow> ().setSongPlayingStatus (false);
			isComposing = false;


			//Save the results of the user's performance into the database.
			GameObject.Find ("pitch_line").GetComponent<PitchLine.DetectNote> ().getDataAnalysis ().SetCurrentValues ();

			//Get the next song in the queue list if there is another
			if(Manager.getQueueLength() != 0){
				Manager.nextExercise();
				game_window.GetComponent<GameWindow>().startSong(Manager.generateSong());
			}
			//Or just leave
			else{
				game_window.GetComponent<GameWindow> ().exitSession ();
			}
		}
			
	}
}
