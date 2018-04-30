using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviourFast<UIManager> {
	[SerializeField]
	Toggle TeamToggle1;
	[SerializeField]
	Toggle TeamToggle2;

	public void SetTeamIdByUIToggle(){
		if (TeamToggle1.isOn) {
			GameStatusManager.Instance.myNetworkManager.myNetworkPlayerManager.CmdProvidChangeTeamId (1);
		} else if(TeamToggle2.isOn) {
			GameStatusManager.Instance.myNetworkManager.myNetworkPlayerManager.CmdProvidChangeTeamId (2);
		}
	}

	public void OnValueChangeTeamToggle1(Toggle toggle){
		if(toggle.isOn){
			GameStatusManager.Instance.myNetworkManager.myNetworkPlayerManager.CmdProvidChangeTeamId(1);
		}
	}

	public void OnValueChangeTeamToggle2(Toggle toggle){
		if(toggle.isOn){
			GameStatusManager.Instance.myNetworkManager.myNetworkPlayerManager.CmdProvidChangeTeamId(2);
		}
	}
}
