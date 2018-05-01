using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : BaseBulletController {

    void OnCollisionEnter2D (Collision2D c){
		if ((transform.gameObject.CompareTag ("bullet") && c.gameObject.CompareTag ("other_player_character")) || (transform.gameObject.CompareTag ("enemy_bullet") && c.gameObject.CompareTag ("my_player_character"))) {
			var enemyNetId = c.gameObject.GetComponent<NetworkPlayerManager> ().netId;
			weaponController.networkPlayerManager.CmdProvideHitDamageObjectOtherPlayerToServer (enemyNetId, damage);
		} else if (c.gameObject.CompareTag ("item") || c.gameObject.CompareTag ("my_home_area") || c.gameObject.CompareTag ("other_home_area")) {
			Destroy (this.gameObject);
		}
    }
}
