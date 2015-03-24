using UnityEngine;
using System.Collections;

/// <summary>
/// Moves a "living" object through a set of waypoints.
/// </summary>
public class WaypointMover : MonoBehaviour {
	public Queue waypoints;

	public Vector3 destination;
	public bool hasDestination = false;
	private float epsilon = 0.3f;

	void Start () {
		if ((waypoints == null || waypoints.Count < 1)&&!hasDestination){
			print ("Error: Too few waypoints on start. (WaypointMover");
		} else{
			if (!hasDestination)
				destination = (Vector3)(waypoints.Dequeue());
			hasDestination = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// If a waypoint was reached, aim for next waypoint or delete self.
		if (Vector3.Distance(transform.position, destination) <= epsilon){
			if (waypoints == null)
				return;
			if (waypoints.Count < 1){
				// Delete object, since it has reached its destination. May want to notify Game Controller.
				Destroy(transform.gameObject);
				return;
			} else{
				destination = (Vector3)(waypoints.Dequeue());
			}
		}

		Vector3 targetDirection = destination - transform.position;
		transform.rotation = Quaternion.LookRotation(targetDirection);
		rigidbody.velocity = transform.forward * transform.gameObject.GetComponent<Vida>().speed;

	}

	public void InitializeWaypoints(Vector3[] points){
		waypoints = new Queue();
		for (int i = 0; i < points.Length; ++i){
			waypoints.Enqueue(points[i]);
		}
	}
}
