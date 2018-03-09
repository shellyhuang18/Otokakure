using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_note : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		DestroyNote ();
	}

	void DestroyNote(){
		float height = 2*Camera.main.orthographicSize;
		float width = height*Camera.main.aspect;

		float left_bound = (Camera.main.transform.position.x- width)/2;

		if (this.transform.position.x < left_bound-5) {
			Destroy (this.gameObject);
		}
	}
}
