//Contributor: Naseeb Gafar

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class PitchDetector : MonoBehaviour {
	AudioSource microphone;
	public string[] letterNoteArray =  {"A","A#","B","C","C#","D","D#","E","F","F#","G","G#"};
	private const int samples = 1024;
	private float SampleRate;
	private PitchTrack.Yin yin;
	private PitchTrack.PitchTracker ptracker;
	public string Note = "";
	public float MidiNote = 0;
	public float pitch = 0.0f;
	public string algorithm = "";
	void Start()
	{
		Application.RequestUserAuthorization (UserAuthorization.Microphone);
		SampleRate = AudioSettings.outputSampleRate;
		microphone = GetComponent<AudioSource>();
		microphone.clip = Microphone.Start ("Built-in Microphone", true, 10, 44100);
		microphone.loop = true;
		//microphone.volume = 0.0f;
		while (!(Microphone.GetPosition (null) > 0)) {
		}
		microphone.Play();
		algorithm = "Yin";
		if (algorithm == "Yin") {
			// Yin algorithm
			yin = new PitchTrack.Yin (SampleRate, samples);
		} else {
			// AutoCorrelation
			ptracker = new PitchTrack.PitchTracker ();
			ptracker.PitchDetected += ptracker_PitchDetected;
			ptracker.SampleRate = samples;
		}


	}
		
	void Update()
	{
		float[] floatBuffer = new float[samples];
		microphone.GetOutputData(floatBuffer,0);
		if (algorithm == "Yin") {
			pitch = yin.getPitch (floatBuffer).getPitch ();
			//Debug.Log ("The Yin Algorithm frequency   " + yin.getPitch(floatBuffer).getPitch());
			//Debug.Log ("The Note is " + getPitch (yin.getPitch (floatBuffer).getPitch ()));
		} else {
			ptracker.ProcessBuffer (floatBuffer);
		}
	}

	void ptracker_PitchDetected (PitchTrack.PitchTracker sender, PitchTrack.PitchTracker.PitchRecord pitchRecord)
	{
		setNumbers (pitchRecord.Pitch);

	}

	private void setNumbers(float pitchz){
		if (pitch < 0)
			pitch = 0;
		Debug.Log ("AutoCorrellation is " + pitch);
		pitch = pitchz;
		MidiNote = PitchTrack.PitchDsp.PitchToMidiNote (pitch);
		Note = PitchTrack.PitchDsp.GetNoteName((int)MidiNote, true, true);
	}
		

	public string getPitch(float pitchInHz) {
		// Base Note is A4 = 440
		float baseNote = 440;
		// octaves from base -> octaves  =  log(base2)(freq/base).
		double octavesFromBase = Mathf.Round(Mathf.Log(pitchInHz/baseNote, 2));
		// half steps = log2^12 (freq/base)
		// half steps from base -> half steps  =  12 * log(base2)(freq/base).
		double halfStepsFromBase = Mathf.Round (12 * Mathf.Log(pitchInHz/baseNote, 2));
		int letterNoteIndex = ((int) halfStepsFromBase % 12);
		if (letterNoteIndex < 0)
			letterNoteIndex += 12;
		int numberNote;
		if (halfStepsFromBase < 3) {
			numberNote = (int)halfStepsFromBase / 12 + 4;
		} else {
			numberNote = (int)halfStepsFromBase / 12 + 5;
		}
		return letterNoteArray[letterNoteIndex] + numberNote;
	}
		

}
