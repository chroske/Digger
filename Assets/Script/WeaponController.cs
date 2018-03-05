using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    public NetworkPlayerManager networkPlayerManager;

	public GameObject bullet;
	public GameObject muzzle;

	// Update is called once per frame
	void Update () {
		//武器をマウス方向に追従させる
        if(networkPlayerManager.isLocalPlayer){
			var pos = Camera.main.WorldToScreenPoint (transform.position);
			var rotation = Quaternion.LookRotation(Vector3.forward, Input.mousePosition - pos );
			this.transform.rotation = rotation;
        }
	}

    public void Shot(bool isShot){
		if(isShot){
			var baseBulletController = Instantiate (bullet, muzzle.transform.position, transform.rotation).GetComponent<BaseBulletController>();
			baseBulletController.weaponController = this;
            networkPlayerManager.CmdProvideWeaponShotToServer(this.transform.localEulerAngles);
		}
	}

    public void ShotVector(Vector3 shotWeaponVector){
        this.transform.localEulerAngles = shotWeaponVector;
        ShotEnemyPlayer();
    }

    void ShotEnemyPlayer(){
        var enemyBullet = Instantiate (bullet, muzzle.transform.position, transform.rotation);
		enemyBullet.GetComponent<BaseBulletController>().weaponController = this;;
        enemyBullet.tag = "enemy_bullet";
    }
}
