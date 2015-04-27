using UnityEngine;
using System.Collections;

public class MultiShotGunnery : Gunnery {
	public int targets;

	private ArrayList inRange;  
	//private GameObject closestObj;
	//private float closestDistance;

	//private float lastFire;

	void Start () {
		GetComponent<SphereCollider>().radius = range;
		inRange = new ArrayList();
		lastFire = 0;
	}

	void Update () {
		// Fire a shot if we can.
		if (Time.time > lastFire + fireRate){
			lastFire = Time.time;
			Shoot();
		}

		// Clear closest distance and object.
		inRange.Clear();
	}

	/// <summary>
	/// Shoots a shot at the closest target, if we have a target.
	/// </summary>
	void Shoot(){
		// If we don't have any objects in range, don't shoot.
		if (inRange.Count == 0)
			return;

		foreach(GameObject target in inRange) {
			if (target != null){
			Vector3 direction = Vector3.Normalize(transform.position - target.transform.position);
			GameObject myShot = Instantiate(shot, transform.position, Quaternion.LookRotation(direction)) as GameObject;
			TargetedMover mover = myShot.GetComponent<TargetedMover>();
			mover.speed = shotSpeed;
			mover.target = target;
			Vida shotVida = myShot.GetComponent<Vida>();
			shotVida.damage = damage;
			shotVida.owner = Vida.Owner.FRIENDLY;
			}
		}
	}

	/// <summary>
	/// Sets the given object to be our next target if it is the closest object.
	/// </summary>
	/// <param name="obj">An object within firing range</param>
	public override void RadiusDetected(GameObject obj){
		if (obj != null)
			inRange.Add (obj);
	}

	protected override void OnTriggerStay(Collider other){
		GameObject obj = other.transform.gameObject;
		if (other.tag == "Enemy"){
			//GameObject self = GetComponent<Collider>().gameObject;
			//self.GetComponent<MultiShotGunnery>().RadiusDetected(obj);
			RadiusDetected(obj);
		}
	}
}
