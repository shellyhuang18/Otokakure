using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseModule = Module.BaseModule;
using NoteLogic;
using UnityEngine.SceneManagement;
using Song = NoteLogic.NoteLogic.Song;

/*This static class is responsible for maintaining the queue of modules that the user
 * is expected to complete, as well as the iterations through each module.
 * It is also responsible for communicating between the various modules and 
 * the designated game window.
 * */
namespace Communication{
	public class Manager{
		private struct Exercise{
			public BaseModule module;
			public int repetitions;

			public Exercise(BaseModule module, int rep){
				this.module = module;
				this.repetitions = rep;
			}

		}
		private static BaseModule current_module; 
		private static int repetitions; //The repetitions required for the current_module
		private static Queue<Exercise> queue;
		private static GameObject game_window;

		//Attempts to assign the Manager a game_window by ID
		public static void setGameWindow(GameObject curr_game_window){
			game_window = curr_game_window;
		}


		public static BaseModule getCurrentModule(){
			return current_module;
		}

		//Dequeues the current exercise, updates manager info
		public static void nextExercise(){
			Exercise curr_ex = queue.Dequeue ();
			current_module = curr_ex.module;
		}

		public static void addExercise(BaseModule module, int repetitions){
			queue.Enqueue (new Exercise(module,repetitions));
		}

		//Clears everything the manager is currently tracking
		public static void clear(){
			queue.Clear ();
			repetitions = 0;
			current_module = null;
		}

		public static void startSession(){
			SceneManager.LoadScene ("test");
		}

		//Generates a random song depending on the current module the manager is using.
		public static Song generateSong(){
			string total_sfs = "";
			for(int i=0; i<repetitions; i++ ){
				total_sfs += current_module.generateSFS ();
			}
			return new Song(total_sfs);
		}

		public int getQueueLength(){
			return queue.Count;
		}

	}
}
