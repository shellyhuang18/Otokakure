using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor;

namespace Spawner{
	public class GenerateNotes : MonoBehaviour {
		[SerializeField]
		private GameObject note;
		private float height;
		private Color color; 
		private string pitch; //The pitch this child is responsible for.


		//Sets the pitch that this spawner is responsible for spawning.
		public void setAssociatedPitch(string pitch){
			this.pitch = pitch;
		}

		public void setColor(Color color){
			this.color = color;
		}
			
		public GameObject generateNote(float duration){
			//Duration is out of 16. (for how many 16th notes)
			GameObject generated_note = (GameObject)Instantiate (note);

			float tempo = GetComponentInParent<ConductorBehavior> ().getTempo();
			generated_note.GetComponent<NoteBehavior>().setVelocityToTempo(tempo);

			//generate width based on duration and height to be as large as the note_spawner's height; TRUST THE MATH IT WORKS DONT TOUCH
			generated_note.transform.localScale = new Vector2 (duration / 16, 
				GetComponentInParent<Conductor.CreateNoteGenerator>().div_space/generated_note.GetComponent<SpriteRenderer> ().bounds.size.y );

			//Resize collider
			BoxCollider2D col = (BoxCollider2D)generated_note.GetComponent<BoxCollider2D>();
			SpriteRenderer sprite = generated_note.GetComponent<SpriteRenderer> ();
			float collider_x = sprite.bounds.size.x / generated_note.transform.localScale.x;
			float collider_y = sprite.bounds.size.y / generated_note.transform.localScale.y;
			col.size = new Vector2 ( collider_x, collider_y);

			//TRANSLATE THE NOTE TO THE CORRECT POSITION(On top of the spawner and behind the conductor line)
			generated_note.transform.position = new Vector2(this.transform.position.x + generated_note.GetComponent<SpriteRenderer>().bounds.extents.x, (float)(this.transform.position.y));

			//changes color of notes being generated
			generated_note.GetComponent<SpriteRenderer>().color = color;

			return generated_note;
		}
		//destroys note when off screen 

	}
}



