using UnityEngine;
using System.Collections;

public class ButtonSlowUpgrade : ButtonUpgrade {
	public override int cost(Gunnery gn){
		return gn.slowRateUpgradeValue;
	}
}
