using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utility;

public class create_note_generator : MonoBehaviour {
	public GameObject note_spawner; //The child object that spawns notes


	float height; //The total absolute height of the thing.
	float lower_bound; //The lowest point of the note generating thing.
	public float div_space;

	public float tempo;

	// Use this for initialization
	void Start () {
		height = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
		lower_bound = gameObject.transform.position.y - height / 2;

		generateChildren ("c4", "e4");

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")) {
			triggerPitch ("c4", 4);
		}

		if (Input.GetKeyDown ("a")) {
			triggerPitch ("d4", 8);
		}

		if (Input.GetKeyDown ("w")) {
			triggerPitch ("d#4", 16);
		}

	}

	void setTempo(float tempo){
		this.tempo = tempo;
	}

	void triggerPitch(string pitch, int duration){
		GameObject note_spawner = GameObject.Find (pitch);
		note_spawner.GetComponent<generate_notes>().generateNote(duration);
	}


	void generateChildren(string lowest_pitch, string highest_pitch){
		//Determining how many children to create
		int total_children_to_create = utility.utility.get_total_half_steps(lowest_pitch, highest_pitch) + 1;

		//Create children and reposition them to fit note generator(rectangle)
		div_space = height / total_children_to_create;
		float interval = 0;

		string pitch_id = lowest_pitch;
		for (int i = 0; i < total_children_to_create; ++i) {
			GameObject new_child = (GameObject)Instantiate (note_spawner);
			new_child.transform.parent = this.transform;
			new_child.name = pitch_id;

			pitch_id = utility.utility.incrementPitch (pitch_id, 1);

			new_child.transform.position = new Vector2 (this.transform.position.x, lower_bound + interval);
			interval += div_space;
		}

	}



}
