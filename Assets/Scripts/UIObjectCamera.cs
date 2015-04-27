using UnityEngine;
using System.Collections;

public class UIObjectCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setCameraPosition(Vector3 newPos) {
		//Debug.Log ("Camera x: " + this.gameObject.transform.position.x + ", Camera y: " + this.gameObject.transform.position.y);
		this.gameObject.transform.position = newPos;
		//Debug.Log ("Camera x: " + this.gameObject.transform.position.x + ", Camera y: " + this.gameObject.transform.position.y);
	}

	public GameObject getObjectCamera() {
		return this.gameObject;
	}

	public void enableCamera() {
		this.gameObject.SetActive (true);
	}

	public void disableCamera() {
		this.gameObject.SetActive (false);
	}
}
