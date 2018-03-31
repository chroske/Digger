using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameStatusManager : SingletonMonoBehaviourFast<GameStatusManager> {
	public MyNetworkManager myNetworkManager;
	public Dictionary<uint, NetworkPlayerManager> playersManagerDic = new Dictionary<uint, NetworkPlayerManager>();
}
