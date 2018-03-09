using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utility;
using System.Threading;


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
		if (Input.GetKeyDown ("q")) {
			triggerPitch ("c4", 4);
		}

		if (Input.GetKeyDown ("w")) {
			triggerPitch ("c#4", 4);
		}
		if (Input.GetKeyDown ("e")) {
			triggerPitch ("d4", 8);
		}

		if (Input.GetKeyDown ("r")) {
			triggerPitch ("d#4", 16);
		}

		if (Input.GetKeyDown ("t")) {
			triggerPitch ("e4", 16);
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

		//Create children and reposition them to fit note generator(conductor)
		div_space = height / total_children_to_create;
		float interval = (float)0.5*div_space; //we want the first position to spawn at 1/2th a divspace

		string pitch_id = lowest_pitch;

		float color = 0;
		//how much the color will change between each note depending on how many pitches there are
		float gradient = 255/total_children_to_create;

		//chooses random color 
		System.Random rand = new System.Random ();
		int color_choice = rand.Next (1, 6);

		for (int i = 0; i < total_children_to_create; ++i) {
			GameObject new_child = (GameObject)Instantiate (note_spawner);
			new_child.transform.parent = this.transform;
			new_child.name = pitch_id;

			pitch_id = utility.utility.incrementPitch (pitch_id, 1);

			new_child.transform.position = new Vector2 (this.transform.position.x, lower_bound + interval);
			interval += div_space;

			//changes color of notes being generated
			new_child.GetComponent<generate_notes> ().color = RandomColor (color, color_choice);
			color += gradient;
		}

	}

	Color RandomColor(float color, int color_choice){
		Debug.Log (color_choice);

		switch (color_choice) {
		case 1:
			return new Color (color / 255, 0, 25);//ok
		case 2:
			return new Color (25, color / 255, 0); //ok
		case 3:
			return new Color (color / 255, 0, 0);//ok
		case 4:
			return new Color (0, color / 255 , 20);//ok
		case 5:
			return new Color (30, color / 255, 20);//ok
		}

		return new Color (0, 0, 0);

	}



}
