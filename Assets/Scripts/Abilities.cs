using UnityEngine;
using UnityEngine.UI;
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

	private GameObject highlight;
	private GameObject fader;

	void Start () 
	{

		controller = transform.gameObject.GetComponent<PlayerController> ();
		highlight = GameObject.FindGameObjectWithTag("AbilityPanel");
		fader = GameObject.FindGameObjectWithTag("AbilityFader");
		if (controller == null){
			print ("Error: Player controller not found.");
		}
	}

	void Update () 
	{
		// Active slow
		if (Input.GetKeyDown (KeyCode.Q) && curSlowCooldown < 0f) 
		{
			curSlowCooldown = slowCooldown;
			foreach (GameObject enemy in controller.enemiesInRange){
				enemy.GetComponent<Vida>().temporarySlow(slowAmount, slowDuration);
			}
			GameObject slowSphere = Instantiate(slowVisualEffect, transform.position, Quaternion.identity) as GameObject;
			RotateMover mover = slowSphere.GetComponent<RotateMover>();
			mover.targetRadius = transform.gameObject.GetComponent<SphereCollider>().radius;
		}

		
		Color c = highlight.GetComponent<Image>().color;
		Color f = fader.GetComponent<Image>().color;
		if (curSlowCooldown < 0f){
			c.a = 1.0f;
			f.a = 1.0f;
		} else{
			c.a = 0.0f;
			f.a = Mathf.Pow(1.0f - (curSlowCooldown / slowCooldown), 2.0f);
		}
		highlight.GetComponent<Image>().color = c;
		fader.GetComponent<Image>().color = f;

		curSlowCooldown -= Time.deltaTime;

		// Passive buff
		foreach (GameObject obj in controller.towersInRange){
			if (obj.transform.parent != null && obj.transform.parent.gameObject != null){
				GameObject parent = obj.transform.parent.gameObject;
				Gunnery g = parent.GetComponent<Gunnery>();
				if (g != null){
					g.increaseDamageMod(dmgUp);
					g.isCounted = false;
				}
			}
		}
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