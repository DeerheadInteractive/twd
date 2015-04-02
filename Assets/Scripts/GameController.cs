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

	public int startingHealth;
	public int startingMoney;
	public GameObject healthText;
	public GameObject moneyText;
	public GameObject waveText;

	public int health;
	public int money;
	public int wavenum;


	private bool isFirstWave = true;
	
	void Start () {
		monsterCount = 0;
		health = startingHealth;
		money = startingMoney;
		updateHealth(0);
		updateMoney(0);
		//StartCoroutine(NextWave());
	}


	//TODO POTENTIAL BUG: Need to make sure we account for spawner enemies?
	public void updateMonsterCount(int val){
		monsterCount += val;
		if (monsterCount <= 0){
			StartCoroutine(NextWave());
		}
	}

	public void updateHealth(int val){
		health += val;
		Text txt = healthText.GetComponent<Text>();
		txt.text = "Health: " + health;
	}

	public void updateMoney(int val){
		money += val;
		Text txt = moneyText.GetComponent<Text>();
		txt.text = "Money: " + money;
	}

	public void updateWave(){
		wavenum++;
		Text txt = waveText.GetComponent<Text>();
		txt.text = "Wave: " + wavenum;
	}

	IEnumerator NextWave(){
		if (waveInfo.Count > 0){
			updateWave();
			if (!isFirstWave){
				print ("yielding");
				yield return new WaitForSeconds(waveDelay);
				print ("Yielded");
			} else{
				isFirstWave = false;
			}
			Queue wave = (Queue)(waveInfo.Dequeue());
			foreach (Vector3 batchInfo in wave){
				for (int i = 0; i < batchInfo.y; ++i){
					GameObject enemy = Instantiate(enemies[(int)batchInfo.x], waypoints[0], Quaternion.identity) as GameObject;
					enemy.GetComponent<WaypointMover>().InitializeWaypoints(waypoints);
					updateMonsterCount(1);
					yield return new WaitForSeconds(batchInfo.z);
				}
			}
			yield break;
		} else{
			GameOver(true);
			yield break;
		}
	}

	public void GameOver(bool win){
		if (win){
			print ("End of level.");
		} else{
			print ("You lost? Pfft.");
		}
	}
}
