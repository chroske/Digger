﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
	[SerializeField]
	private List<GameObject> bullets;

	private int currentBulletsIndex = 0;
	private bool isShotBullet;
	private float rateOfFire;

    public NetworkPlayerManager networkPlayerManager;
	public GameObject bullet;
	public GameObject muzzle;


	void Start(){
		bullet = bullets [0];
		rateOfFire = bullet.GetComponent<BaseBulletController> ().rateOfFire;
		isShotBullet = true;
	}

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
			if(isShotBullet){
				isShotBullet = false;

				var baseBulletController = Instantiate (bullet, muzzle.transform.position, transform.rotation).GetComponent<BaseBulletController>();
				baseBulletController.weaponController = this;
				if(networkPlayerManager.gameObject.CompareTag("other_player_character")){
					baseBulletController.gameObject.tag = "enemy_"+baseBulletController.gameObject.tag;
					baseBulletController.gameObject.layer = 16;
				}
				networkPlayerManager.CmdProvideWeaponShotToServer(this.transform.localEulerAngles);

				StartCoroutine ("ShopBulletLoop");
			}
		}
	}

	IEnumerator ShopBulletLoop ()
	{
		yield return new WaitForSeconds (rateOfFire);
		isShotBullet = true;
	}

    public void ShotVector(Vector3 shotWeaponVector){
        this.transform.localEulerAngles = shotWeaponVector;
        ShotEnemyPlayer();
    }

	public void ChangeBullet(float axis){
		if (axis > 0.0f) {
			currentBulletsIndex++;
			if(currentBulletsIndex > bullets.Count-1){
				currentBulletsIndex = 0;
			}
			bullet = bullets [currentBulletsIndex];
			networkPlayerManager.unityChan2DController.weaponIcon.sprite = bullet.GetComponent<BaseBulletController> ().weaponIconImage;
			networkPlayerManager.CmdProvideChangeWaeponBulletToServer (currentBulletsIndex);
		} else if (axis < 0.0f) {
			currentBulletsIndex--;
			if(currentBulletsIndex < 0){
				currentBulletsIndex = bullets.Count-1;
			}
			bullet = bullets [currentBulletsIndex];
			networkPlayerManager.unityChan2DController.weaponIcon.sprite = bullet.GetComponent<BaseBulletController> ().weaponIconImage;
			networkPlayerManager.CmdProvideChangeWaeponBulletToServer (currentBulletsIndex);
		} else {
			// do nothing
		}
		rateOfFire = bullet.GetComponent<BaseBulletController> ().rateOfFire;
	}

	public void ChangeWaeponBulletByBulletIndex(int bulletIndex){
		bullet = bullets [bulletIndex];
		rateOfFire = bullet.GetComponent<BaseBulletController> ().rateOfFire;
	}

    void ShotEnemyPlayer(){
        var enemyBullet = Instantiate (bullet, muzzle.transform.position, transform.rotation);
		enemyBullet.GetComponent<BaseBulletController>().weaponController = this;
		if(networkPlayerManager.gameObject.CompareTag("other_player_character")){
			enemyBullet.tag = "enemy_"+enemyBullet.tag;
			enemyBullet.layer = 16;
		}
    }
}
