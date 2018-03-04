using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBulletController : MonoBehaviour {
	public WeaponController weaponController;
	public float speed = 10f;

	void Start(){
		GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
	}

	public void ChangeBulletSpeed(float newSpeed){
		GetComponent<Rigidbody2D>().velocity = transform.up.normalized * newSpeed;
	}
}
