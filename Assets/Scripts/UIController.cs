using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
	public AudioSource music;
	public AudioSource confirm;
	public GameObject gameOverPanel;
	public GameObject swapFader;
	public Animator animator;

	private bool isSwapping;
	private State curState;

	private enum State{
		GAME_OVER, PLAY, TO_LEVEL, TO_MAIN
	};

	public void Start(){
		curState = State.PLAY;
		isSwapping = true;
	}

	public void Update(){
		float swapAlpha = swapFader.GetComponent<Image>().color.a;
		
		if (curState == State.TO_LEVEL)
			music.volume = 1.0f - swapAlpha;
		
		if (!isSwapping && swapAlpha > 0.99f){
			isSwapping = true;
			animator.SetTrigger("Swap");
			switch (curState){
			case State.GAME_OVER:
				gameOverPanel.SetActive(true);
				break;
			case State.PLAY:
				gameOverPanel.SetActive(false);
				break;
			case State.TO_LEVEL:
				Application.LoadLevel(1);
				break;
			case State.TO_MAIN:
				Application.LoadLevel(0);
				break;
			}
		}
		
		if (isSwapping && swapAlpha < 0.01f){
			// Done swapping
			isSwapping = false;
		}
	}

	public void GameOver(bool win){
		if (win){
			print ("You won!");
		} else{
			print ("You lost.");
		}
		animator.SetTrigger("Swap");
		curState = State.GAME_OVER;
	}

	// Button Handlers ----------------------------------------------------

	public void clickedResume(){
		confirm.Play();
		animator.SetTrigger("Swap");
		curState = State.PLAY;
	}
	public void clickedRestart(){
		confirm.Play();
		animator.SetTrigger("Swap");
		curState = State.TO_LEVEL;
	}
	public void clickedToMain(){
		confirm.Play();
		animator.SetTrigger("Swap");
		curState = State.TO_MAIN;
	}
}
