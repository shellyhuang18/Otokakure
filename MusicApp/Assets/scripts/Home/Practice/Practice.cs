using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Practice : MonoBehaviour {

	public void GoToHomePage (string scene_name) {
		SceneManager.LoadScene (scene_name);
	}

	public void GoToUserInfoPage (string scene_name) {
		PlayerPrefs.SetString ("lastLoadedScene", SceneManager.GetActiveScene ().name);
		SceneManager.LoadScene (scene_name);
	}
}
