using UnityEngine;
using System.Collections;

public class ButtonDamageUpgrade : ButtonUpgrade {
	public override int cost(Gunnery gn){
        return gn.getUpgradeCost(TowerData.ATTRIBUTE.DAMAGE);
	}
}
