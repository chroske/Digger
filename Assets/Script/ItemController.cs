using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemController : MonoBehaviour {
    public int itemPopId;
	public GameStageManager gameStageManager;

	[SerializeField]
	BoxCollider2D boxCol2d;
	[SerializeField]
	Rigidbody2D rigidbody2D;

	void Start(){
		rigidbody2D.gravityScale = 0;
	}

	void OnTriggerExit2D(Collider2D c){
		if(c.gameObject.CompareTag ("dungeon")){
			if(boxCol2d.isTrigger == true){
				boxCol2d.isTrigger = false;
				rigidbody2D.gravityScale = 2;
			}
		}
	}
}
