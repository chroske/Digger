﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MyNetworkManager : NetworkManager {

	public NetworkPlayerManager myNetworkPlayerManager;
    [SerializeField]
    GameStageManager gameStageManagerPrefab;

	public GameStageManager gameStageManager;

	public override void OnServerConnect(NetworkConnection connection)
	{
		//Change the text to show the connection and the client's ID
		Debug.Log ("player connected:"+connection.connectionId.ToString());
		if(connection.connectionId.ToString() != "0"){
        } else {
            GameObject genGameStageManager = (GameObject)Instantiate(gameStageManagerPrefab.gameObject, transform.position, transform.rotation);
			NetworkServer.Spawn(genGameStageManager);
			gameStageManager = genGameStageManager.GetComponent<GameStageManager> ();
			UIManager.Instance.SetGameStartButton (true); //サーバだけスタートボタンを表示するやつ
        }
	}
}
