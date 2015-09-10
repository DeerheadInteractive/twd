using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for tower attributes and behaviors. Holds info about upgrades.
/// </summary>
public class Gunnery : MonoBehaviour {
    protected int damage;
	protected float range;
	protected float rate;

	public GameObject shot;
	public float shotSpeed;

    /// <summary>
    /// Magnitude of the player's damage buff.
    /// </summary>
	public float damageMod;
    /// <summary>
    /// Max duration before player's damage buff fades from this tower.
    /// </summary>
    protected const float damageModDuration = 0.25f;
    /// <summary>
    /// Duration of the last application of the player's damage buff.
    /// </summary>
    protected float lastDamageModTime = 0.0f;
	public bool isCounted;

	public string towerName;

	// These counters keep track of how far along the upgrade tree we are.
    protected int[] upgradeLevels = {0,0,0};

	// Targeting
	protected GameObject closestObj;
	protected float closestDistance;
	protected float lastFire;

	void Start () {
        damage = getDamage();
        range = getRange();
        rate = getRate();
		GetComponent<SphereCollider>().radius = range;
		lastFire = 0;
	}

	void Update () {
        // Reduce damage mod time, or reset damage mod if player's buff duration expired.
        if (lastDamageModTime <= 0.0f){
            damageMod = 1.0f;
        } else{
            lastDamageModTime -= Time.deltaTime;
        }

		// Fire a shot if we can.
		if (Time.time > lastFire + rate){
			lastFire = Time.time;
			Shoot();
		}

		// Clear closest distance and object.
		closestDistance = -1;
		closestObj = null;
		damageMod = 1;
	}

	/// <summary>
	/// Shoots at the closest target, if we have a target.
	/// </summary>
	protected virtual void Shoot(){
		// If we don't have any objects in range, don't shoot.

		Vector3 direction = Vector3.Normalize(transform.position - closestObj.transform.position);

		// Turn turret to face target
		transform.forward = -direction;

		// Create shot
		GameObject myShot = Instantiate(shot, transform.position, Quaternion.LookRotation(direction)) as GameObject;
		TargetedMover mover = myShot.GetComponent<TargetedMover>();
		mover.speed = shotSpeed;
		mover.target = closestObj;
		Vida shotVida = myShot.GetComponent<Vida>();
		shotVida.damage = getModifiedDamage();
		shotVida.owner = Vida.Owner.FRIENDLY;
		GetComponent<AudioSource>().Play();
	}

    /// <summary>
    /// Resets the damage mod value and timer.
    /// </summary>
    /// <param name="val">Damage mod.</param>
	public void resetDamageMod(float val){
		damageMod = val;
        lastDamageModTime = damageModDuration;
	}

	/// <summary>
	/// Sets the given object to be our next target if it is the closest object.
	/// </summary>
	/// <param name="obj">An object within firing range</param>
	public virtual void RadiusDetected(GameObject obj){
		float distance = Vector3.Distance(obj.transform.position, transform.position);
		if ((closestDistance == -1) || 
		    (closestDistance != -1 && closestDistance > distance)){
			closestDistance = distance;
			closestObj = obj;
		}
	}

	protected virtual void OnTriggerStay(Collider other){
		GameObject obj = other.transform.gameObject;
		if (other.tag == "Enemy"){
			GameObject self = GetComponent<Collider>().gameObject;
			self.GetComponent<Gunnery>().RadiusDetected(obj);
		}
	}

    /// <summary>
    /// Upgrade the specified attribute.
    /// </summary>
    /// <param name="attribute">Attribute.</param>
	public virtual void upgrade(TowerData.ATTRIBUTE attribute) {
        if (TowerData.canUpgrade(towerName, attribute, getUpgradeLevel(attribute))){
            upgradeLevels[TowerData.attributeToIndex(attribute)]++;
        }
        // Refresh the upgraded attribute.
        switch (attribute){
            case TowerData.ATTRIBUTE.DAMAGE:
                damage = getDamage();
                break;
            case TowerData.ATTRIBUTE.RANGE:
                range = getRange();
                break;
            case TowerData.ATTRIBUTE.RATE:
                rate = getRate();
                break;
        }
	}

    // Accessors

    /// <summary>
    /// Gets the modified damage. Different towers may implement this differently.
    /// This value will be the displayed value in the TowerInfo panel.
    /// </summary>
    /// <returns>The modified damage.</returns>
    public virtual int getModifiedDamage(){
        return (int)(getDamage() * damageMod);
    }
    /// <summary>
    /// Gets the unmodified damage of the tower.
    /// </summary>
    /// <returns>The damage.</returns>
    public int getDamage(){
        // TODO Include modifiers (player, buff tower)
        return (int)(TowerData.getUpgradeValue(towerName, getUpgradeLevel(TowerData.ATTRIBUTE.DAMAGE),
                                         TowerData.ATTRIBUTE.DAMAGE));
    }

    /// <summary>
    /// Gets the unmodified rate of the tower.
    /// </summary>
    /// <returns>The rate.</returns>
    public float getRate(){
        // TODO Include modifiers (player, buff tower)
        return TowerData.getUpgradeValue(towerName, getUpgradeLevel(TowerData.ATTRIBUTE.RATE),
                                         TowerData.ATTRIBUTE.RATE);
    }

    /// <summary>
    /// Gets the unmodified range of the tower.
    /// </summary>
    /// <returns>The range.</returns>
    public float getRange(){
        // TODO Include modifiers (player, buff tower)
        return TowerData.getUpgradeValue(towerName, getUpgradeLevel(TowerData.ATTRIBUTE.RANGE),
                                         TowerData.ATTRIBUTE.RANGE);
    }

    /// <summary>
    /// Gets the base cost of the tower.
    /// </summary>
    /// <returns>The base cost.</returns>
	public int getBaseCost(){
        return TowerData.getBaseCost(towerName);
	}

    /// <summary>
    /// Calculate and return the sell value of the tower.
    /// </summary>
    /// <returns>The sell value.</returns>
    public int getSellValue(){
        // TODO Actually calculate a reasonable sell value based on base cost and upgrades.
        return 1;
    }

    /// <summary>
    /// Gets the upgrade cost for the given attribute at this tower's upgrade level.
    /// If no more upgrades are available, returns <c>int.MinValue</c>.
    /// </summary>
    /// <returns>The upgrade cost.</returns>
    /// <param name="attribute">Attribute.</param>
    public int getUpgradeCost(TowerData.ATTRIBUTE attribute){
        int level = getUpgradeLevel(attribute);
        if (TowerData.canUpgrade(towerName, attribute, level)){
            return TowerData.getUpgradeCost(towerName, level, attribute);
        }
        return int.MinValue;
    }

    public int getUpgradeLevel(TowerData.ATTRIBUTE attribute){
        return upgradeLevels[(int)attribute];
    }
}
