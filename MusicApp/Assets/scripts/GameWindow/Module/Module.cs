using System.Collections;
using System.Collections.Generic;
using RandomGenerator = System.Random;

//The base class used as a template for generating random
namespace Module{
	public abstract class BaseModule {
		protected const string DEFAULT_LOWEST_PITCH = "c2";
		protected const string DEFAULT_HIGHEST_PITCH = "c6";
		protected const int DEFAULT_LEADING_LENGTH = 4;
		protected const int DEFAULT_TRAILING_LENGTH = 4;
		protected const int DEFAULT_MIN_NOTE_LENGTH = 4;
		protected const int DEFAULT_MAX_NOTE_LENGTH = 4;

		protected const int TOTAL_INTERVALS = 14; //There are 12 half steps + unison + octave

		protected RandomGenerator generator = new RandomGenerator(); //Random number generator

		protected string lowest_pitch = DEFAULT_LOWEST_PITCH;
		protected string highest_pitch = DEFAULT_HIGHEST_PITCH;
		protected int leading_rest_len = DEFAULT_LEADING_LENGTH;
		protected int trailing_rest_len = DEFAULT_TRAILING_LENGTH;
		protected int min_note_length = DEFAULT_MIN_NOTE_LENGTH;
		protected int max_note_length = DEFAULT_MAX_NOTE_LENGTH;
	

		abstract public string generateSFS ();

		public void setPitchRange(string lowest_pitch, string highest_pitch){
			if(Utility.Pitch.isHigherPitch(lowest_pitch, highest_pitch)){
				this.lowest_pitch = lowest_pitch;
				this.highest_pitch = highest_pitch;
			}
		}

		public bool setMinNoteLength(int i){
			//Check to see if the min len is less than max
			if (i > this.max_note_length) {
				return false;
			}
			else {
				this.min_note_length = i;
				return true;
			}
		}

		public bool setMaxNoteLength(int i){
			//Check to see if the max len is greater than min
			if (i < this.min_note_length) {
				return false;
			} 
			else {
				this.max_note_length = i;	
				return true;
			}
		}

		public void setTrailingLength(int length){
			this.trailing_rest_len = length;
		}

		public void setLeadingLength(int length){
			this.leading_rest_len = length;
		}

	}
}
