using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemController : MonoBehaviour {
    public int itemId;
	[SerializeField]
	BoxCollider2D boxCol2d;
	[SerializeField]
	Rigidbody2D rigidbody2D;

	void Start(){
		rigidbody2D.gravityScale = 0;
	}

    void OnCollisionEnter2D (Collision2D c){
        if(c.gameObject.CompareTag ("my_player_character") || c.gameObject.CompareTag ("other_player_character")){
            Destroy(this.gameObject);
		}
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
