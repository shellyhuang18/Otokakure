//Contributor: Jack Chen, Shelly Huang

using UnityEngine;
using System.Collections;
using Random = System.Random;
using UnityEngine.UI;
public class LoadingSceneAnimation : MonoBehaviour
{
	
	void Awake(){
		StartCoroutine (routine());
	}

	private IEnumerator routine(){
		Random generator = new Random();
		int beat = 0;
		string txt = "LOADING ";
		while (true) {
			

			if (beat % 4 == 0) {
				GameObject[] notes = GameObject.FindGameObjectsWithTag ("UINote");
				foreach (GameObject g in notes) {
					g.GetComponent<Image> ().enabled = false;
				}

				txt = "LOADING";

				//clear everything

			} else if (beat % 4 == 1) {
				txt = "LOADING .";
				//pick from first col

				int row = generator.Next (0, 3);
				GameObject.Find ("column_1").transform.GetChild (row).GetComponent<Image> ().enabled = true;


			}else if (beat % 4 == 2) {
				txt = "LOADING . .";
				//pick from second col

				int row = generator.Next (0, 3);
				GameObject.Find ("column_2").transform.GetChild (row).GetComponent<Image> ().enabled = true;

			} 
			else if( beat % 4 == 3){
				txt = "LOADING . . .";
				//pick from third col

				int row = generator.Next (0, 3);
				GameObject.Find ("column_3").transform.GetChild (row).GetComponent<Image> ().enabled = true;

			}
			beat++;
			GameObject.Find ("Text").GetComponent<Text> ().text = txt;

			yield return new WaitForSeconds (.5f);
		}
	}
}

