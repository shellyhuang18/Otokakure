using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Utility{
	public class ErrorCheck : MonoBehaviour {

		//Returns boolean whether something is a validly formmated pitch.
		public static bool isValidPitchFormat(string pitch){
			if (!Regex.IsMatch (pitch, Pitch.valid_pitch_expression)) {
				return false;
			} else {
				return true;
			}
		}

	}
}
