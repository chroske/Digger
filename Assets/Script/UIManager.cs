using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviourFast<UIManager> {
	[SerializeField]
	Toggle TeamToggle1;
	[SerializeField]
	Toggle TeamToggle2;
	[SerializeField]
	Text GemCounterTeam1;
	[SerializeField]
	Text GemCounterTeam2;
	[SerializeField]
	GameObject GameStartButton;


	public void SetTeamIdByUIToggle(){
		if (TeamToggle1.isOn) {
			GameStatusManager.Instance.myNetworkManager.myNetworkPlayerManager.CmdProvidChangeTeamId (1);
		} else if(TeamToggle2.isOn) {
			GameStatusManager.Instance.myNetworkManager.myNetworkPlayerManager.CmdProvidChangeTeamId (2);
		}
	}

	public void PopRandomItems(){
		GameStatusManager.Instance.myNetworkManager.gameStageManager.CmdRandomPopItems ();
	}

	public void SetGameStartButton(bool isServer){
		GameStartButton.SetActive (isServer);
	}

	public void SetValueTeamGemCounter(int teamId, int count){
		if (teamId == 1) {
			GemCounterTeam1.text = count.ToString ();
		} else if(teamId == 2) {
			GemCounterTeam2.text = count.ToString ();
		}

	}
}
