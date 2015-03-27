using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static float CAMERA_HEIGHT = 10;
	public GameObject[] enemies;
	public Vector3[] waypoints;
	public Queue waveInfo;
	public int monsterCount = 0;
	public float waveDelay;

	private bool isFirstWave = true;
	/*
	void Start () {
		monsterCount = 0;
		StartCoroutine(NextWave());
	}
	*/


	//TODO POTENTIAL BUG: Need to make sure we account for spawner enemies.
	public void updateMonsterCount(int val){
		monsterCount += val;
		if (monsterCount <= 0){
			StartCoroutine(NextWave());
		}
	}


	IEnumerator NextWave(){
		if (waveInfo.Count > 0){
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
