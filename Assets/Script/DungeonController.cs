using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour {

	public GameObject digCircle;
	public GameObject exprosionEffect;

	void OnCollisionEnter2D (Collision2D c){
		if (c.gameObject.CompareTag ("bullet")) {
			var effect = Instantiate(exprosionEffect, c.transform.position, Quaternion.identity, this.transform.parent);
			var digHole = Instantiate(digCircle, c.transform.position, Quaternion.identity, this.transform);
			digHole.transform.localScale = new Vector3 (digHole.transform.localScale.x/2, digHole.transform.localScale.y/2, digHole.transform.localScale.z/2);
			Destroy (c.gameObject);
			Destroy (effect, 0.3f);
		}
	}
}
