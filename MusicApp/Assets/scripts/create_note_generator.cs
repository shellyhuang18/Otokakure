using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class create_note_generator : MonoBehaviour {
	public GameObject note_spawner;


	float height; //The total absolute height of the thing.
	float lower_bound; //The lowest point of the note generating thing.
//	float higher_bound;
	float center; //The absolute center position of the note generating module
	public float tempo = 1;

	// Use this for initialization
	void Start () {
		//note_spawner = (GameObject)Resources.Load ("Assets/prefabs/note_spawner");

		height = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
		lower_bound = gameObject.transform.position.y - height / 2;
		center = gameObject.transform.position.y;

		generateChildren ("g4", "c5");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setTempo(float tempo){
		this.tempo = tempo;
	}

	void generateChildren(string lowest_pitch, string highest_pitch){
		//Determining how many children to create
		int total_children_to_create = GetComponent<utility>().get_total_half_steps(lowest_pitch, highest_pitch);

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
