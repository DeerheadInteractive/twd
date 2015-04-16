using UnityEngine;
using System.Collections;

/// <summary>
/// Rotates an object and then destroys it after some time. (For VFX).
/// </summary>
public class RotateMover : MonoBehaviour {
	public float duration;
	public float targetRadius;
	public float baseAlpha;
	private float timeAlive;

	void Start(){
		// Set object's alpha to be baseAlpha (aka the starting alpha)
		GameObject obj = transform.gameObject;
		Renderer r = obj.GetComponent<Renderer>();
		Color color = r.material.color;
		color.a = baseAlpha * (1.0f - Mathf.Sqrt(timeAlive / duration));
		r.material.color = color;

		transform.localScale = new Vector3(targetRadius, targetRadius, targetRadius);
		rigidbody.angularVelocity = new Vector3(1.0f, 0f, 0f); 
	}

	void Update(){
		timeAlive += Time.deltaTime;
		if (timeAlive >= duration){
			Destroy(transform.gameObject);
		} else{
			GameObject obj = transform.gameObject;
			Renderer r = obj.GetComponent<Renderer>();
			Color color = r.material.color;
			color.a = baseAlpha * (1.0f - Mathf.Sqrt(timeAlive / duration));
			r.material.color = color;
		}
	}
}
