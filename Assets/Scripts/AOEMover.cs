using UnityEngine;
using System.Collections;

public class AOEMover : MonoBehaviour {
	public GameObject target;
	public float aoeRadius;
	public float speed;
	public float lifeTime = 3f;

	private float spawnTime = 0f;
	private ArrayList splashed;

	private Vector3 targetDirection; // Last known direction towards target.

	void Start(){
		spawnTime = Time.time;
	}
	/*
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
		transform.rotation = Quaternion.LookRotation(targetDirection);
		rigidbody.velocity = transform.forward * speed;
	}
	void Explode(Vector3 center, float aoeRadius) {
			var hitColliders = Physics.OverlapSphere(center, aoeRadius);
			for (var i = 0; i < hitColliders.Length; i++) {
			if(hitColliders[i].tag == "ENEMY");
			{
				//call damageObject here?
			}
		}
	}
*/
}