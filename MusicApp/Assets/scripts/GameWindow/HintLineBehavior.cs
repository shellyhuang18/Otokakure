//Contributor: Jack Chen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HintLine{
public class HintLineBehavior : MonoBehaviour {


		void OnTriggerEnter2D(Collider2D collider){
			if(collider.gameObject.tag == "MusicalNote" ){
				GameObject note = collider.gameObject;
				note.GetComponent<NoteBehavior> ().playAudio();
			}
		}
			

		public void setEnabled(bool val){
				gameObject.GetComponent<SpriteRenderer> ().enabled = val;
				gameObject.GetComponent<BoxCollider2D> ().enabled = val;
		}

	}
}