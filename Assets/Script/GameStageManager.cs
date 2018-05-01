using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameStageManager : NetworkBehaviour {
	[SyncVar(hook = "SyncTeam1GemCountValue")]
	public int syncTeam1GemCount;
	[SyncVar(hook = "SyncTeam2GemCountValue")]
	public int syncTeam2GemCount;

	[SerializeField]
	GameObject itemPrefab;
	[SerializeField]
	BoxCollider2D ItemFieldInDungeon;

	[Serializable]
	public class ItemData{
		public int itemId;
		public int itemCount;
		public Vector2 itemScale;
		public Vector2 itemPopPosition;
	}
	int popItemCount = 155;
	Vector2 fieldSize = new Vector2(300, 300);

	GameObject dungeon;
	Dictionary<int, ItemController> generatedItemList = new Dictionary<int, ItemController>();
	int itemIdCounter;

	void Start () {
		Initialize ();
	}

	public void Initialize(){
        dungeon = GameObject.Find("Dungeons");
		itemIdCounter = 0;
		//GenItem (1, 1, new Vector2 (86, 35), new Vector2 (1, 1));
		//GenItem (1, 2, new Vector2 (80, 40), new Vector2 (2, 2));
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

	[Command]
	public void CmdRandomPopItems(){
		List<ItemData> itemsPopPosition = new List<ItemData> ();
		for(int i=0; i<popItemCount; i++){
			ItemData item = new ItemData ();
			item.itemId = 1;
			item.itemCount = 1;
			item.itemScale = new Vector2 (1, 1);
			item.itemPopPosition = new Vector2 (UnityEngine.Random.Range(0, fieldSize.x), UnityEngine.Random.Range(0, fieldSize.y));
			itemsPopPosition.Add (item);
		}

		var itemsPopPositionJson = JsonUtility.ToJson (new Serialization<ItemData>(itemsPopPosition));
		RpcRandomPopItems (itemsPopPositionJson);

		foreach(var itemPopPosition in itemsPopPosition){
			GenItem(itemPopPosition.itemId, itemPopPosition.itemCount, itemPopPosition.itemPopPosition, itemPopPosition.itemScale);
		}
	}

	[ClientRpc]
	public void RpcRandomPopItems(string itemsPopPositionJson){
		List<ItemData> itemsPopPosition = JsonUtility.FromJson<Serialization<ItemData>> (itemsPopPositionJson).ToList();

		foreach(var itemPopPosition in itemsPopPosition){
			GenItem(itemPopPosition.itemId, itemPopPosition.itemCount, itemPopPosition.itemPopPosition, itemPopPosition.itemScale);
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

	void SyncTeam1GemCountValue(int team1GemCount){
		UIManager.Instance.SetValueTeamGemCounter(1, team1GemCount);
	}

	void SyncTeam2GemCountValue(int team2GemCount){
		UIManager.Instance.SetValueTeamGemCounter(2, team2GemCount);
	}
}
