using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerManager : NetworkBehaviour {

    [SyncVar]
    public Vector3 syncScale;


    [Command]
    public void CmdProvideScaleToServer(Vector3 scale){
        syncScale = scale;
        //        if(syncPlayerNetID == gameSceneManager.turnPlayerId){
        //            //次のplayerIdを生成
        //            int nextTurnPlayerId = InclementTurnPlayerId(gameSceneManager.turnPlayerId);
        //            //全クライアントをターンエンドさせる
        //            RpcTurnEndClient(unitPos, nextTurnPlayerId);
        //        }
    }
}
