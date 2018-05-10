using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomGenerator = System.Random;
using Utility;
using Module;

namespace Module{
	public sealed class PitchModule : BaseModule {


		//Constructor. 
		public PitchModule(string lowest_pitch = DEFAULT_LOWEST_PITCH, string highest_pitch = DEFAULT_HIGHEST_PITCH, 
			int leading_rest_len = DEFAULT_LEADING_LENGTH, int trailing_rest_len = DEFAULT_TRAILING_LENGTH, 
			int min_note_len = DEFAULT_MIN_NOTE_LENGTH, int max_note_len = DEFAULT_MAX_NOTE_LENGTH){

			this.lowest_pitch = lowest_pitch;
			this.highest_pitch = highest_pitch;
			this.leading_rest_len = leading_rest_len;
			this.trailing_rest_len = trailing_rest_len;
			this.min_note_length = min_note_len;
			this.max_note_length = max_note_len;

		}

		public PitchModule(string lower_pitch, string higher_pitch, int length){
			this.lowest_pitch = lower_pitch;
			this.highest_pitch = higher_pitch;
		}

		//The method responsible for generating a random SFS, given the parameters the module was set to.
		public override string generateSFS (){
			string sfs = "";

			if(leading_rest_len != 0 ){
				sfs += (leading_rest_len.ToString() + "r" + " ");
			}

			int pitch_range = Pitch.getTotalHalfSteps (lowest_pitch, highest_pitch);
			string note = Pitch.incrementPitch (lowest_pitch, generator.Next (0, pitch_range));
			int note_duration = generator.Next (min_note_length, max_note_length + 1); //max is exclusive, so +1

			sfs += (note_duration.ToString() + note + " ");


			if(trailing_rest_len != 0 ){
				sfs += (trailing_rest_len.ToString() + "r" + " ");
			}

			return sfs;
		}
			
	}
}
