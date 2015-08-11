using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	public static float CAMERA_HEIGHT = 10;
	public GameObject[] enemies;
	public Vector3[] waypoints;
	public Queue waveInfo;
	public int monsterCount = 0;
	public float waveDelay;
	public float startWaveDelay;

	public int startingHealth;
	public int startingMoney;
	public GameObject healthText;
	public GameObject moneyText;
	public GameObject waveText;
	public GameObject towerLimitText;
	public GameObject UIControllerObject;

	public int health;
	public int money;
	public int wavenum;

	public int towerLimit = 10;
	public int maxTowerLimit = 10;

	private bool gameOver = false;

	private bool isFirstWave = true;
	private Queue curWave;
	
	void Start () {
		monsterCount = 0;
		health = startingHealth;
		money = startingMoney;
		updateMonsterCount(0);
		updateHealth(0);
		updateMoney(0);
	}

	void Update(){
		if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && !gameOver){
			UIControllerObject.GetComponent<UIController>().Pause();
		}
		
		Time.timeScale = UIControllerObject.GetComponent<UIController>().isPaused() ? 0.0f : 1.0f;
	}


	//TODO POTENTIAL BUG: Need to make sure we account for spawner enemies?
	public void updateMonsterCount(int val){
		monsterCount += val;
		if (monsterCount < 1){
			if (curWave != null && curWave.Count < 1){
			    if (waveInfo.Count < 1){
					gameOver = true;
					GameOver(true);
				} else{
					StartCoroutine(NextWave());
				}
			}
		}
	}

	public void updateHealth(int val){
		if (gameOver)
			return;
		health += val;
		if (health < 0)
			health = 0;

		Text txt = healthText.GetComponent<Text>();
		txt.text = "Health: " + health;

		if (health == 0){
			gameOver = true;
			GameOver(false);
		}
	}

	public void updateMoney(int val){
		money += val;
		Text txt = moneyText.GetComponent<Text>();
		txt.text = "Nanites: " + money;
	}

	public void updateTowerLimit()
	{
		Text txt = towerLimitText.GetComponent<Text> ();
		txt.text = "Towers available: " + towerLimit + "/" + maxTowerLimit;
	}

	public bool canAfford(int val){
		return (money - val) >= 0;
	}

	public void updateWave(){
		wavenum++;
		if (waveText != null){
			Text txt = waveText.GetComponent<Text>();
			txt.text = "Wave: " + wavenum;
		}
	}

	public void launch(){
		StartCoroutine(NextWave());
	}

	IEnumerator NextWave(){
		if (gameOver)
			yield break;
		if (waveInfo.Count > 0){
			if (isFirstWave){
				// TODO: Display countdown on UI
				isFirstWave = false;
				//yield return new WaitForSeconds(startWaveDelay);
				yield return null;
			}
			curWave = (Queue)(waveInfo.Dequeue());
			updateWave();
			while (curWave.Count > 0){
			//(Vector3 batchInfo in curWave){
				Vector3 batchInfo = (Vector3)curWave.Dequeue();
				for (int i = 0; i < batchInfo.y; ++i){
					if (gameOver)
						yield break;
					GameObject enemy = Instantiate(enemies[(int)batchInfo.x], waypoints[0], Quaternion.identity) as GameObject;
					enemy.GetComponent<WaypointMover>().InitializeWaypoints(waypoints);
					updateMonsterCount(1);
					//yield return new WaitForSeconds(batchInfo.z);
					yield return null;
				}
			}
			yield return new WaitForSeconds(waveDelay);
		} else{
			//GameOver(true);
			yield break;
		}
	}

	public void GameOver(bool win){
		gameOver = true;
		UIControllerObject.GetComponent<UIController>().GameOver(win);
	}
}
