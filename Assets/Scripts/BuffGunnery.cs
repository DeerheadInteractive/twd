using UnityEngine;
using System.Collections;

public class BuffGunnery : Gunnery {
	public int targets;
	public float dmgUp = 1.2f;

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
		//if (other.tag != "Untagged" && other.tag != "Enemy")
		//	print (other.tag);
		GameObject obj = other.transform.gameObject;
		if (obj.transform.parent == null)
						return;
//		if (obj.transform.parent.gameObject != null){
//			print ("something has a parent.");
//			GameObject parent = obj.transform.parent.gameObject;
//			Gunnery g = parent.GetComponent<Gunnery>();
//			if (g != null){
//				print ("increasing damageMod");	
//				g.increaseDamageMod(dmgUp);
//			}
//		}
		if (other.tag == "Tower" && obj.transform.parent != null){
			Gunnery g = obj.transform.parent.gameObject.GetComponent<Gunnery>();
			if (!g.isCounted)
				g.increaseDamageMod(dmgUp);
		}
	}
}
