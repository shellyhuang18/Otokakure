using UnityEngine;
using System.Collections;
using Manager = Communication.Manager;
using Module;

public class PrepareIntervalModule
{
	
	int repetitions;
	string lowest_pitch;
	string highest_pitch;
	int leading_rest_len;
	int trailing_rest_len;
	int min_note_length;
	int max_note_length;

	PrepareIntervalModule(){

	}

	// Use this for initialization
	void buttonClick ()
	{
		BaseModule module = new Module.PitchModule ();
		Manager.addExercise (module, 10);
		Manager.startSession ();
	}

	public void setRepetitions(int repetitions){
		this.repetitions = repetitions;
	}

}

