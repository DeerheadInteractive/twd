using UnityEngine;
using System.Collections;

public class Regenerator : MonoBehaviour {

	Vida vid;
	float timer = 0f;
	// Use this for initialization
	void Start () 
	{
		vid = GetComponent<Vida> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;
		if (timer > 0.1f)
		{
			vid.curHP += vid.regenRate;
			timer = 0f;
		}			
	}
}
