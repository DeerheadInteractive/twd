using UnityEngine;
using System.Collections;

public class ButtonUpgrade : ButtonController {
	public GameObject towerInfo;
	public override Gunnery getGunnery(){
		tower = towerInfo.GetComponent<TowerInfo>().selectedTower;
		return tower.GetComponent<Gunnery>();
	}
}
