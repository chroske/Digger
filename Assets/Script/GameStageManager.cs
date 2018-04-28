using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStageManager : SingletonMonoBehaviourFast<GameStageManager> {
	[SerializeField]
	GameObject itemPrefab;
	[SerializeField]
	BoxCollider2D ItemFieldInDungeon;

	GameObject dungeon;
	Dictionary<int, ItemController> generatedItemList = new Dictionary<int, ItemController>();

	int itemIdCounter;

	void Start () {
		Initialize ();
	}

	public void Initialize(){
        dungeon = GameObject.Find("Dungeons");
		itemIdCounter = 0;
		GenItem (new Vector2 (0, -5), new Vector2 (1, 1));
		GenItem (new Vector2 (4, 10), new Vector2 (2, 2));
	}

	void GenItem(Vector2 popItemPosition, Vector2 popItemSizeScale){
		//var popItemField = Instantiate (ItemFieldInDungeon.gameObject, popItemPosition, Quaternion.identity);
		//popItemField.transform.SetParent (dungeon.transform);
		//popItemField.transform.localScale = popItemSizeScale;

		var popItem = Instantiate (itemPrefab, popItemPosition, Quaternion.identity);
		popItem.transform.localScale = popItemSizeScale;
		itemIdCounter++;
		var itemController = popItem.GetComponent<ItemController> ();
		itemController.itemId = itemIdCounter;
		generatedItemList.Add (itemIdCounter, itemController);

		//popItemField.GetComponent<BoxCollider2D> ().size = new Vector2 (popItem.GetComponent<BoxCollider2D> ().size.x + 0.5f, popItem.GetComponent<BoxCollider2D> ().size.y + 0.5f);
		popItem.GetComponent<Rigidbody2D> ().simulated = true;
	}
}
