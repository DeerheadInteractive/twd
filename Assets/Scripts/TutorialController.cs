using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour {
	public int numPanels;
	private int curPanel = 0;
	public Animator animator;
	// Button Handlers ------------------------------------------------
	public void BackPressed(){
		if (curPanel == 0){

		} else{
			curPanel--;
			animator.SetInteger("Panel", curPanel);
		}
	}
	public void NextPressed(){

	}
}
