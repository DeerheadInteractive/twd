using UnityEngine;
using System.Collections;

public class ButtonPlayerDamage : ButtonPlayer {
	protected override int cost(){
		return abilities.damageUpgradeValue;
	}
}
