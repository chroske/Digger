using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameStageManager : NetworkBehaviour {
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
		GenItem (1, 1, new Vector2 (0, -5), new Vector2 (1, 1));
		GenItem (1, 2, new Vector2 (4, 10), new Vector2 (2, 2));
	}

	void GenItem(int itemId, int itemCount, Vector2 popItemPosition, Vector2 popItemSizeScale){
		var popItem = Instantiate (itemPrefab, popItemPosition, Quaternion.identity);
		popItem.transform.localScale = popItemSizeScale;
		itemIdCounter++;
		var itemController = popItem.GetComponent<ItemController> ();
		itemController.itemPopId = itemIdCounter;
		itemController.itemId = itemId;
		itemController.itemCount = itemCount;
		itemController.gameStageManager = this;
		generatedItemList.Add (itemIdCounter, itemController);

		popItem.GetComponent<Rigidbody2D> ().simulated = true;
	}

	public void DeleteGetItem(int ItemPopId){
		if (generatedItemList.ContainsKey (ItemPopId)) {
			Destroy (generatedItemList [ItemPopId].gameObject);
			generatedItemList.Remove (ItemPopId);
		} else {
			Debug.LogError ("他のプレイヤーが取得済み");
		}
	}

	[ClientRpc]
	public void RpcPlayerGetItem(int ItemPopId){
		if (generatedItemList.ContainsKey (ItemPopId)) {
			Destroy (generatedItemList [ItemPopId].gameObject);
			generatedItemList.Remove (ItemPopId);
		} else {
			Debug.LogError ("他のプレイヤーが取得済み");
		}
	}

	[TargetRpc]
	public void TargetGiveItem(NetworkConnection target, int itemId, int itemCount){
		//GameStatusManager.Instance.myNetworkManager.myNetworkPlayerManager.holdItems.Add(new holdItem());
		Debug.Log ("give item");
	}
}
