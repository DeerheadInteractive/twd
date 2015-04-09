using UnityEngine;
using System.Collections;

public class TowerInfo : MonoBehaviour {

	GameObject towerInfoPanel;
	public GameObject buildMenuPanel;
	public GameObject playerInfoPanel;
	private GameObject selectedTower;

	int towerDamage;
	string selectedTag;
	GameObject selectedObject;

	RaycastHit hit;
	Vector3 mousePos;

	string selectedTowerType; // This will be Tower(Clone) or MultiShotTower(Clone)

	// Use this for initialization
	void Start () {
		towerInfoPanel = this.gameObject;
		towerDamage = selectedTower.GetComponent<Gunnery> ().damage;
		//buildMenuPanel = GameObject.Find ("Build");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			setSelectedObject();

			if(getSelectedObjectTag() == "Tower") {
				setSelectedTower(getSelectedObject ());
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
	
	string getSelectedObjectTag() {
		return selectedTag;
	}
	
	GameObject getSelectedObject() {
		return this.selectedObject;
	}

	public void backButtonClicked() {
		buildMenuPanel.SetActive (true);
		towerInfoPanel.SetActive (false);
	}

	public void upgradeButtonClicked() {
		switch (selectedTowerType) {
			case "Tower(Clone)":
			// Update tower damage
			selectedTower.GetComponent<Gunnery>().updateDamage(5);

			// Return to build menu
			buildMenuPanel.SetActive(true);
			towerInfoPanel.SetActive(false);
			break;

			case "MultiShotTower(Clone)":
			// Update tower damage
			selectedTower.GetComponent<MultiShotGunnery>().updateDamage (2);

			// Return to build menu
			buildMenuPanel.SetActive(true);
			towerInfoPanel.SetActive(false);				
			break;

			default:
			Debug.Log("this is bad");
			break;
		}
	}

	public void sellButtonClicked() {
		getSelectedTower ().SetActive (false);
		//DestroyImmediate (getSelectedTower ());
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
		this.selectedTowerType = tower.name;
		Debug.Log ("Select tower name: " + this.selectedTowerType);
	}

	public GameObject getSelectedTower() {
		return this.selectedTower;
	}

}
