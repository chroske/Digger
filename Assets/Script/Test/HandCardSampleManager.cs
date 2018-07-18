using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCardSampleManager : MonoBehaviour {

	[SerializeField]
	GameObject cardPrefab;
	[SerializeField]
	PlayerHandCardView playerHandCardView;

	public void OnClickGenerateCard(){
		var card = Instantiate (cardPrefab);
		card.transform.SetParent (playerHandCardView.gameObject.transform);
		playerHandCardView.ResetPositionHandCard ();
	}
}