﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TowerInfo : MonoBehaviour {

	GameObject towerInfoPanel;
	public GameObject buildMenuPanel;
	public GameObject playerInfoPanel;
	public GameObject objectCamera;
	private GameObject selectedTower;

	// Objects for stats panel
	public GameObject towerName;
	public GameObject damageText;
	public GameObject rangeText;
	public GameObject rateOfFireText;
	public GameObject sellText;
	public Button upgradeButton;
	public Button damageUpgradeButton;
	public Button slowUpgradeButton;
	public Button rangeUpgradeButton;
	public Button rateOfFireUpgradeButton;

	int towerDamage;
	string selectedTag;
	string selectedTowerName;
	GameObject selectedObject;
	GameController gc;

	RaycastHit hit;
	Vector3 mousePos;
	Vector3 newPos;

	string selectedTowerType; // This will be Tower(Clone) or MultiShotTower(Clone)

	// Use this for initialization
	void Start () {
		gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		towerInfoPanel = this.gameObject;
		towerDamage = selectedTower.GetComponent<Gunnery> ().damage;

		damageUpgradeButton.gameObject.SetActive (false);
		slowUpgradeButton.gameObject.SetActive (false);
		rangeUpgradeButton.gameObject.SetActive (false);
		rateOfFireUpgradeButton.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			setSelectedObject();

			if(getSelectedObjectTag() == "Tower") {
				setSelectedTower(getSelectedObject ());
				objectCamera.GetComponent<UIObjectCamera>().setCameraPosition(new Vector3(getSelectedObject().transform.position.x, objectCamera.transform.position.y, getSelectedObject().transform.position.z));
				updateStats ();
			}
			else if(getSelectedObjectTag() == "Player") {
				playerInfoPanel.SetActive(true);
				towerInfoPanel.SetActive(false);
				selectedTag = null;
				selectedObject = null;
			}
			// TODO: Enemies here
		}

		return;
	}

	void OnEnable() {
		if (selectedTower) {
			objectCamera.SetActive (true);
			newPos = new Vector3 ((float)selectedTower.transform.position.x, objectCamera.transform.position.y, (float)selectedTower.transform.position.z);
			Debug.Log ("Move to " + newPos);
			objectCamera.GetComponent<UIObjectCamera> ().setCameraPosition (newPos);
			Debug.Log ("moving object camera");

			upgradeButton.gameObject.SetActive(true);
			damageUpgradeButton.gameObject.SetActive (false);
			slowUpgradeButton.gameObject.SetActive (false);
			rangeUpgradeButton.gameObject.SetActive (false);
			rateOfFireUpgradeButton.gameObject.SetActive (false);

			// Change to appropriate tower name
			//towerName.GetComponent<Text> ().text = selectedTower.GetComponent<Gunnery> ().towerName;
			selectedTowerName = selectedTower.name;

			// Update stats
			Debug.Log ("updating stats");
			updateStats ();
		} else {
			Debug.Log ("No selectedTower");
		}
	}

	void setSelectedObject() {
		hit = new RaycastHit ();
		mousePos = Input.mousePosition;
		
		if (Physics.Raycast (Camera.main.ScreenPointToRay (mousePos), out hit)) {
			selectedTag = hit.collider.gameObject.tag;
			if(hit.collider.gameObject.transform.parent == null) {
				Debug.Log("no parent");
				selectedObject = hit.collider.gameObject;
			}
			else {
				selectedObject = hit.collider.gameObject.transform.parent.gameObject;
			}
			
			Debug.Log ("Selected Object Tag: " + selectedTag);
			
		}
		
		Debug.Log ("End of setSelectedObject");
		
		return;
	}

	void updateStats() {
		Debug.Log ("Updating Stats");

		towerName.GetComponent<Text> ().text = selectedTower.GetComponent<Gunnery> ().towerName;
		
		if (selectedTower.name == "DebuffTower(Clone)") {
			damageText.GetComponent<Text> ().text = "Slow Rate: " + selectedTower.GetComponent<DebuffGunnery> ().slowMove;
		} else {
			damageText.GetComponent<Text> ().text = "Damage: " + selectedTower.GetComponent<Gunnery> ().damage;
		}
		
		rangeText.GetComponent<Text> ().text = "Range: " + selectedTower.GetComponent<Gunnery> ().range;
		rateOfFireText.GetComponent<Text> ().text = "Rate Of Fire: " + selectedTower.GetComponent<Gunnery> ().fireRate;
		sellText.GetComponent<Text> ().text = "Sell Value: " + selectedTower.GetComponent<Gunnery> ().sellValue;
		return;
	}
	
	string getSelectedObjectTag() {
		return selectedTag;
	}
	
	GameObject getSelectedObject() {
		return this.selectedObject;
	}

	public void setSelectedTower(GameObject tower) {
		this.selectedTower = tower;
	}

	GameObject getSelectedTower() {
		return this.selectedTower;
	}

	public void backButtonClicked() {
		objectCamera.SetActive (false);
		buildMenuPanel.SetActive (true);
		towerInfoPanel.SetActive (false);
	}

	public void upgradeButtonClicked() {
		upgradeButton.gameObject.SetActive (false);

		// Activate various upgrade buttons
		if (selectedTowerName == "DebuffTower(Clone)") {
			slowUpgradeButton.gameObject.SetActive (true);
		} else {
			damageUpgradeButton.gameObject.SetActive (true);
		}

		rangeUpgradeButton.gameObject.SetActive (true);
		rateOfFireUpgradeButton.gameObject.SetActive (true);
		// Can player afford upgrade?
		/*if(!gc.canAfford(selectedTower.GetComponent<Gunnery>().upgradeValue)) {
			Debug.Log ("can't afford upgrade");
			return;
		}

		Debug.Log (selectedTower.GetComponent<Gunnery> ().upgradeValue);
		// TODO: Change this to upgradeValue
		// Update money
		gc.updateMoney (-selectedTower.GetComponent<Gunnery> ().upgradeValue);

		// TODO: Add buttons to tower menu for damage, range, and rate of fire upgrades
		// Upgrade tower
		switch (selectedTowerName) {
			case "Tower(Clone)":
			// Update tower damage
			selectedTower.GetComponent<Gunnery>().upgrade(0);

			// Return to build menu
			buildMenuPanel.SetActive(true);
			towerInfoPanel.SetActive(false);
			break;

			case "MultiShotTower(Clone)":
			// Update tower damage
			selectedTower.GetComponent<MultiShotGunnery>().upgrade (0);

			// Return to build menu
			buildMenuPanel.SetActive(true);
			towerInfoPanel.SetActive(false);				
			break;

			default:
			Debug.Log("this is bad");
			break;
		}

		updateStats ();
		*/
	}

	public void damageUpgradeClicked() {
		// Can player afford upgrade?
		if(!gc.canAfford(selectedTower.GetComponent<Gunnery>().damageUpgradeValue)) {
			Debug.Log ("can't afford upgrade");
			return;
		}

		Debug.Log (selectedTower.GetComponent<Gunnery> ().damageUpgradeValue);

		gc.updateMoney (-selectedTower.GetComponent<Gunnery> ().damageUpgradeValue);
		selectedTower.GetComponent<Gunnery> ().upgrade (0);

		selectedTower.GetComponent<Gunnery> ().damageUpgradeValue += 30;

		upgradeButton.gameObject.SetActive (true);

		if (selectedTowerName == "DebuffTower(Clone)") {
			slowUpgradeButton.gameObject.SetActive (false);
		} else {
			damageUpgradeButton.gameObject.SetActive (false);
		}

		rangeUpgradeButton.gameObject.SetActive (false);
		rateOfFireUpgradeButton.gameObject.SetActive (false);

		updateStats ();

		return;
	}
	
	public void slowRateUpgradeClicked() {
		// Can player afford upgrade?
		if(!gc.canAfford(selectedTower.GetComponent<Gunnery>().slowRateUpgradeValue)) {
			Debug.Log ("can't afford upgrade");
			return;
		}
		
		Debug.Log (selectedTower.GetComponent<Gunnery> ().slowRateUpgradeValue);
		
		gc.updateMoney (-selectedTower.GetComponent<Gunnery> ().slowRateUpgradeValue);
		selectedTower.GetComponent<DebuffGunnery> ().upgrade (0);

		selectedTower.GetComponent<DebuffGunnery> ().slowRateUpgradeValue += 50;
		
		upgradeButton.gameObject.SetActive (true);
		slowUpgradeButton.gameObject.SetActive (false);
		rangeUpgradeButton.gameObject.SetActive (false);
		rateOfFireUpgradeButton.gameObject.SetActive (false);

		updateStats ();

		return;
	}
	
	public void rangeUpgradeClicked() {
		// Can player afford upgrade?
		if(!gc.canAfford(selectedTower.GetComponent<Gunnery>().rangeUpgradeValue)) {
			Debug.Log ("can't afford upgrade");
			return;
		}
		
		Debug.Log (selectedTower.GetComponent<Gunnery> ().rangeUpgradeValue);
		
		gc.updateMoney (-selectedTower.GetComponent<Gunnery> ().rangeUpgradeValue);
		selectedTower.GetComponent<Gunnery> ().upgrade (1);

		selectedTower.GetComponent<Gunnery> ().rangeUpgradeValue += 20;
		
		upgradeButton.gameObject.SetActive (true);

		if (selectedTowerName == "DebuffTower(Clone)") {
			slowUpgradeButton.gameObject.SetActive (false);
		} else {
			damageUpgradeButton.gameObject.SetActive (false);
		}

		rangeUpgradeButton.gameObject.SetActive (false);
		rateOfFireUpgradeButton.gameObject.SetActive (false);

		updateStats ();

		return;
	}
	
	public void rateOfFireUpgradeClicked() {
		// Can player afford upgrade?
		if(!gc.canAfford(selectedTower.GetComponent<Gunnery>().rateOfFireUpgradeValue)) {
			Debug.Log ("can't afford upgrade");
			return;
		}
		
		Debug.Log (selectedTower.GetComponent<Gunnery> ().rateOfFireUpgradeValue);
		
		gc.updateMoney (-selectedTower.GetComponent<Gunnery> ().rateOfFireUpgradeValue);
		selectedTower.GetComponent<Gunnery> ().upgrade (2);

		selectedTower.GetComponent<Gunnery> ().rateOfFireUpgradeValue += 60;

		upgradeButton.gameObject.SetActive (true);

		if (selectedTowerName == "DebuffTower(Clone)") {
			slowUpgradeButton.gameObject.SetActive (false);
		} else {
			damageUpgradeButton.gameObject.SetActive (false);
		}

		rangeUpgradeButton.gameObject.SetActive (false);
		rateOfFireUpgradeButton.gameObject.SetActive (false);

		updateStats ();

		return;
	}

	public void sellButtonClicked() {
		// Update money
		gc.updateMoney (selectedTower.GetComponent<Gunnery> ().sellValue);

		objectCamera.SetActive (false);
		DestroyImmediate (selectedTower);
		//TODO: Increase money here
		buildMenuPanel.SetActive (true);
		towerInfoPanel.SetActive (false);
		Debug.Log ("End of sell");
	}

}
