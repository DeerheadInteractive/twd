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
	public GameObject rateText;
	public GameObject sellText;
	public GameObject damageUpgradeButtonText;
    public GameObject rangeUpgradeButtonText;
    public GameObject rateUpgradeButtonText;
    public Button damageButton;
    public Button rangeButton;
    public Button rateButton;

	//int towerDamage;
	string selectedTag;
	GameObject selectedObject;
	GameController gc;

	RaycastHit hit;
	Vector3 mousePos;
	Vector3 newPos;

	string selectedTowerType;

	// Use this for initialization
	void Start () {
		gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		towerInfoPanel = this.gameObject;
		//towerDamage = selectedTower.GetComponent<Gunnery>().damage;

		resetButtons();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			setSelectedObject();

			if(getSelectedObjectTag() == "Tower") {
				setSelectedTower(getSelectedObject());
				objectCamera.GetComponent<UIObjectCamera>().setCameraPosition(new Vector3(getSelectedObject().transform.position.x, objectCamera.transform.position.y, getSelectedObject().transform.position.z));
				updateStats();
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
	}

	void OnEnable() {
		if (selectedTower) {
			objectCamera.SetActive (true);
			newPos = new Vector3 ((float)selectedTower.transform.position.x, objectCamera.transform.position.y, (float)selectedTower.transform.position.z);
			objectCamera.GetComponent<UIObjectCamera>().setCameraPosition (newPos);
			resetButtons();
			updateStats ();
		} else {
			Debug.Log ("No selected tower? That's impossible!");
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
		}
	}

	void updateStats() {
        towerName.GetComponent<Text>().text = selectedTower.GetComponent<Gunnery>().towerName;
        damageText.GetComponent<Text>().text = "Damage: " + selectedTower.GetComponent<Gunnery>().getModifiedDamage();
        rangeText.GetComponent<Text>().text = "Range: " + selectedTower.GetComponent<Gunnery>().getRange();
        rateText.GetComponent<Text>().text = "Rate Of Fire: " + selectedTower.GetComponent<Gunnery>().getRate();
        sellText.GetComponent<Text>().text = "Sell Value: " + selectedTower.GetComponent<Gunnery>().getSellValue();

		if (selectedTower.name == "DebuffTower(Clone)") {
			rateText.GetComponent<Text>().text = "Slow Rate: " + ((1.0f - selectedTower.GetComponent<Gunnery>().getRate()) * 100) + "%";
            damageUpgradeButtonText.GetComponent<Text>().text = "---";
        } else{
            damageUpgradeButtonText.GetComponent<Text>().text = "Damage";
        }
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

    /// <summary>
    /// Upgrades an attribute for the selected tower based on the given index.
    /// </summary>
    /// <param name="index">Index of upgrade (see TowerData for index to attribute mapping).</param>
    public void upgradeClicked(int index){
        TowerData.ATTRIBUTE attribute = TowerData.indexToAttribute(index);
        if (selectedTower.name == "DebuffTower(Clone)"){
            if (attribute == TowerData.ATTRIBUTE.DAMAGE){
                return;
            }
        }
        if(!gc.canAfford(selectedTower.GetComponent<Gunnery>().getUpgradeCost(attribute))){
            return;
        }
        gc.updateMoney(-selectedTower.GetComponent<Gunnery>().getUpgradeCost(attribute));
        selectedTower.GetComponent<Gunnery>().upgrade(attribute);
        updateStats();
        resetButtons();
    }

	private void resetButtons(){
        Gunnery gn = selectedTower.GetComponent<Gunnery>();
        // Disable buttons if no more upgrades are avaiable for that attribute.
        damageButton.enabled = gn.getUpgradeCost(TowerData.ATTRIBUTE.DAMAGE) != int.MinValue;
        rangeButton.enabled = gn.getUpgradeCost(TowerData.ATTRIBUTE.RANGE) != int.MinValue;
        rateButton.enabled = gn.getUpgradeCost(TowerData.ATTRIBUTE.RATE) != int.MinValue;
		tooltip.SetActive(false);
	}

    /// <summary>
    /// Sell the selected tower.
    /// </summary>
	public void sellButtonClicked() {
		gc.updateMoney(selectedTower.GetComponent<Gunnery>().getSellValue());

		objectCamera.SetActive (false);
		DestroyImmediate (selectedTower);
		buildMenuPanel.SetActive (true);
		towerInfoPanel.SetActive (false);
	}

	public void backButtonClicked() {
		objectCamera.SetActive (false);
		buildMenuPanel.SetActive (true);
		towerInfoPanel.SetActive (false);
	}

}
