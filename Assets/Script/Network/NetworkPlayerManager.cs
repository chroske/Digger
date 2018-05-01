using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
	[SyncVar(hook = "SyncTeamIdValue")]
	public int syncTeamId;

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
			UIManager.Instance.SetTeamIdByUIToggle();
        }

		InitializeGameStageOnServer ();
    }

    public override void OnStartLocalPlayer() { 
        this.gameObject.tag = "my_player_character";
		this.gameObject.layer = 13;
		CmdProvideGenerateMineToServer ();
    }

	public override void OnStartClient() {
		SyncTeamIdValue (syncTeamId);
	}



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

	[Command]
	public void CmdDeliverGemToServer(NetworkInstanceId getItemPlayerNetId, int gemCount){
		int teamId = GameStatusManager.Instance.playersManagerDic [getItemPlayerNetId.Value].syncTeamId;
		if (teamId == 1) {
			GameStatusManager.Instance.myNetworkManager.gameStageManager.syncTeam1GemCount += gemCount;
		} else if(teamId == 2){
			GameStatusManager.Instance.myNetworkManager.gameStageManager.syncTeam2GemCount += gemCount;
		}
	}

	[Command]
	public void CmdProvidChangeTeamId(int teamId){
		syncTeamId = teamId;
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

	void SyncTeamIdValue(int teamId){
		if(teamId == 1){
			this.gameObject.tag = "my_player_character";
			this.gameObject.layer = 13;
			unityChan2DController.hpSlider.fillRect.GetComponent<Image>().color = Color.red;
		} else if(teamId == 2){
			this.gameObject.tag = "other_player_character";
			this.gameObject.layer = 14;
			unityChan2DController.hpSlider.fillRect.GetComponent<Image>().color = Color.blue;
		}
	}

	void SyncWaeponBulletIndex(int waeponBulletIndex){
		weaponController.ChangeWaeponBulletByBulletIndex (waeponBulletIndex);
	}
}
