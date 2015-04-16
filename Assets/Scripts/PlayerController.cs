using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	GameObject player;
	public float speed;
	public ArrayList enemiesInRange;
	public ArrayList towersInRange;

	void Start() {
		enemiesInRange = new ArrayList();
		towersInRange = new ArrayList();
		player = this.gameObject;
	}

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		Vector3 move = new Vector3(moveHorizontal, 0, moveVertical);
		rigidbody.velocity = move * speed;
	}

	void Update() {

	}

	void OnTriggerStay(Collider other){
		GameObject obj = other.transform.gameObject;
		if (other.tag == "TowerSphere"){
			towersInRange.Add(obj);
		} else if (other.tag == "Enemy"){
			enemiesInRange.Add(obj);
		} else if (other.tag == "Tower"){
			// We are ignoring this.
		}
	}
}
