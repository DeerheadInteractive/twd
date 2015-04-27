using UnityEngine;
using System.Collections;

public class Vida : MonoBehaviour {
	public int curHP = 1, maxHP = 1;
	public int damage = 1;
	public int bounty = 5;
	public int damageToPlayer = 1;
	public float baseSpeed;
	private float tempSlow;
	private float tempSlowDuration;
	private float speedModifier;
	public float speed;
	public Owner owner;
	public GameObject explosion;
	public int regenRate;
	public GameObject spawned;
	public float deathTimer;
	public GameObject healthBar;
	public enum Owner{
		FRIENDLY, ENEMY, NEUTRAL
	}

	void Start(){
		//curHP = maxHP;//Can we remove this or add a tag so we can add wounded monsters?
	}

	void Update(){
		tempSlowDuration -= Time.deltaTime;
		if (tempSlowDuration <= 0)
			tempSlow = 1;
		speed = baseSpeed * speedModifier * tempSlow;
		speedModifier = 1.0f;
		//Vida vida = this;
		//Vector3 screenPos = camera.WorldToScreenPoint(vida.transform.position);
		//vida.GetComponent<TextMesh> ().text = curHP + "/" + maxHP;
		if(healthBar!=null)
		{
			healthBar.GetComponent<TextMesh> ().text = curHP.ToString();
		}

	}

	public void temporarySlow(float val, float duration){
		tempSlow = val;
		tempSlowDuration = duration;
	}

	public void modifySpeed(float val){
		speedModifier *= val;
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Boundary" || other.tag == "Tower")
			return;
		GameObject curObj = GetComponent<Collider>().gameObject;
		GameObject otherObj = other.gameObject;
		Vida otherVida = otherObj.GetComponent<Vida>();
		if (otherVida == null || owner == otherVida.owner){
			return;
		}
		bool otherDestroyed = DamageObject(otherObj, damage);
		bool thisDestroyed = DamageObject(curObj, otherVida.damage);
		if (otherDestroyed && thisDestroyed){
			// this is here to remove warnings lol. I'll use them for real later.
		}
		if (thisDestroyed){
			Explode();
		}
		if (otherDestroyed){
			otherVida.Explode();
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
		Vida vida = obj.GetComponent<Vida>();
		vida.curHP -= damage;
		if (vida.curHP < 0)
			vida.curHP = 0;
		if (vida.curHP == 0){
			GameObject objSpawn = obj.GetComponent<Vida>().spawned;
			if (objSpawn != null){
				GameObject spawn = Instantiate(objSpawn, vida.transform.position, Quaternion.identity) as GameObject;
				WaypointMover spawnMover = spawn.GetComponent<WaypointMover>();
				WaypointMover parentMover = obj.GetComponent<WaypointMover>();
				spawnMover.waypoints = parentMover.waypoints;
				spawnMover.destination = parentMover.destination;
				spawnMover.hasDestination = parentMover.hasDestination;
				if (objSpawn.tag == "Enemy"){
					GameObject gcobj = GameObject.FindGameObjectWithTag("GameController");
					GameController gc = gcobj.GetComponent<GameController>();
					gc.updateMonsterCount(1);
				}
			} /*else if (obj.tag == "Enemy"){
				GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
				gc.updateMonsterCount(-1);
			}*/
			if (obj.tag == "Enemy"){
				GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
				gc.updateMoney(bounty);
			}
			Destroy (obj);
			return true;
		}
		return false;
	}

	void OnDestroy(){
		if (tag == "Enemy"){
			GameObject obj = GameObject.FindGameObjectWithTag("GameController");
			if (obj != null){
				GameController gc = obj.GetComponent<GameController>();
				if (gc != null){
					gc.updateMonsterCount(-1);
				}
			}
		}
	}

	public virtual void Explode(){
		if (explosion != null){
			GameObject ex = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
			Destroy (ex, deathTimer);
			
		}
	}

}
