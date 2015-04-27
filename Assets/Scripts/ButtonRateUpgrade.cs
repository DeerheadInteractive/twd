using UnityEngine;
using System.Collections;

public class ButtonRateUpgrade : ButtonUpgrade {
	public override int cost(Gunnery gn){
		return gn.rateOfFireUpgradeValue;
	}
}
