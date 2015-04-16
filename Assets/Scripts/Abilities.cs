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
			if (obj.transform.parent.gameObject != null){
				print ("something has a parent.");
				GameObject parent = obj.transform.parent.gameObject;
				Gunnery g = parent.GetComponent<Gunnery>();
				if (g != null){
					print ("increasing damageMod");	
					g.increaseDamageMod(dmgUp);
				}
			}
		}
		
		controller.enemiesInRange.Clear ();
		controller.towersInRange.Clear ();	

	}
}