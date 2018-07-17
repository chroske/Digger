using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameCardView : MonoBehaviour {
	[SerializeField]
	Image cardImage;
	Vector3 currentCardUseablePosition;
	bool isMouseEnter;

	void OnMouseEnter() {
		Debug.Log ("OnMouseEnter");
		isMouseEnter = true;
		StartCoroutine(UpdateScale(new Vector3(1.3f, 1.3f, 1), 0.1f, isMouseEnter));
	}

	void OnMouseExit() {
		Debug.Log ("OnMouseExit");
		isMouseEnter = false;
		StartCoroutine(UpdateScale(new Vector3(1, 1, 1), 0.1f, isMouseEnter));
	}

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

	IEnumerator UpdateScale(Vector3 targetScale, float time, bool isMouseEnter)
	{
		float startTime = Time.time;

		do
		{
			float timeStep = time > 0.0f ? ( Time.time - startTime ) / time : 1.0f;
			this.gameObject.transform.localScale = Vector3.Lerp( this.gameObject.transform.localScale, targetScale, timeStep );

			yield return null;
		}
		while( (Time.time < startTime + time) && (this.isMouseEnter == isMouseEnter));
	}
}
