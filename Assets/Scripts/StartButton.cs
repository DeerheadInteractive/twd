using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour 
{
	public GameController gc;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	public void beginWave()
	{
		gc.launch ();
	}
}
