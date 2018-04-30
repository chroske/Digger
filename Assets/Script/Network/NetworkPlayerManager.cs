using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerManager : NetworkBehaviour {

//    [SerializeField]
//    private GameObject characterPrefab;
    [SerializeField]
	private WeaponController weaponController;

    [SyncVar(hook = "SyncScaleValue")]
    public Vector3 syncScale;
	[SyncVar(hook = "SyncHpValue")]
	public float syncHp;
	[SyncVar(hook = "SyncWaeponBulletIndex")]
	public int syncWaeponBulletIndex;

	public struct holdItem
	{
		public int itemId;
		public int itemCount;
	};
	public class HoldItems : SyncListStruct<holdItem> {}
	public HoldItems syncListholdItems = new HoldItems();

    public DungeonController dungeonController;
	public UnityChan2DController unityChan2DController;

	//Dictionary<uint, NetworkPlayerManager> playersManagerDic = new Dictionary<uint, NetworkPlayerManager>();

    void Awake(){
        dungeonController = GameObject.Find("Dungeons").GetComponent<DungeonController>();
    }

    void Start(){
        if(isLocalPlayer){
            dungeonController.networkPlayerManager = this;
			GameStatusManager.Instance.myNetworkManager.myNetworkPlayerManager = this;
        }

		InitializeGameStageOnServer ();




	//	if(isServer){
	//		Dictionary<int, Vector2> test = new Dictionary<int, Vector2> ();
	//		test.Add (1, new Vector2(1f, -5f));
	//		RpcGenerateStageToServer (test);
	//	}
    }

    public override void OnStartLocalPlayer() { 
        this.gameObject.tag = "my_player_character";
		CmdProvideGenerateMineToServer ();
    }

//    void SpawnCharacter(){
//        var character = Instantiate(characterPrefab, transform.position, Quaternion.identity);
//        unityChan2DController = character.GetComponent<UnityChan2DController>();
//        unityChan2DController.networkPlayerManager = this;
//        NetworkServer.SpawnWithClientAuthority(character, connectionToClient);
//    }

	[Command]
	public void CmdProvideGenerateMineToServer(){
		GameStatusManager.Instance.playersManagerDic.Add(netId.Value, this);
	}

    [Command]
    public void CmdProvideScaleToServer(Vector3 scale){
        syncScale = scale;
    }

	[Command]
	public void CmdProvideChangeWaeponBulletToServer(int currentBulletsIndex){
		syncWaeponBulletIndex = currentBulletsIndex;
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
	public void CmdProvideHitDamageObjectOtherPlayerToServer(NetworkInstanceId hitPlayerNetId, float damage){
		GameStatusManager.Instance.playersManagerDic [hitPlayerNetId.Value].syncHp -= damage;
    }
	[Command]
	public void CmdGetItem(int itemPopId, int itemId, int itemCount){
		GameStatusManager.Instance.myNetworkManager.gameStageManager.DeleteGetItem (itemPopId);

//		GameStatusManager.Instance.myNetworkManager.gameStageManager.RpcPlayerGetItem(itemPopId);
//		GameStatusManager.Instance.myNetworkManager.gameStageManager.TargetGiveItem(connectionToClient, itemId, itemCount);
	}

	[Command]
	public void CmdProvideGetItemToServer(NetworkInstanceId getItemPlayerNetId, int itemId, int itemCount, int itemPopId){
		holdItem item = new holdItem ();
		item.itemId = itemId;
		item.itemCount = itemCount;
		GameStatusManager.Instance.playersManagerDic [getItemPlayerNetId.Value].syncListholdItems.Add(item);

		GameStatusManager.Instance.myNetworkManager.gameStageManager.DeleteGetItem (itemPopId);
		GameStatusManager.Instance.myNetworkManager.gameStageManager.RpcPlayerGetItem(itemPopId);
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

	[Server]
	void InitializeGameStageOnServer (){
		//GameStageManager.Instance.Initialize ();
	}

    void SyncScaleValue(Vector3 scale){
        if(!isLocalPlayer){
            transform.localScale = scale;
        }
    }

	void SyncHpValue(float hp){
		unityChan2DController.hp = hp;
		unityChan2DController.DoDamageAction ();
	}

	void SyncWaeponBulletIndex(int waeponBulletIndex){
		weaponController.ChangeWaeponBulletByBulletIndex (waeponBulletIndex);
	}
}
