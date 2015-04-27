using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

	public AudioSource music;
	public AudioSource confirm;
	public AudioSource pause;

	private float pausedVolume = 0.30f;

	public GameObject tooltip;
	public GameObject gameOverPanel;
	public Text gameOverText;
	public GameObject pausePanel;
	public GameObject swapFader;
	public Animator animator;

	private bool isSwapping;
	private State curState;

	private int lastCost;

	private enum State{
		GAME_OVER, PLAY, TO_LEVEL, TO_MAIN, PAUSE
	};

	public void Start(){
		music.volume = Globals.musicVolume;
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

		GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		if (!gc.canAfford(lastCost))
			tooltip.GetComponent<ObjectHolder>().obj1.GetComponent<Text>().color = Color.red;
		else
			tooltip.GetComponent<ObjectHolder>().obj1.GetComponent<Text>().color = Color.black;
	}

	public void showTooltip(Vector3 pos, int cost){
		lastCost = cost;
		Text txt = 	tooltip.GetComponent<ObjectHolder>().obj1.GetComponent<Text>();
		if (cost > 0)
			txt.text = "Cost: " + cost.ToString();
		else
			txt.text = "Value: " + (-cost).ToString();
		GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		if (!gc.canAfford(cost))
			txt.color = Color.red;
		else
			txt.color = Color.black;
		//tooltip.GetComponent<Text>().text = "Cost: " + cost;
		tooltip.GetComponent<RectTransform>().position = new Vector3(pos.x - 115, pos.y, pos.z);
		tooltip.SetActive(true);
	}

	public void hideTooltip(){
		tooltip.SetActive(false);
	}

	public void GameOver(bool win){
		if (win){
			gameOverText.text = "You won!";
		} else{
			gameOverText.text = "You failed...";
		}
		animator.SetTrigger("Swap");
		curState = State.GAME_OVER;
	}

	public void Pause(){
		//pause.Play();
		if (curState == State.PAUSE){
			clickedResume();
			music.volume = Globals.musicVolume;
		} else if (curState == State.PLAY){
			pause.Play();
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
		pause.Play();
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
