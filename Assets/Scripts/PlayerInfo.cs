using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

	GameObject playerInfoPanel;
	public GameObject buildMenuPanel;
	public GameObject towerInfoPanel;
	public GameObject objectCamera;
	GameObject player;

	string selectedTag;
	GameObject selectedObject;

	RaycastHit hit;
	Vector3 mousePos;
	Vector3 newPos;

	// Use this for initialization
	void Start () {
		playerInfoPanel = this.gameObject;;
	}
	
	// Update is called once per frame
	void Update () {
		updateObjectCamera ();

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

	void OnEnable() {
		if (player) {
			objectCamera.SetActive (true);
			newPos = new Vector3 ((float)player.transform.position.x, objectCamera.transform.position.y, (float)player.transform.position.z);
			Debug.Log ("Move to " + newPos);
			objectCamera.GetComponent<UIObjectCamera> ().setCameraPosition (newPos);
			Debug.Log ("moving object camera");
		} else {
			Debug.Log ("No player");
		}
	}

	void updateObjectCamera() {
		Vector3 update = new Vector3 (player.transform.position.x, objectCamera.transform.position.y, player.transform.position.z);

		objectCamera.transform.position = update;
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

	public void setPlayerObject(GameObject player) {
		this.player = player;
	}

	public void backButtonClicked() {
		objectCamera.SetActive (false);
		buildMenuPanel.SetActive (true);
		playerInfoPanel.SetActive (false);
		selectedTag = null;
		selectedObject = null;
	}

	public void upgradeButtonClicked() {
		
	}

}
