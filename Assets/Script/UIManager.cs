﻿using System.Collections;
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
	[SerializeField]
	Text GameResultText;

	public Text timerText;

	public void SetTeamIdByUIToggle(){
		if (TeamToggle1.isOn) {
			GameStatusManager.Instance.myNetworkManager.myNetworkPlayerManager.CmdProvidChangeTeamId (1);
			GameStatusManager.Instance.myNetworkManager.myNetworkPlayerManager.PlayerMoveHomePosition (1);
		} else if(TeamToggle2.isOn) {
			GameStatusManager.Instance.myNetworkManager.myNetworkPlayerManager.CmdProvidChangeTeamId (2);
			GameStatusManager.Instance.myNetworkManager.myNetworkPlayerManager.PlayerMoveHomePosition (2);
		}
	}

	public void OnClickGameStart(){
		PopRandomItems();
		GameStatusManager.Instance.myNetworkManager.gameStageManager.CmdStartGameTimer ();
	}

	void PopRandomItems(){
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

	public void SetValueGameResultText(int winnerTeamId){
		GameResultText.gameObject.SetActive (true);
		if (winnerTeamId == 1) {
			GameResultText.text = "RED TEAM WIN";
		} else if (winnerTeamId == 2) {
			GameResultText.text = "BLUE TEAM WIN";
		} else {
			GameResultText.text = "DRAW";
		}
	}
}
