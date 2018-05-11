using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEditor;
using System.IO;
using System;

public class AlertBehavior : MonoBehaviour {
	private GameObject curr_content;

	private bool next_slide;
	private bool one_slide;
	private bool end;

	void Start(){
		curr_content = null;
		next_slide = false;
		one_slide = false;
		end = false;

	}
		
	public bool getEndStatus(){
		return this.end;
	}
	public void setEndStatus(bool val){
		this.end = val;
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

		content.transform.position = new Vector2 (canvas.transform.position.x/3, canvas.transform.position.y/3);

		GameObject.Find ("game_window").GetComponent<GameWindow> ().pause (); 

		this.curr_content = content;


	}

	public IEnumerator DisplayAlertSlides(string id){
		//curr_content = null;
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
				this.end = true;
				break;
			}

			//Display single alert
			DisplayAlert(id + i);

			//wait until user clicks to go to next slide
			while (next_slide == false) {
				if (Input.GetMouseButtonDown (0)) {

					Destroy (curr_content);
					next_slide = true;

					if (one_slide) {
						GameObject.Find ("game_window").GetComponent<GameWindow> ().resume ();
					}
				}

				yield return new WaitForSeconds (.01f);
			}

			next_slide = false;
			i++;

		}
		GameObject.Find ("game_window").GetComponent<GameWindow> ().resume ();
	}
}
