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

	public int startingHealth;
	public int startingMoney;
	public GameObject healthText;
	public GameObject moneyText;
	public GameObject waveText;

	public int health;
	public int money;
	public int wavenum;

	private bool gameOver = false;


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
		if (gameOver)
			return;
		health += val;
		if (health < 0)
			health = 0;

		Text txt = healthText.GetComponent<Text>();
		txt.text = "Health: " + health;

		if (health == 0){
			GameOver(false);
		}
	}

	public void updateMoney(int val){
		money += val;
		Text txt = moneyText.GetComponent<Text>();
		txt.text = "Money: " + money;
	}

	public bool canAfford(int val){
		return (money - val) >= 0;
	}

	public void updateWave(){
		wavenum++;
		Text txt = waveText.GetComponent<Text>();
		txt.text = "Wave: " + wavenum;
	}


	IEnumerator NextWave(){
		if (gameOver)
			yield break;
		if (waveInfo.Count > 0){
			updateWave();
			if (!isFirstWave){
				if (gameOver)
					yield break;
				yield return new WaitForSeconds(waveDelay);
			} else{
				isFirstWave = false;
			}
			Queue wave = (Queue)(waveInfo.Dequeue());
			foreach (Vector3 batchInfo in wave){
				for (int i = 0; i < batchInfo.y; ++i){
					if (gameOver)
						yield break;
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
		gameOver = true;
		if (win){
			print ("End of level. You won!");
			animator.SetTrigger("GameOver");
		} else{
			animator.SetTrigger("GameOver");
			print ("End of level. You lost? Pfft.");
		}
	}

}
