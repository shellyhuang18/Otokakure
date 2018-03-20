using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor;

namespace Spawner{
	public class GenerateNotes : MonoBehaviour {
		public GameObject note;
		private float height;
		public Color color;

		void Start(){
			
		}

		public void generateNote(float duration){
			//Duration is out of 16. (for how many 16th notes)
			GameObject generated_note = (GameObject)Instantiate (note);

			float tempo = GetComponentInParent<Conductor.CreateNoteGenerator> ().tempo;
			generated_note.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-1 * tempo, 0);

			//generate width based on duration and height to be as large as the note_spawner's height; TRUST THE MATH IT WORKS DONT TOUCH
			generated_note.transform.localScale = new Vector2 (duration / 16, 
				GetComponentInParent<Conductor.CreateNoteGenerator>().div_space/generated_note.GetComponent<SpriteRenderer> ().bounds.size.y );


			//TRANSLATE THE NOTE TO THE CORRECT POSITION(On top of the spawner and behind the conductor line)
			generated_note.transform.position = new Vector2(this.transform.position.x + generated_note.GetComponent<SpriteRenderer>().bounds.extents.x, (float)(this.transform.position.y));

			//changes color of notes being generated
			generated_note.GetComponent<SpriteRenderer>().color = color;
		}
		//destroys note when off screen 

	}
}



