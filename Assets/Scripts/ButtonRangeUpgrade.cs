using UnityEngine;
using System.Collections;

public class ButtonRangeUpgrade : ButtonUpgrade {
	public override int cost(Gunnery gn){
		return gn.rangeUpgradeValue;
	}
}
