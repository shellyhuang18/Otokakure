using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertBehavior : MonoBehaviour {
	private GameObject content;

	public void DisplayAlert(string id){
		//instantiate prefab into gameobject 
		content = Instantiate ((GameObject)Resources.Load("Alerts/" + id));
		//set parent to canvas so it will display on screen
		content.transform.parent = GameObject.Find("AlertCanvas").transform;

		content.transform.position = new Vector2 (150, 150);

		GameObject.Find ("game_window").GetComponent<GameWindow> ().pause ();

	}
}
