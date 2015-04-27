using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	public float rotationInterval = 0.2f;
	public float rotationSpeed = 1.0f;

	private float countdown = 0.0f;

	private Quaternion lastRotation;
	// Use this for initialization
	void Start () {
		/*
		lastRotation = Quaternion.Euler(new Vector3(
			Random.Range(-180.0f, 180.0f),
			Random.Range(-180.0f, 180.0f),
			Random.Range(-180.0f, 180.0f)));
		 */
		rigidbody.angularVelocity = new Vector3(0.0f, 1.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		/*
		countdown -= Time.deltaTime;
		if(countdown < 0) { // timer resets at 2, allowing .5 s to do the rotating
			lastRotation = Quaternion.Euler(new Vector3(
				Random.Range(-180.0f, 180.0f),
				Random.Range(-180.0f, 180.0f),
				Random.Range(-180.0f, 180.0f)));
			countdown = rotationInterval;
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, lastRotation, Time.deltaTime * rotationSpeed);
		*/
	}
}
