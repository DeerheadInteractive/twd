using UnityEngine;
using System.Collections;

public class SelectorSpinner : MonoBehaviour {
	public float maxRotationSpeed;

	// Use this for initialization
	void Start () {
		rigidbody.angularVelocity = Random.insideUnitSphere * maxRotationSpeed;
	}
}
