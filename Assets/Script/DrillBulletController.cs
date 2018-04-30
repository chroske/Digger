using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBulletController : BaseBulletController {
	[SerializeField]
	TrailRendererWith2DCollider trailRendererWith2DCollider;

	public float destroyTime = 2f;

	void OnTriggerEnter2D (Collider2D c){
		if((transform.gameObject.CompareTag ("drillBullet") && c.gameObject.CompareTag ("other_player_character")) || (transform.gameObject.CompareTag ("enemy_drillBullet") && c.gameObject.CompareTag ("my_player_character"))){
			var enemyNetId = c.gameObject.GetComponent<NetworkPlayerManager>().netId;
			weaponController.networkPlayerManager.CmdProvideHitDamageObjectOtherPlayerToServer(enemyNetId, damage);
		} else if(c.gameObject.CompareTag ("dungeon")){
			trailRendererWith2DCollider.pausing = false;
			ChangeBulletSpeed (5f);
		}

		Destroy (this.gameObject, destroyTime);
	}
}
