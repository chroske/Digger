using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerManager : NetworkBehaviour {

    [SyncVar(hook = "SyncScaleValue")]
    public Vector3 syncScale;


    [Command]
    public void CmdProvideScaleToServer(Vector3 scale){
        syncScale = scale;
    }

    [Command]
    public void CmdProvideWeaponShotToServer(){
        
    }

    void SyncScaleValue(Vector3 scale){
        transform.localScale = scale;
    }
   
}
