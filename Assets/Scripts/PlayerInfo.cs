using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

	GameObject playerInfoPanel;
	public GameObject buildMenuPanel;
	public GameObject towerInfoPanel;
	public GameObject objectCamera;
	public GameObject player;
	public GameObject tooltip;
	GameController gc;

	string selectedTag;
	GameObject selectedObject;

	RaycastHit hit;
	Vector3 mousePos;
	Vector3 newPos;

	// Stats Text
	public Text damageBuffText;
	public Text slowDebuffText;
	public Text cooldownText;
	public Text rangeText;

	// Upgrade buttons
	public Button upgradeButton;
	public Button damageBuffButton;
	public Button slowDebuffButton;
	public Button cooldownButton;
	public Button rangeButton;

	// Use this for initialization
	void Start () {
		playerInfoPanel = this.gameObject;
		gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		upgradeButton.gameObject.SetActive (true);
		damageBuffButton.gameObject.SetActive (false);
		slowDebuffButton.gameObject.SetActive (false);
		cooldownButton.gameObject.SetActive (false);
		rangeButton.gameObject.SetActive (false);

		updateStats ();
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
		resetButtons();

		if (player) {
			objectCamera.SetActive (true);
			newPos = new Vector3 ((float)player.transform.position.x, objectCamera.transform.position.y, (float)player.transform.position.z);
			objectCamera.GetComponent<UIObjectCamera> ().setCameraPosition (newPos);
		} else {
			Debug.Log ("No player");
		}

		updateStats ();
	}

	void updateObjectCamera() {
		Vector3 update = new Vector3 (player.transform.position.x, objectCamera.transform.position.y, player.transform.position.z);

		objectCamera.transform.position = update;

		return;
	}

	void setSelectedObject() {
		hit = new RaycastHit ();
		mousePos = Input.mousePosition;
		
		if (Physics.Raycast (Camera.main.ScreenPointToRay (mousePos), out hit)) {
			selectedTag = hit.collider.gameObject.tag;
			if(hit.collider.gameObject.transform.parent == null) {
				selectedObject = hit.collider.gameObject;
			}
			else {
				selectedObject = hit.collider.gameObject.transform.parent.gameObject;
			}
		}
		return;
	}

	void updateStats() {
		damageBuffText.text = "Damage Buff: " + player.GetComponent<Abilities> ().dmgUp;
		slowDebuffText.text = "Slow Debuff: " + player.GetComponent<Abilities> ().slowAmount;
		cooldownText.text = "Cooldown: " + player.GetComponent<Abilities> ().slowCooldown;
		rangeText.text = "Range: " + player.GetComponent<SphereCollider> ().radius;
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

	public void upgradeButtonClicked() {
		upgradeButton.gameObject.SetActive (false);
		damageBuffButton.gameObject.SetActive (true);
		slowDebuffButton.gameObject.SetActive (true);
		cooldownButton.gameObject.SetActive (true);
		rangeButton.gameObject.SetActive (true);

		return;
	}

	public void damageBuffButtonClicked() {
		// Can player afford upgrade?
		if(!gc.canAfford(player.GetComponent<Abilities>().damageUpgradeValue)) {
			return;
		}
		
		gc.updateMoney (-player.GetComponent<Abilities> ().damageUpgradeValue);
		player.GetComponent<Abilities> ().upgrade (0);
		
		resetButtons();
		
		updateStats ();
		
		return;
	}

	public void slowDebuffButtonClicked() {
		// Can player afford upgrade?
		if(!gc.canAfford(player.GetComponent<Abilities>().slowUpgradeValue)) {
			return;
		}
		
		gc.updateMoney (-player.GetComponent<Abilities> ().slowUpgradeValue);
		player.GetComponent<Abilities> ().upgrade (1);
		
		resetButtons();
		
		updateStats ();
		
		return;
	}

	public void cooldownButtonClicked() {
		// Can player afford upgrade?
		if(!gc.canAfford(player.GetComponent<Abilities>().cooldownUpgradeValue)) {
			return;
		}
		
		gc.updateMoney (-player.GetComponent<Abilities> ().cooldownUpgradeValue);
		player.GetComponent<Abilities> ().upgrade (2);
		
		resetButtons();
		
		updateStats ();
		
		return;
	}

	public void rangeButtonClicked() {
		// Can player afford upgrade?
		if(!gc.canAfford(player.GetComponent<Abilities>().rangeUpgradeValue)) {
			return;
		}
		
		gc.updateMoney (-player.GetComponent<Abilities> ().rangeUpgradeValue);
		player.GetComponent<Abilities> ().upgrade (3);
		
		resetButtons();
		
		updateStats ();
		
		return;
	}

	private void resetButtons(){
		upgradeButton.gameObject.SetActive (true);
		damageBuffButton.gameObject.SetActive (false);
		slowDebuffButton.gameObject.SetActive (false);
		cooldownButton.gameObject.SetActive (false);
		rangeButton.gameObject.SetActive (false);
		tooltip.SetActive(false);
	}

	public void backButtonClicked() {
		objectCamera.SetActive (false);
		buildMenuPanel.SetActive (true);
		playerInfoPanel.SetActive (false);
		selectedTag = null;
		selectedObject = null;

		return;
	}

}
