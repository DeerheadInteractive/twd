using UnityEngine;
using System.Collections;

public class UIObjectCamera : MonoBehaviour {

	GameObject objectCamera;

	// Use this for initialization
	void Start () {
		objectCamera = this.gameObject;
		//objectCamera.SetActive (false);
		//Debug.Log ("object camera disabled");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setCameraPosition(Vector3 newPos) {
		Debug.Log ("Camera x: " + objectCamera.transform.position.x + ", Camera y: " + objectCamera.transform.position.y);
		objectCamera.gameObject.transform.position = newPos;
		Debug.Log ("Camera x: " + objectCamera.transform.position.x + ", Camera y: " + objectCamera.transform.position.y);
	}

	public GameObject getObjectCamera() {
		return this.objectCamera;
	}

	public void enableCamera() {
		this.objectCamera.SetActive (true);
	}

	public void disableCamera() {
		this.objectCamera.SetActive (false);
	}
}
