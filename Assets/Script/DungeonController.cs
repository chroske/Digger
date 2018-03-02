using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HC.Debug;

public class DungeonController : SingletonMonoBehaviourFast<DungeonController> {

    public NetworkPlayerManager networkPlayerManager;
	public GameObject digCircle;
	public GameObject exprosionEffect;
	public CompositeCollider2D compositeCollider2D;

	public ColliderVisualizer colliderVisualizer;
	void Awake(){
		var color = ColliderVisualizer.VisualizerColorType.Red;
		colliderVisualizer.Initialize( color, "あいうえお", 36 );
	}

	void OnCollisionEnter2D (Collision2D c){
		var tagIndex = c.gameObject.tag.IndexOf ("enemy_");
		if (tagIndex >= 0) {
			var tagText = c.gameObject.tag.Remove (tagIndex, 6);
			switch(tagText){
			case "bullet":
				var effect = Instantiate (exprosionEffect, c.transform.position, Quaternion.identity, this.transform.parent);
				Destroy (c.gameObject);
				Destroy (effect, 0.3f);
				break;
			case "drillBullet":
				break;
			}
		} else {
			switch (c.gameObject.tag) {
			case "bullet":
				var effect = Instantiate (exprosionEffect, c.transform.position, Quaternion.identity, this.transform.parent);
				CreateDigCircle (c.transform.position, 0.5f);
				networkPlayerManager.CmdProvideDigToServer (c.transform.position, 0.5f);
				//Destroy (c.gameObject);
				Destroy (effect, 0.3f);
				break;
			case "drillBullet":
				break;
			}
		}




//		if (c.gameObject.CompareTag ("bullet")) {
//
//        } else if(c.gameObject.CompareTag ("enemy_bullet")){
//           
//        }

		//compositeCollider2D.edgeRadius;
	}
		

    public GameObject CreateDigCircle(Vector2 position, float sizeRatio = 1f){
        var digHole = Instantiate(digCircle, position, Quaternion.identity, this.transform);
        digHole.transform.localScale = new Vector3 (digHole.transform.localScale.x*sizeRatio, digHole.transform.localScale.y*sizeRatio, digHole.transform.localScale.z*sizeRatio);

        return digHole;
    }



//	public float lifespan; private float timer;
//	//Create this later: 
//	public AnotherScript master; 
//	void Update () { 
//		//Set the lifespan of this cube to the masterscript length value 
//		lifespan = master.length; 
//		//Keep the timer ticking! 
//		timer += Time.deltaTime; 
//		//If the timer (which keeps track of how many seconds we've been alive) is bigger than our allotted lifespan, destroy ourselves 
//		if(timer>lifespan){ GameObject.Destroy (gameObject); } 
//	}
}
