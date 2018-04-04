using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomePage : MonoBehaviour {

	public void UserInfoPage (string scene_name) {
		PlayerPrefs.SetString ("lastLoadedScene", SceneManager.GetActiveScene ().name);
		SceneManager.LoadScene (scene_name);
	}

	public void DailyPage (string scene_name) {
		SceneManager.LoadScene (scene_name);
	}

	public void PracticePage (string scene_name) {
		SceneManager.LoadScene (scene_name);
	}

	public void CurriculumPage (string scene_name) {
		SceneManager.LoadScene (scene_name);
	}
}
