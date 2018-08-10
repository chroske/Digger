using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameCardView : MonoBehaviour {
	/* const */
	const int ON_MOUSE_SORTING_ORDER = 100;
	const float ON_MOUSE_CARD_SCALE = 1.5f;
    const float ON_MOUSE_CARD_RISE = 190.0f;
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
    Quaternion defaultRotation;

	Vector3 defaultImagePosition;
	Quaternion defaultImageRotation;
	Vector2 defaultImageScale;
    Vector2 defaultPivot;
	bool isMouseEnter;
    bool isMouseDown;
	int defaultSortingOrder;

	void Awake(){
        defaultImageScale = cardImage.transform.localScale;
	}

	void OnMouseDrag() {
		var mousePosition = Input.mousePosition;
		mousePosition.z = defaultPosition.z;
		var mousePositionOnWorld = Camera.main.ScreenToWorldPoint(mousePosition);
		mousePositionOnWorld.y -= rectTransform.sizeDelta.y / 2;
		transform.position = mousePositionOnWorld;
        cardImage.transform.rotation = Quaternion.identity;
	}
		
	void OnMouseDown() {
        isMouseDown = true;
		defaultPosition = transform.position;
		defaultRotation = transform.rotation;
        cardImage.transform.position = defaultImagePosition;
	}

	void OnMouseUp() {
        isMouseDown = false;
		transform.position = defaultPosition;
        transform.rotation = defaultRotation;
        canvas.sortingOrder = defaultSortingOrder;
        cardImage.transform.position = defaultImagePosition;
        cardImage.transform.rotation = defaultImageRotation;
        cardImage.transform.localScale = defaultImageScale;
	}

	void OnMouseEnter() {
        if(!Input.GetMouseButton(0)){
            isMouseEnter = true;
            defaultSortingOrder = canvas.sortingOrder;
            defaultImagePosition = cardImage.transform.position;
            defaultImageRotation = cardImage.transform.rotation;
            cardImage.transform.rotation = Quaternion.identity;
            cardImage.transform.position = new Vector3(cardImage.transform.position.x, cardImage.transform.position.y + ON_MOUSE_CARD_RISE, cardImage.transform.position.z);
            canvas.sortingOrder = ON_MOUSE_SORTING_ORDER;
			StartCoroutine(UpdateScale(new Vector3(ON_MOUSE_CARD_SCALE, ON_MOUSE_CARD_SCALE, 1), scaleCardAnimationTime, isMouseEnter));
		}
	}

	void OnMouseExit() {
        if ((!Input.GetMouseButton(0) || isMouseEnter) && !isMouseDown) {
			isMouseEnter = false;
            canvas.sortingOrder = defaultSortingOrder;
            cardImage.transform.position = defaultImagePosition;
            cardImage.transform.rotation = defaultImageRotation;
			StartCoroutine (UpdateScale (defaultImageScale, scaleCardAnimationTime, isMouseEnter));
		}
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
            cardImage.transform.localScale = Vector3.Lerp(cardImage.transform.localScale, targetScale, timeStep);

			yield return null;
		}
		while( (Time.time < startTime + time) && (this.isMouseEnter == isMouseEnter));
	}
}
