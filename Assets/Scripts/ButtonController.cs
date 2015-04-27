using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public GameObject tower;

	public virtual int cost(Gunnery gn){
		return gn.buyValue;
	}

	public virtual Gunnery getGunnery(){
		return tower.GetComponent<Gunnery>();
	}

	public void OnPointerEnter(PointerEventData data){
		Gunnery gn = getGunnery();
		UIController uc = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
		uc.showTooltip(transform.position, cost(gn));
	}

	public void OnPointerExit(PointerEventData data){
		UIController uc = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
		uc.hideTooltip();
	}
}
