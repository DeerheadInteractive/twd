using UnityEngine;
using System.Collections;

public class PortalRotator : MonoBehaviour {
	public float rotateSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
	}
}
