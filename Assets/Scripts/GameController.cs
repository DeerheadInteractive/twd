using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	public static float CAMERA_HEIGHT = 10;
	public Animator animator;
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
	public GameObject UIControllerObject;

	public int health;
	public int money;
	public int wavenum;

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
			if (curWave != null && curWave.Count < 1 && waveInfo.Count < 1){
				gameOver = true;
				GameOver(true);
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

	public bool canAfford(int val){
		return (money - val) >= 0;
	}

	public void updateWave(){
		wavenum++;
		Text txt = waveText.GetComponent<Text>();
		txt.text = "Wave: " + wavenum;
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
				yield return new WaitForSeconds(startWaveDelay);
			}
			curWave = (Queue)(waveInfo.Dequeue());
			updateWave();
			foreach (Vector3 batchInfo in curWave){
				for (int i = 0; i < batchInfo.y; ++i){
					if (gameOver)
						yield break;
					GameObject enemy = Instantiate(enemies[(int)batchInfo.x], waypoints[0], Quaternion.identity) as GameObject;
					enemy.GetComponent<WaypointMover>().InitializeWaypoints(waypoints);
					updateMonsterCount(1);
					yield return new WaitForSeconds(batchInfo.z);
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
