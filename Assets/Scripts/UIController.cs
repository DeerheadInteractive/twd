using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
	public AudioSource music;
	public AudioSource confirm;
	public GameObject gameOverPanel;
	public GameObject pausePanel;
	public GameObject swapFader;
	public Animator animator;

	private bool isSwapping;
	private State curState;

	private float pausedVolume = 0.35f;
	private float musicVolume = 0.95f;

	private enum State{
		GAME_OVER, PLAY, TO_LEVEL, TO_MAIN, PAUSE
	};

	public void Start(){
		music.volume = musicVolume;
		curState = State.PLAY;
		isSwapping = true;
	}

	public void Update(){
		float swapAlpha = swapFader.GetComponent<Image>().color.a;
		/*
		if (curState == State.PAUSE)
			if (!pausePanel.activeSelf)
				pausePanel.SetActive(true);
		else
			pausePanel.SetActive(false);
			*/
		
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

	public void Pause(){
		//pause.Play();
		if (curState == State.PAUSE){
			clickedResume();
			music.volume = musicVolume;
		} else if (curState == State.PLAY){
			confirm.Play();
			curState = State.PAUSE;
			music.volume = pausedVolume;
			pausePanel.SetActive(true);
		}
	}

	public bool isPaused(){
		return (curState == State.PAUSE);
	}

	// Button Handlers ----------------------------------------------------

	public void clickedResume(){
		confirm.Play();
		curState = State.PLAY;
		pausePanel.SetActive(false);
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
