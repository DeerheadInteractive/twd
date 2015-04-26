using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public AudioSource music;
	public AudioSource confirm;
	public GameObject swapFader;
	public GameObject selectPanel;
	public Animator animator;
	private State curState;
	private string selectedLevel;

	private enum State{
		MAIN, SELECT, TOLEVEL
	};

	private bool isSwapping;

	public void Start(){
		curState = State.MAIN;
		music.volume = 0.0f;
		StartCoroutine(startMusic(0.3f));
	}

	IEnumerator startMusic(float delay){
		yield return new WaitForSeconds(delay);
		music.volume = 0.0f;
		music.Play();
	}

	public void Update(){
		float swapAlpha = swapFader.GetComponent<Image>().color.a;
		music.volume = 1.0f - swapAlpha;

		if (!isSwapping && swapAlpha > 0.99f){
			isSwapping = true;
			animator.SetTrigger("Swap");
			switch (curState){
			case State.MAIN:
				curState = State.SELECT;
				selectPanel.SetActive(true);
				break;
			case State.SELECT:
				curState = State.MAIN;
				selectPanel.SetActive(false);
				break;
			case State.TOLEVEL:
				Application.LoadLevel(1);
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
	}
	public void clickedBack(){
		confirm.Play();
		animator.SetTrigger("Swap");
	}
	public void clickedLevel(string name){
		confirm.Play();
		animator.SetTrigger("Swap");
		Globals.levelName = name;
		curState = State.TOLEVEL;
	}
}
