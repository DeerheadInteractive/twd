using UnityEngine;
using System.Collections;

public class DebuffGunnery : Gunnery {
	public int targets;
	public int slowMove;
	public int slowMoveUpgrade;
	//int towerNameMark = 1;

	private ArrayList inRange;  
	//private GameObject closestObj;
	//private float closestDistance;

	//private float lastFire;

	void Start () {
		GetComponent<SphereCollider>().radius = range;
		lastFire = 0;
	}

	void Update () {
		// Fire a shot if we can.
		/*if (Time.time > lastFire + fireRate){
			lastFire = Time.time;
			Shoot();
		}*/

		// Clear closest distance and object.
		inRange = new ArrayList();
		/*foreach (Vida target in inRange) {
						print ("slowing update");
						target.speed -= slowMove;
				}*/

	}

	/// <summary>
	/// Shoots a shot at the closest target, if we have a target.
	/// </summary>
	void Shoot(){
		// If we don't have any objects in range, don't shoot.
		if (inRange.Count == 0)
			return;

		/*foreach(GameObject target in inRange) {
			Vector3 direction = Vector3.Normalize(transform.position - target.transform.position);
			GameObject myShot = Instantiate(shot, transform.position, Quaternion.LookRotation(direction)) as GameObject;
			TargetedMover mover = myShot.GetComponent<TargetedMover>();
			mover.speed = shotSpeed;
			mover.target = target;
			Vida shotVida = myShot.GetComponent<Vida>();
			shotVida.damage = damage;
			shotVida.owner = Vida.Owner.FRIENDLY;
			if (target.tag == "ENEMY")
			{
				Vida enemy = GameObject.getComponent<Vida>();
				enemy.speed = enemy.baseSpeed - slowMove;
			}
		}*/
		/*foreach (Vida target in inRange) {
			print("slowing shot");
				target.speed = target.baseSpeed - slowMove;
		}*/
	}

	/// <summary>
	/// Sets the given object to be our next target if it is the closest object.
	/// </summary>
	/// <param name="obj">An object within firing range</param>
	/// 
	public void RadiusDetected(GameObject obj){
		inRange.Add (obj);
	}

	void OnTriggerStay(Collider other){
		GameObject obj = other.transform.gameObject;
		if (other.tag == "Enemy"){
			RadiusDetected(obj);
			print("slowing onstay");
			other.GetComponent<Vida>().speed = other.GetComponent<Vida>().baseSpeed - slowMove;
		}
	}

	void upgrade(int choice) {
		switch (choice) {
			// Upgrade damage
		case 0:
			slowMove += slowMoveUpgrade;
			slowMoveUpgrade *= 2;
			break;
			// Upgrade range
		case 1:
			range += rangeUpgrade;
			rangeUpgrade *= 2;
			break;
		// Upgrade rate of fire
		case 2:
			fireRate += rateOfFireUpgrade;
			rateOfFireUpgrade *= 3;
			break;
		}

		towerNameMark++;
		towerName = towerName.Substring (0, towerName.Length - 1) + towerNameMark.ToString ();
	}
}
