using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameStageManager : /*SingletonMonoBehaviourFast<GameStageManager>*/NetworkBehaviour {
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
		var popItem = Instantiate (itemPrefab, popItemPosition, Quaternion.identity);
		popItem.transform.localScale = popItemSizeScale;
		itemIdCounter++;
		var itemController = popItem.GetComponent<ItemController> ();
		itemController.itemId = itemIdCounter;
		itemController.gameStageManager = this;
		generatedItemList.Add (itemIdCounter, itemController);

		popItem.GetComponent<Rigidbody2D> ().simulated = true;
	}

	public void GetItem(int generatedItemListId){
		CmdPlayerGetItem (generatedItemListId);
	}

	[Command]
	public void CmdPlayerGetItem(int generatedItemListId){
		Destroy (generatedItemList [generatedItemListId].gameObject);
		RpcPlayerGetItem (generatedItemListId);
	}

	[ClientRpc]
	public void RpcPlayerGetItem(int generatedItemListId){
		if (!isLocalPlayer) {
			Destroy (generatedItemList [generatedItemListId].gameObject);
		}
	}
}
