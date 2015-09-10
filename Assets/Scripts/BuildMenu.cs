using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildMenu : MonoBehaviour {

	//public bool singleTarget_isClicked, multiTarget_isClicked, aoe_isClicked, dot_isClicked, debuffSlow_isClicked,
	public bool waitForInput;
	private string selectedTag;

	// Menus
	GameObject buildMenuPanel;
	public GameObject towerInfoPanel;
	public GameObject playerInfoPanel;
	public GameObject objectCamera;

	// Tower Prefabs
	public GameObject singleTargetTower;
	public GameObject multiTargetTower;
	public GameObject aoeTower;
	//GameObject DOTTower;
	public GameObject debuffSlowTower;
	GameObject towerToBuild;

	GameObject selectedObject;
	GameController gc;

	// Use this for initialization
	void Start () {
		gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		objectCamera.SetActive (false);

		// Initialize menu panels
		buildMenuPanel = this.gameObject;
		towerInfoPanel.SetActive (false);
		playerInfoPanel.SetActive (false);

		waitForInput = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			setSelectedObject();

			switch (waitForInput) {
			case true:
				BuildTower();
				waitForInput = false;
				break;
			case false:
				if(getSelectedObjectTag() == "Tower") {
					towerInfoPanel.GetComponent<TowerInfo>().setSelectedTower(getSelectedObject ());
					towerInfoPanel.SetActive(true);
					buildMenuPanel.SetActive(false);
					selectedTag = null;
					selectedObject = null;
				}
				else if(getSelectedObjectTag() == "Player") {
					playerInfoPanel.SetActive(true);
					buildMenuPanel.SetActive(false);
					selectedTag = null;
					selectedObject = null;
				}
				break;
			}
		}
	}

	void OnEnable() {
		objectCamera.SetActive (false);
		towerInfoPanel.SetActive (false);
		playerInfoPanel.SetActive (false);
		selectedObject = null;
	}
	
	void setSelectedObject() {
		RaycastHit hit = new RaycastHit ();
		Vector3 mousePos = Input.mousePosition;

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

		return;
	}

	void BuildTower() {
		//Debug.Log ("Entered BuildTower");

		RaycastHit hit = new RaycastHit();
		
		Vector3 mousePos = Input.mousePosition;
		Vector3 offset = new Vector3 (0, 1, 0);
		
		if (Physics.Raycast (Camera.main.ScreenPointToRay (mousePos), out hit)) {
			//Debug.Log ("Raycast hit something");
			if (getSelectedObjectTag() == "Wall") {
				//Debug.Log ("Raycast hit Wall");
				// Add offset so tower is above walls
				Instantiate (towerToBuild, hit.transform.position+offset, hit.transform.rotation);
				
				// Update money
				gc.updateMoney(-towerToBuild.GetComponent<Gunnery>().getBaseCost());
			}
		}
		
		return;
	}

	string getSelectedObjectTag() {
		return selectedTag;
	}

	GameObject getSelectedObject() {
		return this.selectedObject;
	}

	// Functions called by buttons
	public void singleTargetClicked() {
		// Can player afford the tower?
		if (!gc.canAfford (singleTargetTower.GetComponent<Gunnery>().getBaseCost())) {
			Debug.Log("can't afford single target");
			return;
		}

		towerToBuild = singleTargetTower;
		waitForInput = true;
	}

	public void multiTargetClicked() {
		// Can player afford the tower?
		if (!gc.canAfford (multiTargetTower.GetComponent<Gunnery>().getBaseCost())) {
			Debug.Log("can't afford multi target");
			return;
		}

		towerToBuild = multiTargetTower;
		waitForInput = true;
	}

	public void aoeClicked() {
		// Can player afford the tower?
		if (!gc.canAfford (aoeTower.GetComponent<Gunnery>().getBaseCost())) {
			Debug.Log("can't afford aoe");
			return;
		}
		print ("aoe clicked");
		towerToBuild = aoeTower;
		waitForInput = true;
	}

	public void debuffSlowClicked() {
		// Can player afford the tower?
		if (!gc.canAfford (debuffSlowTower.GetComponent<Gunnery>().getBaseCost())) {
			Debug.Log("can't afford debuff");
			return;
		}

		towerToBuild = debuffSlowTower;
		waitForInput = true;
	}
	
}
