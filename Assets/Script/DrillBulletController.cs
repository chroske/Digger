using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBulletController : BaseBulletController {

	//public WeaponController weaponController;

	//public int speed = 10;
	[SerializeField]
	TrailRendererWith2DCollider trailRendererWith2DCollider;

	public float destroyTime = 2f;

	//void Start(){
	//	GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
	//}

	void OnTriggerEnter2D (Collider2D c){
		

		if(transform.gameObject.CompareTag ("bullet") && c.gameObject.CompareTag ("other_player_character")){
			var enemyNetId = c.gameObject.GetComponent<NetworkPlayerManager>().netId;
			weaponController.networkPlayerManager.CmdProvideHitDamageObjectOtherPlayerToServer(enemyNetId);
		} else if(c.gameObject.CompareTag ("enemy_bullet")){
		} else if(c.gameObject.CompareTag ("dungeon")){
			trailRendererWith2DCollider.pausing = false;
			ChangeBulletSpeed (5f);
		}

		Destroy (this.gameObject, destroyTime);
	}
}
