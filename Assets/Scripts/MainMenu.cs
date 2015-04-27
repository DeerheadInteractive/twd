using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public AudioSource music;
	public AudioSource confirm;
	public GameObject swapFader;
	public GameObject selectPanel;
	public GameObject instrPanel;
	public Animator animator;
	private State curState;
	private string selectedLevel;

	private enum State{
		MAIN, SELECT, TO_LEVEL, TO_INSTR
	};

	private bool isSwapping;

	public void Start(){
		curState = State.MAIN;
		music.volume = Globals.musicVolume;
		StartCoroutine(startMusic(0.3f));
	}

	IEnumerator startMusic(float delay){
		yield return new WaitForSeconds(delay);
		music.volume = Globals.musicVolume;
		music.Play();
	}

	public void Update(){
		float swapAlpha = swapFader.GetComponent<Image>().color.a;

		if (curState == State.TO_LEVEL)
			music.volume = 1.0f - swapAlpha;

		if (!isSwapping && swapAlpha > 0.99f){
			isSwapping = true;
			animator.SetTrigger("Swap");
			switch (curState){
			case State.MAIN:
				selectPanel.SetActive(false);
				instrPanel.SetActive(false);
				break;
			case State.SELECT:
				selectPanel.SetActive(true);
				break;
			case State.TO_LEVEL:
				Application.LoadLevel(1);
				break;
			case State.TO_INSTR:
				instrPanel.SetActive(true);
				break;
			}
		}

		if (isSwapping && swapAlpha < 0.01f){
			// Done swapping
			isSwapping = false;
		}
	}

	// Button Handlers ----------------------------------------------------

	public void clickedPlay(){
		confirm.Play();
		animator.SetTrigger("Swap");
		curState = State.SELECT;
	}
	public void clickedBack(){
		confirm.Play();
		animator.SetTrigger("Swap");
		curState = State.MAIN;
	}
	public void clickedLevel(string name){
		confirm.Play();
		animator.SetTrigger("Swap");
		Globals.levelName = name;
		curState = State.TO_LEVEL;
	}
	public void clickedInstruction(){
		confirm.Play();
		animator.SetTrigger("Swap");
		curState = State.TO_INSTR;
	}
}
