using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
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
		Debug.Log ("The pitch is " + Utility.Pitch.getNearestPitch(GetFundamentalFrequency()) + " ");
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

}
