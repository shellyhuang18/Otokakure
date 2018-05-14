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
	public class Manager: MonoBehaviour{
		public struct Exercise{
			public BaseModule module;
			public int repetitions;

			public Exercise(BaseModule module, int rep){
				this.module = module;
				this.repetitions = rep;
			}

		}

		private static Queue<Exercise> queue = new Queue<Exercise>();
		private static bool tutorial_mode = false;

		public static void setTutorialStatus(bool val){
			tutorial_mode = val;
		}

		public static bool getTutorialStatus(){
			return tutorial_mode;
		}

		public static BaseModule getCurrentModule(){
			if (queue.Count != 0) {
				return queue.Peek ().module;
			}
			return null;
		}

		public static int getCurrentRepetition(){
			if (queue.Count != 0) {
				return queue.Peek ().repetitions;
			}
			return 0;
		}

		//Dequeues the current exercise, updates manager info
		public static void nextExercise(){
			if (queue.Count != 0) {
				Exercise curr_ex = queue.Dequeue ();
			}
		}

		public static void addExercise(BaseModule module, int repetitions){
			queue.Enqueue (new Exercise(module,repetitions));
		}

		//Clears everything the manager is currently tracking
		public static void clearQueue(){
			queue.Clear ();
		}

		public static void transitionTo(string scene_name){
			GameObject n = Instantiate (Resources.Load ("LoadingScreen/SceneTransition")) as GameObject;
			n.GetComponent<TransitionScene> ().startTransition (scene_name);
		}
			

		//Generates a random song depending on the current module the manager is using.
		public static Song generateSong(){
			string total_sfs = "";
			for(int i=0; i<getCurrentRepetition(); i++ ){
				total_sfs += getCurrentModule().generateSFS ();
			}
			return new Song(total_sfs);
		}

		public static int getQueueLength(){
			return queue.Count;
		}

	}
}
