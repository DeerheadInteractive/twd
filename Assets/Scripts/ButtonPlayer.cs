using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonPlayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public GameObject playerInfo;
	protected GameObject player;
	protected Abilities abilities;
	
	protected virtual int cost(){
		return abilities.rangeUpgradeValue;
	}
	
	public void OnPointerEnter(PointerEventData data){
		if (player == null)
			player = playerInfo.GetComponent<PlayerInfo>().player;
		if (abilities == null)
			abilities = player.GetComponent<Abilities>();
		UIController uc = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
		uc.showTooltip(transform.position, cost());
	}
	
	public void OnPointerExit(PointerEventData data){
		UIController uc = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
		uc.hideTooltip();
	}
}
