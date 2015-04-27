using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	public float rotationSpeed = 1.0f;
	private Quaternion lastRotation;
	// Use this for initialization
	void Start () {
		/*
		lastRotation = Quaternion.Euler(new Vector3(
			Random.Range(-180.0f, 180.0f),
			Random.Range(-180.0f, 180.0f),
			Random.Range(-180.0f, 180.0f)));
		 */
		rigidbody.angularVelocity = new Vector3(0.0f, rotationSpeed, 0.0f);
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
