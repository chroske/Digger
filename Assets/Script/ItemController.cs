using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {
    public int itemId;


    void OnCollisionEnter2D (Collision2D c){
        if(c.gameObject.CompareTag ("my_player_character") || c.gameObject.CompareTag ("other_player_character")){
            Destroy(this.gameObject);

        }
    }
}
