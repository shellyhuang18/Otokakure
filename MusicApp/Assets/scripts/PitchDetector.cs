using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class PitchDetector : MonoBehaviour {
	AudioSource microphone;
	public int samplerate = 11024;
	public string[] letterNoteArray =  {"A","A#","B","C","C#","D","D#","E","F","F#","G","G#"};
	// Use this for initialization
	void Start () {
		Debug.Log ("Started");
		microphone = GetComponent<AudioSource>();
		microphone.clip = Microphone.Start ("Built-in Microphone", true, 10, 44100);
		microphone.loop = true;
		while (!(Microphone.GetPosition (null) > 0)) {
		}
		microphone.Play();
		//Debug.Log (microphone.clip.frequency + " ");
		//float[] spectrum;
		//spectrum = microphone.GetOutputData ();
		//Debug.Log (microphone.pitch + " ");
		
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (microphone.clip.frequency + " ");
		//Debug.Log (GetFundamentalFrequency() + " ");
		Debug.Log ("The pitch is " + getPitch(GetFundamentalFrequency()) + " ");
		Debug.Log ("The frequency is " + GetFundamentalFrequency() + " ");
	}

	/*float GetFundamentalFrequency()
	{
		float fundamentalFrequency = 0.0f;
		float[] data = new float[8192];
		microphone.GetSpectrumData(data,0,FFTWindow.BlackmanHarris);
		return fundamentalFrequency;
	}*/
	float GetFundamentalFrequency()
	{
		float fundamentalFrequency = 0.0f;
		float[] data = new float[8192];
		microphone.GetSpectrumData(data,0,FFTWindow.BlackmanHarris);
		float s = 0.0f;
		int i = 0;
		for (int j = 1; j < 8192; j++)
		{
			if ( s < data[j] )
			{
				s = data[j];
				i = j;
			}
		}
		fundamentalFrequency = i * samplerate / 8192;
		return fundamentalFrequency;
	}

	public string getPitch(float pitchInHz) {
		// Base Note is A4 = 440
		float baseNote = 440;
		// octaves from base -> octaves  =  log(base2)(freq/base).
		double octavesFromBase = Mathf.Round(Mathf.Log(pitchInHz/baseNote, 2));
		double halfStepsFromBase = Mathf.Round (12 * Mathf.Log(pitchInHz/baseNote, 2));
		//return halfStepsFromBase;
		//double octavesFromBase = Math.round((Math.log((pitchInHz/baseNote))/Math.log(2)));
		// half steps = log2^12 (freq/base)
		//double halfStepsFromBase = (Math.log((pitchInHz/baseNote))/Math.log(a));
		// half steps from base -> half steps  =  12 * log(base2)(freq/base).
		//double halfStepsFromBase = Math.round(12 * (Math.log((pitchInHz/baseNote))/Math.log(2)));
		int letterNoteIndex = ((int) halfStepsFromBase % 12);
		if (letterNoteIndex < 0)
			letterNoteIndex += 12;
		int numberNote;
		int other = (int)halfStepsFromBase / 12;
		if (halfStepsFromBase < 3) {
			numberNote = (int)halfStepsFromBase / 12 + 4;
		} else {
			numberNote = (int)halfStepsFromBase / 12 + 5;
		}
		return letterNoteArray[letterNoteIndex] + numberNote;
		//noteText.setText(letterNoteArray[letterNoteIndex] + numberNote);*/
	}
}
