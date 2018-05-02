using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameStageManager : NetworkBehaviour {
	[SyncVar(hook = "SyncTeam1GemCountValue")]
	public int syncTeam1GemCount;
	[SyncVar(hook = "SyncTeam2GemCountValue")]
	public int syncTeam2GemCount;

	[SerializeField]
	GameObject itemPrefab;

	[Serializable]
	public class ItemData{
		public int itemId;
		public int itemCount;
		public Vector2 itemScale;
		public Vector2 itemPopPosition;
	}

	public bool isStartGame;

	GameObject dungeon;
	GameObject myHomeFrame;
	GameObject otherHomeFrame;
	Dictionary<int, ItemController> generatedItemList = new Dictionary<int, ItemController>();
	int itemIdCounter;
	float countTime = 0;

	/* ゲーム調整パラメータ群 */

	int popItemCount = 155;
	Vector2 fieldSize = new Vector2(300, 300);
	float gameTime = 180f;

	/* ゲーム調整パラメータ群 */


	void Start () {
		Initialize ();
	}

	void Update () {
		if(isStartGame){
			countTime += Time.deltaTime; //スタートしてからの秒数を格納
			float limitTime = ((gameTime-countTime) < 0) ? 0 : (gameTime-countTime); 
			UIManager.Instance.timerText.text = limitTime.ToString("F2"); //小数2桁にして表示
			if((gameTime-countTime) <= 0){
				//game end
				isStartGame = false;
				EndGame ();
			}
		}
	}

	public void Initialize(){
        dungeon = GameObject.Find("Dungeons");
		myHomeFrame = GameObject.Find("MyHome/MyHomeFrame");
		otherHomeFrame = GameObject.Find("OtherHome/OtherHomeFrame");
		itemIdCounter = 0;
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

	[Server]
	public void EndGame(){
		int winnerTeamId;
		if (syncTeam1GemCount > syncTeam2GemCount) {
			Debug.Log ("red win");
			winnerTeamId = 1;
		} else if(syncTeam1GemCount < syncTeam2GemCount) {
			Debug.Log ("blue win");
			winnerTeamId = 2;
		} else {
			Debug.Log ("draw");
			winnerTeamId = 0;
		}
		UIManager.Instance.SetValueGameResultText (winnerTeamId);
		RpcEndGame (winnerTeamId);
	}

	[Command]
	public void CmdPopDeathItem(int itemId, int itemCount, Vector2 itemPopPosition){
		GenItem (itemId, itemCount, itemPopPosition, new Vector2 (1, 1));
		RpcPopDeathItem (itemId, itemCount, itemPopPosition);
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
		RpcPopItems (itemsPopPositionJson);

		foreach(var itemPopPosition in itemsPopPosition){
			GenItem(itemPopPosition.itemId, itemPopPosition.itemCount, itemPopPosition.itemPopPosition, itemPopPosition.itemScale);
		}
		myHomeFrame.SetActive (false);
		otherHomeFrame.SetActive (false);
	}

	[Command]
	public void CmdStartGameTimer(){
		isStartGame = true;
		RpcStartGameTimer ();
	}
		


	[ClientRpc]
	public void RpcStartGameTimer(){
		isStartGame = true;
	}

	[ClientRpc]
	public void RpcEndGame(int winnerTeamId){
		UIManager.Instance.SetValueGameResultText (winnerTeamId);
	}

	[ClientRpc]
	public void RpcPopItems(string itemsPopPositionJson){
		List<ItemData> itemsPopPosition = JsonUtility.FromJson<Serialization<ItemData>> (itemsPopPositionJson).ToList();

		foreach(var itemPopPosition in itemsPopPosition){
			GenItem(itemPopPosition.itemId, itemPopPosition.itemCount, itemPopPosition.itemPopPosition, itemPopPosition.itemScale);
		}
		myHomeFrame.SetActive (false);
		otherHomeFrame.SetActive (false);
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
	[ClientRpc]
	public void RpcPopDeathItem(int itemId, int itemCount, Vector2 itemPopPosition){
		GenItem (itemId, itemCount, itemPopPosition, new Vector2(1,1));
	}

	void SyncTeam1GemCountValue(int team1GemCount){
		UIManager.Instance.SetValueTeamGemCounter(1, team1GemCount);
	}

	void SyncTeam2GemCountValue(int team2GemCount){
		UIManager.Instance.SetValueTeamGemCounter(2, team2GemCount);
	}
}
