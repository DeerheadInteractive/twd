using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject monster;
	public int numMonsters;
	public float monsterDelay;

	public Vector3[] waypoints;

	// Use this for initialization
	void Start () {
		StartCoroutine(SpawnMonster());
	}
	
	IEnumerator SpawnMonster(){
		for (int i = 0; i < numMonsters; ++i){
			// Monsters start at first waypoint.
			GameObject mon = Instantiate(monster, waypoints[0], Quaternion.identity) as GameObject;
			// Give waypoints to mover.
			mon.GetComponent<WaypointMover>().InitializeWaypoints(waypoints);
			print ("Monsters spawned = " + (i + 1));
			yield return new WaitForSeconds(monsterDelay);
		}
	}
}
