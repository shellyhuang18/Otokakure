using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utility;

public class create_note_generator : MonoBehaviour {
	public GameObject note_spawner; //The child object that spawns notes


	float height; //The total absolute height of the thing.
	float lower_bound; //The lowest point of the note generating thing.
	float center; //The absolute center position of the note generating module

	public float tempo = 1;

	// Use this for initialization
	void Start () {
//		note_spawner = (GameObject)Resources.Load ("Assets/prefabs/note_spawner");
		height = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
		lower_bound = gameObject.transform.position.y - height / 2;
		center = gameObject.transform.position.y;

		generateChildren ("c4", "c#4");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")) {
			triggerPitch ("c4", 1);
		}
	}

	void setTempo(float tempo){
		this.tempo = tempo;
	}

	void triggerPitch(string pitch, int duration){
		GameObject note_spawner = GameObject.Find (pitch);
		Debug.Log (note_spawner.name);
//		note_spawner.GetComponents<generate_notes>().generateNote (duration);
	}


	void generateChildren(string lowest_pitch, string highest_pitch){
		//Determining how many children to create
		int total_children_to_create = utility.utility.get_total_half_steps(lowest_pitch, highest_pitch);

		//Create children and reposition them to fit note generator(rectangle)
		float div_space = height / total_children_to_create;
		float interval = 0;
		for (int i = 0; i < total_children_to_create; ++i) {
			GameObject new_child = (GameObject)Instantiate (note_spawner);
			new_child.transform.parent = this.transform;
			new_child.name = "child_spawner";

			new_child.transform.position = new Vector2 (this.transform.position.x, lower_bound + interval);
			interval += div_space;
		}

	}



}
