using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
	[SerializeField]
	private List<GameObject> bullets;

	private int currentBulletsIndex = 0;

    public NetworkPlayerManager networkPlayerManager;
	public GameObject bullet;
	public GameObject muzzle;

	void Start(){
		bullet = bullets [0];
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
			var baseBulletController = Instantiate (bullet, muzzle.transform.position, transform.rotation).GetComponent<BaseBulletController>();
			baseBulletController.weaponController = this;
            networkPlayerManager.CmdProvideWeaponShotToServer(this.transform.localEulerAngles);
		}
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
		} else if (axis < 0.0f) {
			currentBulletsIndex--;
			if(currentBulletsIndex < 0){
				currentBulletsIndex = bullets.Count-1;
			}
			bullet = bullets [currentBulletsIndex];
		} else {
			// do nothing
		}
	}

    void ShotEnemyPlayer(){
        var enemyBullet = Instantiate (bullet, muzzle.transform.position, transform.rotation);
		enemyBullet.GetComponent<BaseBulletController>().weaponController = this;;
		enemyBullet.tag = "enemy_"+enemyBullet.tag;
    }
}
