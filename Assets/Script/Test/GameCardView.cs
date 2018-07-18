using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameCardView : MonoBehaviour {
	/* const */
	const int ON_MOUSE_SORTING_ORDER = 100;
	const float ON_MOUSE_CARD_SCALE = 1.3f;
	/* const */

	[SerializeField]
	float scaleCardAnimationTime = 0.1f;
	[SerializeField]
	Image cardImage;
	[SerializeField]
	Canvas canvas;
	[SerializeField]
	RectTransform rectTransform;

	Vector3 currentCardUseablePosition;
	Vector3 defaultPosition;
	Vector2 defaultPivot;
	Vector2 defaultScale;
	bool isMouseEnter;
	int defaultSortingOrder;

	void Awake(){
		defaultScale = transform.localScale;
	}

	void OnMouseDrag() {
		var mousePosition = Input.mousePosition;
		mousePosition.z = defaultPosition.z;
		var mousePositionOnWorld = Camera.main.ScreenToWorldPoint(mousePosition);
		mousePositionOnWorld.y -= rectTransform.sizeDelta.y / 2;
		transform.position = mousePositionOnWorld;
	}
		
	void OnMouseDown() {
		defaultPosition = transform.position;
	}

	void OnMouseUp() {
		transform.position = defaultPosition;
	}

	void OnMouseEnter() {
		isMouseEnter = true;
		defaultSortingOrder = canvas.sortingOrder;
		canvas.sortingOrder = ON_MOUSE_SORTING_ORDER;
		StartCoroutine(UpdateScale(new Vector3(ON_MOUSE_CARD_SCALE, ON_MOUSE_CARD_SCALE, 1), scaleCardAnimationTime, isMouseEnter));
	}

	void OnMouseExit() {
		isMouseEnter = false;
		canvas.sortingOrder = defaultSortingOrder;
		StartCoroutine(UpdateScale(defaultScale, scaleCardAnimationTime, isMouseEnter));
	}

	public void SetCardPosition(Vector3 cardPosition)
	{
		currentCardUseablePosition = cardPosition;
	}

	//カードの位置を返す
	public Vector3 CardPosition
	{
		get
		{
			return currentCardUseablePosition;
		}
	}

	//拡大アニメーション
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
