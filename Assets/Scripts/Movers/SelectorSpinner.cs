using UnityEngine;
using System.Collections;

public class SelectorSpinner : MonoBehaviour {
	public float maxRotationSpeed;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * maxRotationSpeed;
	}
}
