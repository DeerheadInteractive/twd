using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildMenu : MonoBehaviour {

	public bool singleTarget_isClicked, multiTarget_isClicked, waitForInput;
	private string selectedTag;

	// Menus
	GameObject buildMenuPanel;
	public GameObject towerInfoPanel;
	public GameObject playerInfoPanel;
	public GameObject objectCamera;

	// Tower Prefabs
	GameObject singleTargetTower;
	GameObject multiTargetTower;

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
		if (Input.GetMouseButtonDown (0)) {
			setSelectedObject();

			switch (waitForInput) {
			case true:
				if (singleTarget_isClicked) {
					BuildSingleTarget();
					singleTarget_isClicked = false;
					waitForInput = false;
				} else if (multiTarget_isClicked) {
					BuildMultiTarget();
					multiTarget_isClicked = false;
					waitForInput = false;
				}
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
			/*default:
				Debug.Log ("this is bad");
				break;*/
			}
		}
	}

	/*bool setTower() {
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
	}*/

	// Getter and Setter for selected object tag
	void setSelectedObject() {
		RaycastHit hit = new RaycastHit ();
		Vector3 mousePos = Input.mousePosition;

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

		return;
	}

	string getSelectedObjectTag() {
		return selectedTag;
	}

	GameObject getSelectedObject() {
		return this.selectedObject;
	}

	private void BuildSingleTarget() {
		Debug.Log ("Entered BuildSingleTarget()");
		
		RaycastHit hit = new RaycastHit();

		Vector3 mousePos = Input.mousePosition;
		Vector3 offset = new Vector3 (0, 1, 0);
		
		if (Physics.Raycast (Camera.main.ScreenPointToRay (mousePos), out hit)) {
			Debug.Log ("Raycast hit something");
			if (getSelectedObjectTag() == "Wall") {
				Debug.Log ("Raycast hit Wall");
				// Add offset so tower is above walls
				Instantiate (singleTargetTower, hit.transform.position+offset, hit.transform.rotation);
			}
		}

		// Update money
		gc.updateMoney (-singleTargetTower.GetComponent<Gunnery> ().buyValue);
		
		return;
	}

	private void BuildMultiTarget() {
		RaycastHit hit = new RaycastHit ();
		Vector3 mousePos = Input.mousePosition;
		Vector3 offset = new Vector3 (0, 1, 0);

		if (Physics.Raycast (Camera.main.ScreenPointToRay (mousePos), out hit)) {
			if(getSelectedObjectTag() == "Wall") {
				Instantiate (multiTargetTower, hit.transform.position+offset, hit.transform.rotation);
			}
		}

		// Update money
		gc.updateMoney (-singleTargetTower.GetComponent<Gunnery> ().buyValue);

		return;
	}

	// Functions called by buttons
	public void singleTargetClicked() {
		// Can player afford the tower?
		if (!gc.canAfford (singleTargetTower.GetComponent<Gunnery> ().buyValue)) {
			Debug.Log("can't afford single target");
			return;
		}

		singleTarget_isClicked = true;
		multiTarget_isClicked = false;
		waitForInput = true;
	}

	public void multiTargetClicked() {
		// Can player afford the tower?
		if (!gc.canAfford (multiTargetTower.GetComponent<Gunnery> ().buyValue)) {
			Debug.Log("can't afford multi target");
			return;
		}

		multiTarget_isClicked = true;
		singleTarget_isClicked = false;
		waitForInput = true;
	}
	
}
