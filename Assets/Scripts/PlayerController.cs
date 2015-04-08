using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	GameObject player;
	public float speed;

	void Start() 
	{
		 player = this.gameObject;
	}

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		Vector3 move = new Vector3(moveHorizontal, 0, moveVertical);
		rigidbody.velocity = move * speed;
	}

	void Update () {

		/*

		if (Input.GetKeyDown (KeyCode.S)) 
		{

			player.transform.position += new Vector3(0, 0, -1);


		}
		if(Input.GetKeyDown(KeyCode.W))
		{
			player.transform.position += new Vector3(0, 0, 1);
		}

		if(Input.GetKeyDown (KeyCode.A))
		{
			player.transform.position += new Vector3(-1, 0, 0); 
		}

		if(Input.GetKeyDown (KeyCode.D))
		{
			player.transform.position += new Vector3(1, 0, 0);
		}
		*/
	
	}
}
