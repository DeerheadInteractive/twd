using UnityEngine;
using System.Collections;

public class TowerInteraction : MonoBehaviour {
	public GameObject selectionIndicator;
	public bool isSelected = false;

	void Start () {
		UpdateSelectionIndicatorColor();
	}

	void OnMouseUp(){
		print ("isSelected = " + isSelected);
		isSelected = !isSelected;
		UpdateSelectionIndicatorColor();
	}
	// Update is called once per frame
	void UpdateSelectionIndicatorColor() {
		float alpha = isSelected ? 0.0f : 1.0f;
		Color last = selectionIndicator.GetComponent<Renderer>().material.color;
		Color next = new Color(last.r, last.g, last.b, alpha);
		selectionIndicator.GetComponent<Renderer>().material.SetColor("_Color", next);
	}
}
