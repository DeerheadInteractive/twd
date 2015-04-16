using UnityEngine;
using System.Collections;

public class TargetedMover : MonoBehaviour {
	public GameObject target;
	public float speed;
	public float lifeTime = 3f;

	private float spawnTime = 0f;

	private Vector3 targetDirection; // Last known direction towards target.

	void Start(){
		spawnTime = Time.time;
	}

	void Update () {
		// Despawn if lifetime expires.
		if (Time.time > (spawnTime + lifeTime)){
			Destroy(transform.gameObject);
			return;
		}
		// Calculate direction towards target. If shot outlives enemy, it will continue in this direction.
		if (target != null){
			targetDirection = target.transform.position - transform.position;
		}
		if (targetDirection != Vector3.zero)
			transform.forward = targetDirection;
		//transform.rotation = Quaternion.LookRotation(targetDirection);
		rigidbody.velocity = transform.forward * speed;
	}
}
