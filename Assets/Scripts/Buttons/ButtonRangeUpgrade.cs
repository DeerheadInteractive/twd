using UnityEngine;
using System.Collections;

public class ButtonRangeUpgrade : ButtonUpgrade {
	public override int cost(Gunnery gn){
		return gn.getUpgradeCost(TowerData.ATTRIBUTE.RANGE);
	}
}
