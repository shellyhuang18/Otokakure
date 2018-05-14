using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataAnalytics;
using Manager = Communication.Manager;

public class GenderSelect : MonoBehaviour {

	DataAnalysis table = new DataAnalysis();
	void OnSubmit(string scene){
		
		string choice = gameObject.GetComponent<Button> ().name;
		if (choice == "Male") {
			//average range for male, also change in user preferences 
			table.updateDatabase("HigherRange", "a2");
			table.updateDatabase("LowerRange", "a4");


		}
		if (choice == "Female") {
			table.updateDatabase("HigherRange", "a3");
			table.updateDatabase("LowerRange", "a5");
		}

		Manager.setTutorialStatus (true);
		Manager.transitionTo ("test");

	}
}
