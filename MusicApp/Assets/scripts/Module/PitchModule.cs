using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomGenerator = System.Random;
using Utility;
using Module;

namespace Module{
public class PitchModule : BaseModule {

	public PitchModule(){
		this.lowest_pitch = "c4";
		this.highest_pitch = "c5";
		this.song_length = 16;

	}

	public PitchModule(string lower_pitch, string higher_pitch, int length){
		this.lowest_pitch = lower_pitch;
		this.highest_pitch = higher_pitch;
		this.song_length = length;


	}

	//The method responsible for generating a random SFS, given the parameters the module was set to.
	public override string generateSFS (){
		string SFS = "";
		int total_half_steps = Utility.Pitch.getTotalHalfSteps (this.lowest_pitch, this.highest_pitch);

		string[] pitch_arr = new string[total_half_steps];
		string pitch_itr = this.lowest_pitch;
		for(int i = 0; i < total_half_steps; i++){
			pitch_arr[i] = pitch_itr;
			pitch_itr = Utility.Pitch.incrementPitch(pitch_itr, 1);

		}

		//Generates all dotted 8th notes
		for (int i = 0; i < this.song_length; i += 4) {
			string random_pitch = pitch_arr [this.generator.Next (0, total_half_steps)];
			SFS += "3" + random_pitch + " ";
			SFS += "1r "; //Add a 16th note rest after each note"
		}

		return SFS;
	}
		
}
}
