using UnityEngine;
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
	public GameObject sellText;

	int towerDamage;
	string selectedTag;
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
		objectCamera.SetActive (true);
		newPos = new Vector3 ((float)selectedTower.transform.position.x, objectCamera.transform.position.y, (float)selectedTower.transform.position.z);
		//xPos = selectedTower.transform.position.x;
		//yPos = selectedTower.transform.position.y;
		Debug.Log ("Move to " + newPos);
		objectCamera.GetComponent<UIObjectCamera> ().setCameraPosition (newPos);
		Debug.Log ("moving object camera");

		// Change to appropriate tower name
		towerName.GetComponent<Text>().text = selectedTower.GetComponent<Gunnery> ().towerName;

		// Update stats
		updateStats ();
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
		// Can player afford upgrade?
		if(!gc.canAfford(selectedTower.GetComponent<Gunnery>().upgradeValue)) {
			Debug.Log ("can't afford upgrade");
			return;
		}

		Debug.Log (selectedTower.GetComponent<Gunnery> ().upgradeValue);
		// TODO: Change this to upgradeValue
		// Update money
		gc.updateMoney (-selectedTower.GetComponent<Gunnery> ().upgradeValue);

		// Upgrade tower
		switch (selectedTower.gameObject.name) {
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

		updateStats ();
	}

	public void sellButtonClicked() {
		// Update money
		gc.updateMoney (selectedTower.GetComponent<Gunnery> ().sellValue);

		objectCamera.SetActive (false);
		DestroyImmediate (getSelectedTower ());
		//TODO: Increase money here
		buildMenuPanel.SetActive (true);
		towerInfoPanel.SetActive (false);
		Debug.Log ("End of sell");
	}
		
	void updateStats() {
		Debug.Log ("Updating Stats");
		//Debug.Log ("Damage Text: " + selectedTower.GetComponent<Gunnery> ().damage);
		//Debug.Log ("Range Text: " + selectedTower.GetComponent<SphereCollider> ().radius);
		//Debug.Log ("Sell Text: " + selectedTower.GetComponent<TowerStats> ().value);
		damageText.GetComponent<Text> ().text = "Damage: " + selectedTower.GetComponent<Gunnery> ().damage;
		rangeText.GetComponent<Text> ().text = "Range: " + selectedTower.GetComponent<SphereCollider> ().radius;
		sellText.GetComponent<Text> ().text = "Sell Value: " + selectedTower.GetComponent<Gunnery> ().sellValue;
		return;
	}

}
