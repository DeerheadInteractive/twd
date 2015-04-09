using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildMenu : MonoBehaviour {

	public bool singleTarget_isClicked, multiTarget_isClicked, waitForInput;

	// Menus
	GameObject buildMenuPanel;
	public GameObject towerInfoPanel;

	// Tower Prefabs
	GameObject singleTargetTower;
	GameObject multiTargetTower;

	GameObject selectedObject;

	// Use this for initialization
	void Start () {
		// Initialize menu panels
		buildMenuPanel = this.gameObject;
		//towerInfoPanel.GetComponent<TowerInfo>().setBuildMenu (buildMenuPanel);
		towerInfoPanel.SetActive (false);

		// Initialize tower prefabs
		singleTargetTower = (GameObject)Resources.Load ("Tower");
		multiTargetTower = (GameObject)Resources.Load ("MultiShotTower");

		// Initialize bools
		singleTarget_isClicked = false;
		multiTarget_isClicked = false;
		waitForInput = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Check for left click then build single target tower
		if (Input.GetMouseButtonDown (0) && singleTarget_isClicked && waitForInput) {
			Debug.Log ("Entering BuildSingleTarget()");
			BuildSingleTarget ();
			singleTarget_isClicked = false;
			waitForInput = false;
			Debug.Log ("variables = false now");
		}
		// Check for left click, then build multi target tower
		else if (Input.GetMouseButtonDown (0) && multiTarget_isClicked && waitForInput) {
			Debug.Log("Entering BuildMultiTarget()");
			BuildMultiTarget();
			multiTarget_isClicked = false;
			waitForInput = false;
		}
		// Select a tower or enemy
		else if(Input.GetMouseButtonDown(0) && !waitForInput) {
			if(setTower()) {
				//selectedObject = getSelectedObject();
				towerInfoPanel.SetActive(true);
				//towerInfoPanel.GetComponent<TowerInfo>().setSelectedTower(selectedObject);
				buildMenuPanel.SetActive(false);
			}
		}
	}

	bool setTower() {
		RaycastHit hit = new RaycastHit ();
		Vector3 mousePos = Input.mousePosition;

		GameObject rayObj, tower;

		// Did the raycast hit an object?
		if(Physics.Raycast (Camera.main.ScreenPointToRay(mousePos), out hit)) {
			rayObj = hit.collider.gameObject;
			// Was the object clicked a tower?
			if(rayObj.transform.parent.tag == "Tower") {
				tower = rayObj.transform.parent.gameObject;
				towerInfoPanel.GetComponent<TowerInfo>().setSelectedTower(tower);
				return true;
			}
		}

		return false;
	}

	private void BuildSingleTarget() {
		Debug.Log ("Entered BuildSingleTarget()");
		
		RaycastHit hit = new RaycastHit();

		Vector3 mousePos = Input.mousePosition;
		Vector3 offset = new Vector3 (0, 1, 0);
		
		if (Physics.Raycast (Camera.main.ScreenPointToRay (mousePos), out hit)) {
			Debug.Log ("Raycast hit something");
			if (hit.transform.name == "Wall(Clone)") {
				Debug.Log ("Raycast hit Wall(Clone)");
				// Add offset so tower is above walls
				Instantiate (singleTargetTower, hit.transform.position+offset, hit.transform.rotation);
			}
		}

		return;
	}

	private void BuildMultiTarget() {
		RaycastHit hit = new RaycastHit ();
		Vector3 mousePos = Input.mousePosition;
		Vector3 offset = new Vector3 (0, 1, 0);

		if (Physics.Raycast (Camera.main.ScreenPointToRay (mousePos), out hit)) {
			if(hit.transform.name == "Wall(Clone)") {
				Instantiate (multiTargetTower, hit.transform.position+offset, hit.transform.rotation);
			}
		}
	}

	// Functions called by buttons
	public void singleTargetClicked() {
		singleTarget_isClicked = true;
		multiTarget_isClicked = false;
		waitForInput = true;
	}

	public void multiTargetClicked() {
		multiTarget_isClicked = true;
		singleTarget_isClicked = false;
		waitForInput = true;
	}
	
}
