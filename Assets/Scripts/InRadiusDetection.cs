using UnityEngine;
using System.Collections;

public class InRadiusDetection : MonoBehaviour {

	void OnTriggerStay(Collider other){
		GameObject obj = other.transform.gameObject;
		if (obj == null)
			print ("obj is null");
		GameObject self = GetComponent<Collider>().gameObject;
		if (self == null)
			print ("self is null");
		self.GetComponent<Gunnery>().RadiusDetected(obj);
	}
}
