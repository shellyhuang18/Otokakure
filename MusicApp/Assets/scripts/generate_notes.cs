using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_notes : MonoBehaviour {
	public GameObject note;
	private float height;



	public void generateNote(float duration){
		//Duration is out of 16. (for how many 16th notes)
		GameObject generated_note = (GameObject)Instantiate (note);

		//Generates the note on top of the note_spawner
		generated_note.transform.position = new Vector2(this.transform.position.x, (float)(this.transform.position.y));

		float tempo = this.transform.parent.GetComponent<create_note_generator> ().tempo;
		generated_note.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-1 * tempo, 0);

		//generate width based on duration and height to be as large as the note_spawner's height; TRUST THE MATH IT WORKS DONT TOUCH
		generated_note.transform.localScale = new Vector2 (duration / 16, 
			GetComponentInParent<create_note_generator>().div_space/generated_note.GetComponent<SpriteRenderer> ().bounds.size.y );

	}

}



