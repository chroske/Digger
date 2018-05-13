using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HC.Debug;

public class DungeonController : SingletonMonoBehaviourFast<DungeonController> {

    public NetworkPlayerManager networkPlayerManager;
	public GameObject digCircle;
	public GameObject exprosionEffect;

	void OnCollisionEnter2D (Collision2D c){
		var tagIndex = c.gameObject.tag.IndexOf ("enemy_");
		if ((tagIndex >= 0 && networkPlayerManager.gameObject.CompareTag("my_player_character")) || (tagIndex < 0 && networkPlayerManager.gameObject.CompareTag("other_player_character"))) { //enemy
			string tagText = "";
			if (tagIndex >= 0) {
				tagText = c.gameObject.tag.Remove (tagIndex, 6);
			} else {
				tagText = c.gameObject.tag;
			}
			switch(tagText){
			case "bullet":
				var effect = Instantiate (exprosionEffect, c.transform.position, Quaternion.identity, this.transform.parent);
				Destroy (c.gameObject);
				Destroy (effect, 0.3f);
				break;
			case "drillBullet":
				break;
			}
		} else { //my bullet
			string tagText = "";
			if(tagIndex >= 0){
				tagText = c.gameObject.tag.Remove (tagIndex, 6);
			} else {
				tagText = c.gameObject.tag;
			}
			switch (tagText) {
			case "bullet":
				var effect = Instantiate (exprosionEffect, c.transform.position, Quaternion.identity, this.transform.parent);
				CreateDigCircle (c.transform.position, 0.5f);
				networkPlayerManager.CmdProvideDigToServer (c.transform.position, 0.5f);
				Destroy (c.gameObject);
				Destroy (effect, 0.3f);
				break;
			case "drillBullet":
				break;
			}
		}
	}
		

    public GameObject CreateDigCircle(Vector2 position, float sizeRatio = 1f){
        var digHole = Instantiate(digCircle, position, Quaternion.identity, this.transform);
        digHole.transform.localScale = new Vector3 (digHole.transform.localScale.x*sizeRatio, digHole.transform.localScale.y*sizeRatio, digHole.transform.localScale.z*sizeRatio);

        return digHole;
    }
}
