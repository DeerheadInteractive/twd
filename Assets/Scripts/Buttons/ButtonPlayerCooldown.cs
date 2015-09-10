using UnityEngine;
using System.Collections;

public class ButtonPlayerCooldown : ButtonPlayer {
	protected override int cost(){
		return abilities.cooldownUpgradeValue;
	}
}
