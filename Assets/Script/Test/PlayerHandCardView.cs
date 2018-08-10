using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerHandCardView : MonoBehaviour {

	public List<Canvas> handCardObjects;
	public float cardAngleInterval = 5f; //数が大きいほど斜めになるよ
	public float handCardLowerRatio = 10; //数が小さいほど外側が下がるよ
	public float ResetPositionAnimationTime = 1.0f; //数が大きいほどゆっくりアニメーションするよ
	public float handCardMargin; //数が大きいほどカード間の幅が広がるよ
	public float screenWidth = 1080; //手札幅上限

	float cardBasewidth;
	float handCardInterval = 0;


//	void OnEnable(){
//		ResetPositionHandCard ();
//	}

	//手札の位置整え
	public void ResetPositionHandCard(){

		//子オブジェクト（Card）をリストに追加
		handCardObjects = new List<Canvas>();
		for (int j = 0; j < transform.childCount; j++)
		{
			if (transform.GetChild(j).gameObject.activeSelf)
			{
				handCardObjects.Add(transform.GetChild(j).gameObject.GetComponent<Canvas>());
			}
		}

		//一番左のカードを基準のサイズとする
		if(cardBasewidth <= 0){
			cardBasewidth = handCardObjects [0].GetComponent<RectTransform>().sizeDelta.x;
		}
			
		if ((cardBasewidth + handCardMargin) * handCardObjects.Count < screenWidth)
		{
			handCardInterval = cardBasewidth + handCardMargin;
		}
		else
		{
			if(handCardMargin > 0){
				handCardMargin = 0;
			}
			handCardInterval = (screenWidth / (handCardObjects.Count)) + handCardMargin;
		}

		float handCardAngle = cardAngleInterval*handCardObjects.Count / handCardObjects.Count;
		Vector3 baseLeftCardPosition = new Vector3(handCardInterval*(handCardObjects.Count - 1)/-2, 0, 0);
		Vector3 baseLeftCardRotate = new Vector3(0, 0, cardAngleInterval*(handCardObjects.Count-1)/2);

		if (baseLeftCardPosition.x < screenWidth /-2)
		{
			baseLeftCardPosition = new Vector3(screenWidth /-2, 0, 0);
		}

		//総和
		int totalSum = 0;
		for (int j=1; j<=handCardObjects.Count; j++) {
			totalSum += j;
		}
		//中央値
		float CenterNum = (float)totalSum/(float)handCardObjects.Count;

		int i = 0;
		foreach (Canvas obj in handCardObjects) {
			obj.sortingOrder = i;
			Vector3 targetPosition = new Vector3 (baseLeftCardPosition.x+handCardInterval*i, (handCardLowerRatio/handCardObjects.Count)*Mathf.Abs(CenterNum-(i+1)), 0);

			obj.GetComponent<GameCardView>().SetCardPosition(targetPosition);
			Vector3 targetRotate = new Vector3 (0, 0, baseLeftCardRotate.z-handCardAngle*i);
			StartCoroutine (UpdatePosition (targetPosition, targetRotate, ResetPositionAnimationTime, obj.gameObject));
			i++;
		}
	}

	IEnumerator UpdatePosition(Vector3 targetPosition, Vector3 targetRotate, float time, GameObject obj )
	{
		float startTime = Time.time;

		do
		{
			float timeStep = time > 0.0f ? ( Time.time - startTime ) / time : 1.0f;
			obj.transform.localPosition = Vector3.Lerp( obj.transform.localPosition, obj.GetComponent<GameCardView>().CardPosition, timeStep );
			obj.transform.localRotation = Quaternion.Lerp(obj.transform.localRotation, Quaternion.Euler(targetRotate), timeStep);

			yield return null;
		}
		while( Time.time < startTime + time );
	}
}  