using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    [SerializeField]
    private NetworkPlayerManager networkPlayerManager;

	public GameObject bullet;
	public GameObject muzzle;

	// Update is called once per frame
	void Update () {
		//武器をマウス方向に追従させる
        if(networkPlayerManager.isLocalPlayer){
            Vector3 diff = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position).normalized;
            this.transform.rotation = Quaternion.FromToRotation (Vector3.up, new Vector3(diff.x, diff.y, 0));
        }
	}

    public void Shot(bool isShot){
		if(isShot){
			Instantiate (bullet, muzzle.transform.position, transform.rotation);
            networkPlayerManager.CmdProvideWeaponShotToServer(this.transform.localEulerAngles);
		}
	}

    public void ShotVector(Vector3 shotWeaponVector){
        this.transform.localEulerAngles = shotWeaponVector;
        ShotEnemyPlayer();
    }

    void ShotEnemyPlayer(){
        var enemyBullet = Instantiate (bullet, muzzle.transform.position, transform.rotation);
        enemyBullet.tag = "enemy_bullet";
    }
}
