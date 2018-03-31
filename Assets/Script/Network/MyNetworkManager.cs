using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MyNetworkManager : NetworkManager {

	public NetworkPlayerManager myNetworkPlayerManager;

	public override void OnServerConnect(NetworkConnection connection)
	{
		//Change the text to show the connection and the client's ID
		Debug.Log ("player connected:"+connection.connectionId.ToString());
		if(connection.connectionId.ToString() != "0"){
			//myNetworkPlayerManager.TargetStageState(connection, "test");
			//myNetworkPlayerManager.CmdTest ("test");
		}

	}
}
