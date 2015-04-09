using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

	GameObject playerInfoPanel;
	public GameObject buildMenuPanel;
	public GameObject towerInfoPanel;
	private GameObject selectedTower;

	string selectedTag;
	GameObject selectedObject;

	RaycastHit hit;
	Vector3 mousePos;

	// Use this for initialization
	void Start () {
		playerInfoPanel = this.gameObject;;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			setSelectedObject();

			if(getSelectedObjectTag() == "Tower") {
				towerInfoPanel.GetComponent<TowerInfo>().setSelectedTower(getSelectedObject ());
				towerInfoPanel.SetActive(true);
				playerInfoPanel.SetActive(false);
				selectedTag = null;
				selectedObject = null;
			}
			// TODO: Enemies here
			return;
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

	string getSelectedObjectTag() {
		return selectedTag;
	}
	
	GameObject getSelectedObject() {
		return selectedObject;
	}

	public void backButtonClicked() {
		buildMenuPanel.SetActive (true);
		playerInfoPanel.SetActive (false);
		selectedTag = null;
		selectedObject = null;
	}

	public void upgradeButtonClicked() {
		
	}

}
