using UnityEngine;
using System.Collections;

public class ButtonPlayerSlow : ButtonPlayer {
	protected override int cost(){
		return abilities.slowUpgradeValue;
	}
}
