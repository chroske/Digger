using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerManager : NetworkBehaviour {

//    [SerializeField]
//    private GameObject characterPrefab;
    [SerializeField]
    private WeaponController weaponController;
    [SerializeField]
    private UnityChan2DController unityChan2DController;

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

    public override void OnStartLocalPlayer() { 
        this.gameObject.tag = "my_player_character";
    }

//    void SpawnCharacter(){
//        var character = Instantiate(characterPrefab, transform.position, Quaternion.identity);
//        unityChan2DController = character.GetComponent<UnityChan2DController>();
//        unityChan2DController.networkPlayerManager = this;
//        NetworkServer.SpawnWithClientAuthority(character, connectionToClient);
//    }

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

    [Command]
    public void CmdProvideHitDamageObjectOtherPlayerToServer(NetworkInstanceId hitPlayerNetId){
        RpcHitDamageObject(hitPlayerNetId, 1);
    }

    [ClientRpc]
    public void RpcHitDamageObject(NetworkInstanceId hitPlayerNetId, int damage){

//        hitPlayerNetIdとlocalPlayerのNetIdが同じだった場合
//        localPlayerにダメージ判定を実行する


        if(hitPlayerNetId == netId){
            unityChan2DController.DoDamageAction(damage);
        }
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
