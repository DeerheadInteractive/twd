using UnityEngine;
using System.Collections;

public class Gunnery : MonoBehaviour {
	public float range;
	public float fireRate;
	public int damage;
	public GameObject shot;
	public float shotSpeed;

	protected GameObject closestObj;
	protected float closestDistance;

	protected float lastFire;

	void Start () {
		GetComponent<SphereCollider>().radius = range;
		lastFire = 0;
	}

	void Update () {
		// Fire a shot if we can.
		if (Time.time > lastFire + fireRate){
			lastFire = Time.time;
			Shoot();
		}

		// Clear closest distance and object.
		closestDistance = -1;
		closestObj = null;
	}

	/// <summary>
	/// Shoots a shot at the closest target, if we have a target.
	/// </summary>
	void Shoot(){
		// If we don't have any objects in range, don't shoot.
		if (closestObj == null)
			return;
		
		Vector3 direction = Vector3.Normalize(transform.position - closestObj.transform.position);

		// Turn turret to face target
		transform.forward = -direction;

		// Create shot
		GameObject myShot = Instantiate(shot, transform.position, Quaternion.LookRotation(direction)) as GameObject;
		TargetedMover mover = myShot.GetComponent<TargetedMover>();
		mover.speed = shotSpeed;
		mover.target = closestObj;
		Vida shotVida = myShot.GetComponent<Vida>();
		shotVida.damage = damage;
		shotVida.owner = Vida.Owner.FRIENDLY;
	}

	/// <summary>
	/// Sets the given object to be our next target if it is the closest object.
	/// </summary>
	/// <param name="obj">An object within firing range</param>
	/// 
	public void RadiusDetected(GameObject obj){
		float distance = Vector3.Distance(obj.transform.position, transform.position);
		if ((closestDistance == -1) || 
		    (closestDistance != -1 && closestDistance > distance)){
			closestDistance = distance;
			closestObj = obj;
		}
	}

	void OnTriggerStay(Collider other){
		GameObject obj = other.transform.gameObject;
		if (other.tag == "Enemy"){
			GameObject self = GetComponent<Collider>().gameObject;
			self.GetComponent<Gunnery>().RadiusDetected(obj);
		}
	}
}
