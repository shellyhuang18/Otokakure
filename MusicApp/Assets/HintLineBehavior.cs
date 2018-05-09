using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HintLine{
public class HintLineBehavior : MonoBehaviour {


		void onTriggerEnter(Collider2D collider){
			if(collider.gameObject.tag == "MusicalNote" ){
				collider.gameObject.GetComponent<NoteBehavior> ().playAudio();
			}
		}

		public void enableHintLine(){
			gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		}

		public void disableHintLine(){
			gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		}


	}
}