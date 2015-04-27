using UnityEngine;
using System.Collections;

public class ButtonSell : ButtonUpgrade {
	public override int cost(Gunnery gn){
		return -gn.sellValue;
	}
}
