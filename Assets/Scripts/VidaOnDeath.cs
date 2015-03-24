using UnityEngine;
using System.Collections;

public class VidaOnDeath : MonoBehaviour {
	public int curHP = 1, maxHP = 1;
	public int damage = 1;
	public float speed;
	public Owner owner;
	public GameObject spawned;
	public int bounty;
	public enum Owner{
		FRIENDLY, ENEMY, NEUTRAL
	}
	void Start(){
		curHP = maxHP;//Can we remove this or add a tag so we can add wounded monsters?
	}
	void OnTriggerEnter(Collider other){
		if (other.tag == "Boundary" || other.tag == "Tower")
			return;
		GameObject curObj = GetComponent<Collider>().gameObject;
		GameObject otherObj = other.gameObject;
		VidaOnDeath otherVida = otherObj.GetComponent<VidaOnDeath>();
		if (otherVida == null || owner == otherVida.owner){
			return;
		}
		bool otherDestroyed = DamageObject(otherObj, damage);
		bool thisDestroyed = DamageObject(curObj, otherVida.damage);
		if (otherDestroyed && thisDestroyed){
			// this is here to remove warnings lol. I'll use them for real later.
		}
		/* 
		 * Code for spawning explosions.
		if (otherDestroyed && other.tag == "Enemy"){
			GameObject exp = Instantiate(other.GetComponent<EnemyController>().explosion, other.transform.position, Quaternion.identity) as GameObject;
			Destroy(exp, 5);
		} else if (thisDestroyed && this.tag == "Enemy"){
			GameObject exp = Instantiate(GetComponent<EnemyController>().explosion, transform.position, Quaternion.identity) as GameObject;
			Destroy(exp, 5);
			gameController.increaseScore(100);
		}
		 */
	}

	bool DamageObject(GameObject obj, int damage){
		VidaOnDeath vidaOD = obj.GetComponent<VidaOnDeath>();
		vidaOD.curHP -= damage;
		if (vidaOD.curHP < 0)
			vidaOD.curHP = 0;
		if (vidaOD.curHP == 0){
			Destroy (obj);
			if (spawned == null)
				return true;
			GameObject spawn = Instantiate(spawned, vidaOD.transform.position, Quaternion.identity) as GameObject;
			spawn.GetComponent<WaypointMover>().waypoints = vidaOD.transform.gameObject.GetComponent<WaypointMover>().waypoints;
			return true;
		}
		return false;
	}
}
