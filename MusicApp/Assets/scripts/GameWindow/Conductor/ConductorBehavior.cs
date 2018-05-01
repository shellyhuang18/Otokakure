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
			note_spawner.GetComponent<Spawner.GenerateNotes>().generateNote(duration);
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
			onSongStart ();

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
				//output chords
//				if (item.is_chord) {
//
//					Chord c = item as Chord;
//					//output notes in chord
//					foreach (Note i in c.notes) {
//						if (i.pitch != "r") {
//							triggerPitch (i.pitch, i.duration, metronome, i);
//						}
//					}
//
//					while (metronome != checkpoint) {
//						metronome++;
//						yield return new WaitForSeconds (single_beat_time/*amount of time passed for one beat*/);
//					}
//
//					//output single notes
//				} else {

				if (item.is_alert) {
					Alert alert = item as Alert;

					//retrieve alert from list of alerts(based on id)
					if (alert.multiple) {
						StartCoroutine(GameObject.Find ("AlertCanvas").GetComponent<AlertBehavior> ().DisplayAlertSlides (alert.id));

					} else {
						GameObject.Find("AlertCanvas").GetComponent<AlertBehavior>().DisplayAlert (alert.id);
					}


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
			onSongFinish();
		}
			

		//Called when the Song starts
		private void onSongStart(){
			isComposing = true;
			Debug.Log ("Song has been started");
		}

		//Called when the Song is finished composing. 
		//NOTE: When you want something to happen when the song is off screen,
		//then write in on onSongCompletelyDone.
		private void onSongFinish(){
			StartCoroutine (onSongCompletelyDone());
		}

		//Use this function when you want stuff to happen in the event that the song is off screen
		private IEnumerator onSongCompletelyDone(){
			//Wait until everything is off screen.
			int total_notes = GameObject.FindGameObjectsWithTag ("MusicalNote").Length;
			while(total_notes > 0){
				yield return new WaitForSeconds (0.01f);
				total_notes = GameObject.FindGameObjectsWithTag ("MusicalNote").Length;
			} 


			Debug.Log ("Song has been finished");
			isComposing = false;
		}
			
	}
}
