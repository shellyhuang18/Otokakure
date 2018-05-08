using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScene : MonoBehaviour
{
	[SerializeField]
	private float alpha_incrementer = 0.01f;

//	[SerializeField]
//	GameObject curtain_prefab;

	public void startTransition(string next_scene, int min_loading_time = 4){


		StartCoroutine (loadingScreenCoroutine (next_scene, min_loading_time));


	}


	//A couroutine meant to be called within startTransition
	private IEnumerator loadingScreenCoroutine(string next_scene, int minimum_loading_time){
		
		GameObject curtain_prefab = Resources.Load("LoadingScreen/curtain") as GameObject;

		//Fade out of the current scene
		GameObject lower_curtain = Instantiate(curtain_prefab);
		lower_curtain.transform.SetParent((GameObject.Find("Canvas")).transform, false);
		lower_curtain.GetComponent<Image> ().color = new Color (0, 0, 0, 0);

		Color new_color = lower_curtain.GetComponent<Image> ().color;
		this.alpha_incrementer = 0.1f;
		while(lower_curtain.GetComponent<Image>().color.a < 1){
			
			new_color.a = new_color.a + alpha_incrementer;
			lower_curtain.GetComponent<Image> ().color = new_color;
			yield return new WaitForSeconds (0.01f);
		}

		//Now load the loading screen
		DontDestroyOnLoad (this.gameObject);
		SceneManager.LoadScene ("LoadingScreen");

		int time = 0;
		while((time < minimum_loading_time)) {
			time += 1;
			yield return new WaitForSeconds (1);
		}


		//Load final scene
		SceneManager.LoadScene (next_scene);

//		GameObject raise_curtain;
//		raise_curtain = Instantiate (curtain_prefab) as GameObject;
//		raise_curtain.transform.SetParent ((GameObject.Find ("Canvas")).transform, false);
//		raise_curtain.GetComponent<Image> ().color = new Color (0, 0, 0, 1);

		//Fade into the final scene
//
//
//
//		Color color = raise_curtain.GetComponent<Image> ().color;
//		this.alpha_incrementer = 0.1f;
//
//		while (color.a > 0f) {
//			color.a = color.a - alpha_incrementer;
//			Destroy (raise_curtain);
//			raise_curtain = Instantiate (curtain_prefab) as GameObject;
//			raise_curtain.transform.SetParent ((GameObject.Find ("Canvas")).transform, false);
//
//			raise_curtain.GetComponent<Image> ().color = color;
//			Debug.Log (raise_curtain.GetComponent<Image> ().color);
//
////			Debug.Log ("color var: " + color);
////			Debug.Log ("gameobject color: " + raise_curtain);
////
////
////			if (raise_curtain != null) {
////				color.a = color.a - alpha_incrementer;
////				Debug.Log ("not null");
////				raise_curtain.GetComponent<Image> ().color = color;
////
////			}
//
//
//			yield return new WaitForSeconds(0.1f);
//
//		}
//

//		 Destroy (curtain); 
		Destroy (gameObject); //Destroy this script

	}
		
}

