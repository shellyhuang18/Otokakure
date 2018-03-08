using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_notes : MonoBehaviour {
	

	public GameObject note;



	public void generateNote(float duration){
		//Duration is out of 16. (for how many 16th notes)
		GameObject generate = (GameObject)Instantiate (note);
		generate.transform.position = this.transform.position;

		float tempo = this.transform.parent.GetComponent<create_note_generator> ().tempo;
		generate.GetComponent<Rigidbody2D> ().velocity = new Vector2(-1*tempo, 0);

		//generate width based on duration
		generate.transform.localScale = new Vector2 (duration/16, generate.transform.localScale.y);
	}

}


	//IEnumerator Speed(){
		
	//	yield return new WaitForSeconds (1);
	//}


