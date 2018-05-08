using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using System.IO;
using System;

public class AlertBehavior : MonoBehaviour, IPointerClickHandler {
	private GameObject curr_content;

	private bool next_slide;
	private bool one_slide;

	void Start(){
		curr_content = null;
		next_slide = false;
		one_slide = false;
	}

	public void DisplayAlert(string id){
		GameObject content;

		//if slide is just one slide
		if (!Char.IsNumber( id[id.Length - 1] )) {
			Debug.Log ("single slide");
			one_slide = true;
		}
		//instantiate prefab into gameobject 
		content = Instantiate ((GameObject)Resources.Load("Alerts/" + id));
		//set parent to canvas so it will display on screen
		GameObject canvas = GameObject.Find("AlertCanvas");
		content.transform.parent = canvas.transform;

		content.transform.position = new Vector2 (canvas.transform.position.x, 150);

		GameObject.Find ("game_window").GetComponent<GameWindow> ().pause (); 

		curr_content = content;
	}

	public IEnumerator DisplayAlertSlides(string id){
		curr_content = null;
		next_slide = false;
		one_slide = false;

		//all alert dialogue slides will be ordered by number.
		//Ex: 
		//	alert1 alert2 alert3...
		int i = 1;
		while (true) {
			//iterate through content slides until it reaches a file[i] that doesn't exist
			if(!File.Exists (Application.dataPath + "/Resources/Alerts/" + id + i + ".prefab")){
				Debug.Log ("file not found: " + id + i);
				break;
			}

			//Display single alert
			DisplayAlert(id + i);

			//wait until user clicks to go to next slide
			while (next_slide == false) {
				yield return new WaitForSeconds (1);
			}

			next_slide = false;
			i++;

		}
		GameObject.Find ("game_window").GetComponent<GameWindow> ().resume ();
	}

	public void OnPointerClick(PointerEventData eventData){
		Destroy (curr_content);
		next_slide = true;

		if (one_slide) {
			GameObject.Find ("game_window").GetComponent<GameWindow> ().resume ();
		}
	}
}
