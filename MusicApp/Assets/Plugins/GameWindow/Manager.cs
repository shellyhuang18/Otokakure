using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Module;
using NoteLogic;

/*This static class is responsible for maintaining the queue of modules that the user
 * is expected to complete, as well as the iterations through each module.
 * It is also responsible for communicating between the various modules and 
 * the designated game window.
 * */

public class Manager{
	private static BaseModule current_module; 
	private static Queue<BaseModule> queue;
	private static GameObject game_window;

	//Attempts to assign the Manager a game_window by ID
	public void updateGameWindow(){
		GameObject window_obj = (GameObject)GameObject.FindWithTag ("GameWindow");
		if (window_obj != null) {
			game_window = window_obj; 
		}
	}

	public void nextModule(){
		current_module = queue.Dequeue ();
	}

	public void addModule(BaseModule module){
		queue.Enqueue (module);
	}

	public void clearQueue(){
		queue.Clear ();
	}


	//Attempts to load module by class name. Otherwise returns null
//	private void loadModule(string module_name){
//
//	}

}
