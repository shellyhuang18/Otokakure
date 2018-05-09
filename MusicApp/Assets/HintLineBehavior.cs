using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HintLine{
public class HintLineBehavior : MonoBehaviour {


		void OnTriggerEnter2D(Collider2D collider){
			if(collider.gameObject.tag == "MusicalNote" ){
				GameObject note = collider.gameObject;
				note.GetComponent<NoteBehavior> ().playAudio();
//				highlightForOneSecond (note);
			}
		}
			

		public void setEnabled(bool val){
				gameObject.GetComponent<SpriteRenderer> ().enabled = val;
				gameObject.GetComponent<BoxCollider2D> ().enabled = val;
		}

//		//Highlights a note for one second
//		private void highlightForOneSecond(GameObject note_obj){
//			StartCoroutine (highlightForOneSecondCoroutine (note_obj));
//		}
//
//		//A coroutine meant to be called within highlightForOneSecond()
//		private IEnumerator highlightForOneSecondCoroutine(GameObject note_obj){
//			note_obj.GetComponent<NoteBehavior> ().makeSolid ();
//
//			yield return new WaitForSeconds (1.0f);
//
//			note_obj.GetComponent<NoteBehavior> ().makeGhost ();
//
//		}

	}
}