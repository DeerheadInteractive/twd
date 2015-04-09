using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public void PlayButtonClicked(){
		print ("Moving from Main Menu to Level.");
		Application.LoadLevel(1);
	}
}
