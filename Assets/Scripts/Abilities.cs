using UnityEngine;
using System.Collections;

public class Abilities : MonoBehaviour 
{
	PlayerController controller;
	public float dmgUp;
	public float slowAmount;
	public float slowCooldown;
	private float curSlowCooldown;
	public float slowDuration;
	public GameObject slowVisualEffect;

	// Upgrade values
	public float slowUpgrade;
	public float damageUpgrade;
	public int rangeUpgrade;
	public float cooldownUpgrade;
	public int slowUpgradeValue;
	public int damageUpgradeValue;
	public int rangeUpgradeValue;
	public int cooldownUpgradeValue;

	void Start () 
	{

		controller = transform.gameObject.GetComponent<PlayerController> ();
		if (controller == null){
			print ("no controlllerr");
		}
	}

	void Update () 
	{
		// Active slow
		if (Input.GetKeyDown (KeyCode.Q) && curSlowCooldown <= 0) 
		{
			curSlowCooldown = slowCooldown;
			foreach (GameObject enemy in controller.enemiesInRange){
				enemy.GetComponent<Vida>().temporarySlow(slowAmount, slowDuration);
			}
			GameObject slowSphere = Instantiate(slowVisualEffect, transform.position, Quaternion.identity) as GameObject;
			RotateMover mover = slowSphere.GetComponent<RotateMover>();
			mover.targetRadius = transform.gameObject.GetComponent<SphereCollider>().radius;
		}
		curSlowCooldown -= Time.deltaTime;

		// Passive buff
		foreach (GameObject obj in controller.towersInRange){
			if (obj.transform.parent != null && obj.transform.parent.gameObject != null){
				print ("something has a parent.");
				GameObject parent = obj.transform.parent.gameObject;
				Gunnery g = parent.GetComponent<Gunnery>();
				if (g != null){
					print ("increasing damageMod");	
					g.increaseDamageMod(dmgUp);
					g.isCounted = false;
				}
			}
		}
		print (controller.towersInRange.Count);
		controller.enemiesInRange.Clear ();
		controller.towersInRange.Clear ();	

	}

	public void upgrade(int choice) {
		switch (choice) {
		// Upgrade damage buff
		case 0:
			dmgUp += damageUpgrade;
			damageUpgradeValue += 50;
			break;
		// Upgrade slow debuff
		case 1:
			slowAmount -= slowUpgrade;
			slowUpgradeValue += 100;
			break;
		// Upgrade cooldown
		case 2:
			slowCooldown -= cooldownUpgrade;
			cooldownUpgradeValue += 150;
			break;
		// Upgrade range
		case 3:
			this.gameObject.GetComponent<SphereCollider>().radius += rangeUpgrade;
			rangeUpgradeValue += 75;
			break;
		}

		return;
	}
}