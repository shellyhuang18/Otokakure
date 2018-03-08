using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_notes : MonoBehaviour {
	

	public GameObject note;



	void generateNote(){
		GameObject generate = (GameObject)Instantiate (note);
		Debug.Log (generate.name);
		generate.GetComponent<Rigidbody2D> ().velocity = new Vector2(-2, 0);
		//generate.transform.localScale = new Vector2 (2, generate.transform.localScale.y);
	}

	// Use this for initialization
	void Start () {

	} 

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")) {
			generateNote ();
		}
	}

}


	//IEnumerator Speed(){
		
	//	yield return new WaitForSeconds (1);
	//}


