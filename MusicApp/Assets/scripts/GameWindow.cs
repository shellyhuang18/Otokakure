using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor;
using PitchLine;
using Utility;

public class GameWindow : MonoBehaviour {

	public string lowest_pitch;
	public string highest_pitch;

	// Use this for initialization
	void Start () {
		
	}

	void initalizeChildren(){
		GameObject.Find ("conductor").GetComponent<CreateNoteGenerator>().initalize ();
		GameObject.Find ("pitch_line").GetComponent<CreateNoteGenerator>().initalize ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("o")) {
			Debug.Log ("a3: " + Utility.Pitch.toFrequency ("a3"));
		}
		
		if (Input.GetKeyDown ("n")) {
			Debug.Log ("a4: " + Utility.Pitch.toFrequency ("a4"));
		}
		if (Input.GetKeyDown ("m")) {
			Debug.Log ("c4: " + Utility.Pitch.toFrequency ("c4"));
		}
		if (Input.GetKeyDown ("l")) {
			Debug.Log ("e6: " + Utility.Pitch.toFrequency ("e6"));
		}
	}
}
