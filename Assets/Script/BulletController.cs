using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : BaseBulletController {

    //public WeaponController weaponController;

	//public int speed = 10;

	//void Start(){
	//	GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
	//}

    void OnCollisionEnter2D (Collision2D c){
        if(transform.gameObject.CompareTag ("bullet") && c.gameObject.CompareTag ("other_player_character")){
            var enemyNetId = c.gameObject.GetComponent<NetworkPlayerManager>().netId;
            weaponController.networkPlayerManager.CmdProvideHitDamageObjectOtherPlayerToServer(enemyNetId, damage);
        } else if(c.gameObject.CompareTag ("enemy_bullet")){
        }
    }
}
