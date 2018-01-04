using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerManager : NetworkBehaviour {

    [SerializeField]
    private WeaponController weaponController;

    [SyncVar(hook = "SyncScaleValue")]
    public Vector3 syncScale;

    public DungeonController dungeonController;

    void Awake(){
        dungeonController = GameObject.Find("Dungeons").GetComponent<DungeonController>();
    }

    void Start(){
        if(isLocalPlayer){
            dungeonController.networkPlayerManager = this;
        }
    }

    [Command]
    public void CmdProvideScaleToServer(Vector3 scale){
        syncScale = scale;
    }

    [Command]
    public void CmdProvideWeaponShotToServer(Vector3 shotWeaponVector){
        RpcWeaponShot(shotWeaponVector);
    }

    [Command]
    public void CmdProvideDigToServer(Vector2 digPosition, float digSizeRatio){
        RpcDig(digPosition, digSizeRatio);
    }

    [ClientRpc]
    void RpcWeaponShot(Vector3 shotWeaponVector){
        if(!isLocalPlayer){
            weaponController.ShotVector(shotWeaponVector);
        }
    }

    [ClientRpc]
    void RpcDig(Vector2 digPosition, float digSizeRatio){
        if(!isLocalPlayer){
            dungeonController.CreateDigCircle(digPosition, digSizeRatio);
        }
    }

    void SyncScaleValue(Vector3 scale){
        if(!isLocalPlayer){
            transform.localScale = scale;
        }
    }

}
