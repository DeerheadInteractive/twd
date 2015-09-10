using UnityEngine;
using System.Collections;

public class ButtonPlayerRange : ButtonPlayer {
	protected override int cost(){
		return abilities.rangeUpgradeValue;
	}
}
