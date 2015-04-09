using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {


	public void ToMainMenu(){
		print ("Moving from level to main menu.");
		Application.LoadLevel(0);
	}
}
