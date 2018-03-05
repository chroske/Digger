using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBulletController : MonoBehaviour {
	public WeaponController weaponController;
	public float speed = 10f;
	public float damage = 1.0f;
	public Sprite weaponIconImage;

	void Start(){
		GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
	}

	public void ChangeBulletSpeed(float newSpeed){
		GetComponent<Rigidbody2D>().velocity = transform.up.normalized * newSpeed;
	}
}
