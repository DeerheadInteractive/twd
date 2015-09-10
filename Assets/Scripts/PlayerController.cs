using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	public ArrayList enemiesInRange;
	public ArrayList towersInRange;

	void Start() {
		enemiesInRange = new ArrayList();
		towersInRange = new ArrayList();
	}

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		Vector3 move = new Vector3(moveHorizontal, 0, moveVertical);
		GetComponent<Rigidbody>().velocity = move * speed;
	}

	void Update() {

	}

	void OnTriggerStay(Collider other){
		GameObject obj = other.transform.gameObject;
		if (other.tag == "Tower" && obj.transform.parent != null){
			Gunnery g = obj.transform.parent.gameObject.GetComponent<Gunnery>();
			if (!g.isCounted)
				towersInRange.Add(obj);
			g.isCounted = true;
		} else if (other.tag == "Enemy"){
			enemiesInRange.Add(obj);
		}/* else if (other.tag == "Tower"){
			// We are ignoring this.
		}*/
	}
}
