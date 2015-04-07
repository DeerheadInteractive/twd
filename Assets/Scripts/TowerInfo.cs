using UnityEngine;
using System.Collections;

public class TowerInfo : MonoBehaviour {

	GameObject towerInfoPanel;
	public GameObject buildMenuPanel;
	private GameObject selectedTower;

	// Use this for initialization
	void Start () {
		towerInfoPanel = this.gameObject;
		//buildMenuPanel = GameObject.Find ("Build");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void backButtonClicked() {
		buildMenuPanel.SetActive (true);
		towerInfoPanel.SetActive (false);
	}

	public void upgradeButtonClicked() {

	}

	public void sellButtonClicked() {
		DestroyImmediate (getSelectedTower ());
		//TODO: Increase money here
		buildMenuPanel.SetActive (true);
		towerInfoPanel.SetActive (false);
		Debug.Log ("End of sell");
	}

	public void setBuildMenu(GameObject menu) {
		buildMenuPanel = menu;
	}

	// Getter/Setter for selectedTower
	public void setSelectedTower(GameObject tower) {
		this.selectedTower = tower;
	}

	public GameObject getSelectedTower() {
		return this.selectedTower;
	}

}
