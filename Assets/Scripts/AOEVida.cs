using UnityEngine;
using System.Collections;

public class AOEVida : Vida {
	private bool exploded;

	public override void Explode(){
		if (explosion != null && !exploded){
			exploded = true;
			GameObject ex = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
			ex.GetComponent<RotateMover>().targetRadius = 5.0f;
			
		}
	}

}
