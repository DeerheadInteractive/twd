using UnityEngine;
using System.Collections;

public class TargetedCamera : MonoBehaviour {

	public GameObject target;
	
	private float epsilon = 0.2f; // Distance Tolerance
	private float trackingSpeed = 2.0f;
	private float xOffset = 1.0f; // UI Offset
	private float zOffset = 0.5f;

	void Update () {
		if (target != null){
			Vector3 lp = transform.localPosition;
			Vector3	targetDistance = target.transform.localPosition - lp;
			targetDistance.y = 0.0f;
			targetDistance.x += xOffset;
			targetDistance.z += zOffset;
			if (targetDistance.magnitude > epsilon){
				transform.localPosition = new Vector3(
					lp.x + targetDistance.x * Time.deltaTime * trackingSpeed,
					lp.y,
					lp.z + targetDistance.z * Time.deltaTime * trackingSpeed);
			} else{
				// Don't move
			}
		}
	}
}
