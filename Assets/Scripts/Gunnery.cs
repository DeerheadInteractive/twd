using UnityEngine;
using System.Collections;

public class Gunnery : MonoBehaviour {
	public float range;
	public float fireRate;
	public int damage;
	public GameObject shot;
	public float shotSpeed;

	public float damageMod;
	public float lastDamageMod;
	public bool isCounted;

	public string towerName;
	int towerNameMark = 1;
	public int buyValue;
	public int sellValue;
	public int damageUpgradeValue;
	public int slowRateUpgradeValue;
	public int rangeUpgradeValue;
	public int rateOfFireUpgradeValue;
	public int damageUpgrade;
	public int rangeUpgrade;
	public float rateOfFireUpgrade;

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
		lastDamageMod = damageMod;

		// Clear closest distance and object.
		closestDistance = -1;
		closestObj = null;
		damageMod = 1;
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
		shotVida.damage = (int)((float)damage * damageMod);
		shotVida.owner = Vida.Owner.FRIENDLY;
	}

	public void increaseDamageMod(float val){
		damageMod *= val;
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

	public void upgrade(int choice) {
		switch (choice) {
		// Upgrade damage
		case 0:
			damage += damageUpgrade;
			damageUpgradeValue *= 2;
			break;
		// Upgrade range
		case 1:
			range += rangeUpgrade;
			rangeUpgradeValue *= 2;
			break;
		case 2:
			fireRate -= rateOfFireUpgrade;
			rateOfFireUpgradeValue *= 3;
			break;
		}

		towerNameMark++;
		towerName = towerName.Substring (0, towerName.Length - 1) + towerNameMark.ToString ();
	}

}
