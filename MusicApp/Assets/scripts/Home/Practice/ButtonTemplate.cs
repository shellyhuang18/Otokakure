using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ToastPlugin;

//This class serves as a template for dynamically generated buttons on practice page
public class ButtonTemplate : MonoBehaviour {
	public Button button;
	public Text button_text;

	public void PracticeInfo (){
		ToastHelper.ShowToast(button_text.text.ToString(), true);
	}
}