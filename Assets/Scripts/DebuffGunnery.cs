using UnityEngine;
using System.Collections;

public class DebuffGunnery : Gunnery {

	private GameObject vfxSphere;

	public int targets;
	public float slowAmount = 0.8f;
	public float slowUpgrade = -0.1f;

	private ArrayList inRange;

	//private float lastFire;

	void Start () {
		GetComponent<SphereCollider>().radius = range;
		inRange = new ArrayList();
		vfxSphere = transform.gameObject.GetComponent<ObjectHolder>().obj1;
		updateSphere();
	}

	void Update () {
		foreach (GameObject enemy in inRange){
			if (enemy != null)
				enemy.GetComponent<Vida>().modifySpeed(slowAmount);
		}
		inRange.Clear();
		inRange = new ArrayList();
		/*
		if (inRange.Count() > 0)
			print ("WTF? Debuff inRange did not reset");
			*/
	}

	private void updateSphere(){
		vfxSphere.transform.localScale = new Vector3(range * 2, range * 2, range * 2);
	}

	/// <summary>
	/// Shoots a shot at the closest target, if we have a target.
	/// </summary>
	protected override void Shoot(){

	}

	/// <summary>
	/// Sets the given object to be our next target if it is the closest object.
	/// </summary>
	/// <param name="obj">An object within firing range</param>
	/// 
	public override void RadiusDetected(GameObject obj){
		inRange.Add (obj);
	}

	protected override void OnTriggerStay(Collider other){
		GameObject obj = other.transform.gameObject;
		if (other.tag == "Enemy"){
			RadiusDetected(obj);
		}
	}

	public override void upgrade(int choice) {
		switch (choice) {
			// Upgrade damage
		case 0:
			slowAmount += slowUpgrade;
			break;
			// Upgrade range
		case 1:
			range += rangeUpgrade;
			rangeUpgrade *= 2;
			updateSphere();
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
