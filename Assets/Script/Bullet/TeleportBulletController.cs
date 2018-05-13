using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBulletController : BaseBulletController {

	void OnCollisionEnter2D (Collision2D c){
		if (c.gameObject.CompareTag ("item") || c.gameObject.CompareTag ("my_home_area") || c.gameObject.CompareTag ("other_home_area")) {
			Destroy (this.gameObject);
		} else {
			weaponController.networkPlayerManager.gameObject.transform.position = new Vector3 (c.contacts[0].point.x, c.contacts[0].point.y, weaponController.networkPlayerManager.gameObject.transform.position.z);
			var effect = Instantiate (hitEffect, c.contacts[0].point, Quaternion.identity, this.transform.parent);
			Destroy (effect, effectLifeTime);
			Destroy (this.gameObject);
		}
	}
}
