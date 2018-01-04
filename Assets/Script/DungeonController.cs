using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour {

    public NetworkPlayerManager networkPlayerManager;
	public GameObject digCircle;
	public GameObject exprosionEffect;

	void OnCollisionEnter2D (Collision2D c){
		if (c.gameObject.CompareTag ("bullet")) {
			var effect = Instantiate(exprosionEffect, c.transform.position, Quaternion.identity, this.transform.parent);
            CreateDigCircle(c.transform.position, 0.5f);
            networkPlayerManager.CmdProvideDigToServer(c.transform.position, 0.5f);

			Destroy (c.gameObject);
			Destroy (effect, 0.3f);
        } else if(c.gameObject.CompareTag ("enemy_bullet")){
            var effect = Instantiate(exprosionEffect, c.transform.position, Quaternion.identity, this.transform.parent);
            Destroy (c.gameObject);
            Destroy (effect, 0.3f);
        }
	}

    public GameObject CreateDigCircle(Vector2 position, float sizeRatio = 1f){
        var digHole = Instantiate(digCircle, position, Quaternion.identity, this.transform);
        digHole.transform.localScale = new Vector3 (digHole.transform.localScale.x*sizeRatio, digHole.transform.localScale.y*sizeRatio, digHole.transform.localScale.z*sizeRatio);

        return digHole;
    }
}
