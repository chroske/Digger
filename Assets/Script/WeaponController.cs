using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
	public GameObject bullet;
	public GameObject muzzle;

	// Update is called once per frame
	void Update () {
		//武器をマウス方向に追従させる
		Vector3 diff = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position).normalized;
		this.transform.rotation = Quaternion.FromToRotation (Vector3.up, new Vector3(diff.x, diff.y, 0));

		Shot (Input.GetButtonDown("Fire2"));
	}

	void Shot(bool isShot){
		if(isShot){
			Instantiate (bullet, muzzle.transform.position, transform.rotation);
		}
	}
}
