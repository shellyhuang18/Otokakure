using System.Collections;
using System.Collections.Generic;
using RandomGenerator = System.Random;

//The base class used as a template for generating random
namespace Module{
	public abstract class BaseModule {

		protected RandomGenerator generator; //Random number generator
		protected string lowest_pitch;
		protected string highest_pitch;
		protected int song_length; //The length (in 16th notes) of the song that the module is set to generate.

		//Since the generator uses a consistent seed, it's best to save the generator for multiple use.
		protected BaseModule(){ 
			generator = new RandomGenerator (); 
		}

		abstract public string generateSFS ();

		public void setPitchRange(string lowest_pitch, string highest_pitch){
			if(Utility.Pitch.isHigherPitch(lowest_pitch, highest_pitch)){
				this.lowest_pitch = lowest_pitch;
				this.highest_pitch = highest_pitch;
			}
		}

		public void setLength(int length){
			this.song_length = length;
		}

	}
}
