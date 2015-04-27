using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TowerInfo : MonoBehaviour {

	GameObject towerInfoPanel;
	public GameObject buildMenuPanel;
	public GameObject playerInfoPanel;
	public GameObject objectCamera;
	public GameObject selectedTower;
	public GameObject tooltip;

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

	//int towerDamage;
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
		//towerDamage = selectedTower.GetComponent<Gunnery> ().damage;

		resetUpgradeButtons();
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

		updateStats ();

		return;
	}

	void OnEnable() {
		if (selectedTower) {
			objectCamera.SetActive (true);
			newPos = new Vector3 ((float)selectedTower.transform.position.x, objectCamera.transform.position.y, (float)selectedTower.transform.position.z);
			objectCamera.GetComponent<UIObjectCamera> ().setCameraPosition (newPos);

			resetUpgradeButtons();

			// Change to appropriate tower name
			selectedTowerName = selectedTower.name;

			// Update stats
			updateStats ();
		} else {
			//Debug.Log ("No selectedTower");
		}
	}

	void setSelectedObject() {
		hit = new RaycastHit ();
		mousePos = Input.mousePosition;
		
		if (Physics.Raycast (Camera.main.ScreenPointToRay (mousePos), out hit)) {
			selectedTag = hit.collider.gameObject.tag;
			if(hit.collider.gameObject.transform.parent == null) {
				//Debug.Log("no parent");
				selectedObject = hit.collider.gameObject;
			}
			else {
				selectedObject = hit.collider.gameObject.transform.parent.gameObject;
			}
			
			//Debug.Log ("Selected Object Tag: " + selectedTag);
			
		}
		
		//Debug.Log ("End of setSelectedObject");
		
		return;
	}

	void updateStats() {
		//Debug.Log ("Updating Stats");

		towerName.GetComponent<Text> ().text = selectedTower.GetComponent<Gunnery> ().towerName;
		
		if (selectedTower.name == "DebuffTower(Clone)") {
			damageText.GetComponent<Text> ().text = "Slow Rate: " + selectedTower.GetComponent<DebuffGunnery> ().slowMove;
		} else {
			damageText.GetComponent<Text> ().text = "Damage: " + selectedTower.GetComponent<Gunnery> ().damage * selectedTower.GetComponent<Gunnery>().lastDamageMod;
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

		return;
	}

	GameObject getSelectedTower() {
		return this.selectedTower;
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

		return;
	}

	public void damageUpgradeClicked() {
		if(!gc.canAfford(selectedTower.GetComponent<Gunnery>().damageUpgradeValue)) {
			return;
		}
		gc.updateMoney (-selectedTower.GetComponent<Gunnery> ().damageUpgradeValue);

		selectedTower.GetComponent<Gunnery> ().upgrade (0);
		selectedTower.GetComponent<Gunnery> ().damageUpgradeValue += 30;

		resetUpgradeButtons();

		updateStats();

		return;
	}
	
	public void slowRateUpgradeClicked() {
		if(!gc.canAfford(selectedTower.GetComponent<Gunnery>().slowRateUpgradeValue)) {
			return;
		}
		gc.updateMoney (-selectedTower.GetComponent<Gunnery> ().slowRateUpgradeValue);

		selectedTower.GetComponent<DebuffGunnery> ().upgrade (0);
		selectedTower.GetComponent<DebuffGunnery> ().slowRateUpgradeValue += 50;

		resetUpgradeButtons();

		updateStats();
		return;
	}
	
	public void rangeUpgradeClicked() {
		if(!gc.canAfford(selectedTower.GetComponent<Gunnery>().rangeUpgradeValue)) {
			return;
		}
		gc.updateMoney (-selectedTower.GetComponent<Gunnery> ().rangeUpgradeValue);

		selectedTower.GetComponent<Gunnery> ().upgrade (1);
		selectedTower.GetComponent<Gunnery> ().rangeUpgradeValue += 20;

		resetUpgradeButtons();

		updateStats ();

		return;
	}
	
	public void rateOfFireUpgradeClicked() {
		if(!gc.canAfford(selectedTower.GetComponent<Gunnery>().rateOfFireUpgradeValue)) {
			return;
		}
		gc.updateMoney (-selectedTower.GetComponent<Gunnery> ().rateOfFireUpgradeValue);

		selectedTower.GetComponent<Gunnery> ().upgrade (2);
		selectedTower.GetComponent<Gunnery> ().rateOfFireUpgradeValue += 60;

		resetUpgradeButtons();

		updateStats ();

		return;
	}

	private void resetUpgradeButtons(){
		upgradeButton.gameObject.SetActive(true);
		damageUpgradeButton.gameObject.SetActive (false);
		slowUpgradeButton.gameObject.SetActive (false);
		rangeUpgradeButton.gameObject.SetActive (false);
		rateOfFireUpgradeButton.gameObject.SetActive (false);
		tooltip.SetActive(false);
	}

	public void sellButtonClicked() {
		// Update money
		gc.updateMoney (selectedTower.GetComponent<Gunnery> ().sellValue);

		objectCamera.SetActive (false);
		DestroyImmediate (selectedTower);
		//TODO: Increase money here
		buildMenuPanel.SetActive (true);
		towerInfoPanel.SetActive (false);
		//Debug.Log ("End of sell");

		return;
	}

	public void backButtonClicked() {
		objectCamera.SetActive (false);
		buildMenuPanel.SetActive (true);
		towerInfoPanel.SetActive (false);
		
		return;
	}

}
