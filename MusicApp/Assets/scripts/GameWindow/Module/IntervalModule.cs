//Contributor: Jack Chen

using System.Collections.Generic;
using Pitch = Utility.Pitch;

namespace Module{
	public class IntervalModule: BaseModule{
		public enum step{ up, down, both};

		protected List<int> interval_selection; //All the possible intervals this module is set to generate. 
		protected step step_direction;

		private int middle_rest_len; //the length of the rest between the two notes

		//Constructor. The only required parameter is the interval selection
		public IntervalModule(List<int> intervals, string lowest_pitch = DEFAULT_LOWEST_PITCH, string highest_pitch = DEFAULT_HIGHEST_PITCH, 
		int leading_rest_len = DEFAULT_LEADING_LENGTH, int middle_rest_len = 0, int trailing_rest_len = DEFAULT_TRAILING_LENGTH, 
			int min_note_len = DEFAULT_MIN_NOTE_LENGTH, int max_note_len = DEFAULT_MAX_NOTE_LENGTH, step direction = step.both){

			this.interval_selection = intervals;
			this.lowest_pitch = lowest_pitch;
			this.highest_pitch = highest_pitch;
			this.leading_rest_len = leading_rest_len;
			this.middle_rest_len = middle_rest_len;
			this.trailing_rest_len = trailing_rest_len;
			this.step_direction = direction;
			this.min_note_length = min_note_len;
			this.max_note_length = max_note_len;
		
		}

		//The method responsible for generating a random SFS, given the parameters the module was set to.
		public override string generateSFS (){
			int range_length = Pitch.getTotalHalfSteps (lowest_pitch, highest_pitch);

			string sfs = "";

			//Appending leading rest
			if(leading_rest_len != 0)
				sfs += (leading_rest_len.ToString () + "r" + " ");

			//Appending the first pitch
			string note_a = Pitch.incrementPitch(lowest_pitch, generator.Next(0, range_length));
			int note_a_len = this.generator.Next (min_note_length, max_note_length + 1); //The upper bound is exclusive, so +1;
			sfs += (note_a_len.ToString() + note_a + " ");

			//Appending the middle rest
			if (middle_rest_len != 0)
				sfs += (middle_rest_len.ToString () + "r" + " ");

			//Appending the second pitch
			string note_b = "";
			int interval = interval_selection[this.generator.Next (0, interval_selection.Count)]; //randomly select an interval from the selection list

			//Incase you generate an interval that is outside their range
			do {
				if (step_direction == step.both) {
					int coin_flip = this.generator.Next (0, 1 + 1); //The upper bound is exclusive, so +1;

					if (coin_flip == 0)
						note_b = Pitch.incrementPitch (note_a, interval);
					else if (coin_flip == 1)
						note_b = Pitch.decrementPitch (note_a, interval);
					
				} else if (step_direction == step.up)
					note_b = Pitch.incrementPitch (note_a, interval);
				else if (step_direction == step.down)
					note_b = Pitch.decrementPitch (note_a, interval);
			} while(Utility.Pitch.isHigherPitch (highest_pitch, note_b) || Utility.Pitch.isHigherPitch (note_b, lowest_pitch));

			int note_b_len = this.generator.Next (min_note_length, max_note_length + 1); //The upper bound is exclusive, so +1;
			sfs += (note_b_len.ToString () + note_b + " ");

			//Appending trailing rest
			if(trailing_rest_len != 0)
				sfs += (trailing_rest_len.ToString () + "r" + " ");

			return sfs;
		}

		public void setIntervalSelection(List<int> selection){
			this.interval_selection = selection;
		}

		public void addIntervalSelection(int i){
			this.interval_selection.Add (i);
		}

		public bool removeIntervalSelection(int i){
			return this.interval_selection.Remove (i);
		}

		public void setStepDirection(step direction){
			this.step_direction = direction;
		}
	}
}

