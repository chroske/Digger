using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCardView : MonoBehaviour {
	Vector3 currentCardUseablePosition;

	public void SetCardPosition(Vector3 cardPosition)
	{
		currentCardUseablePosition = cardPosition;
	}

	//カードの位置
	public Vector3 CardPosition
	{
		get
		{
			return currentCardUseablePosition;
		}
	}
}
