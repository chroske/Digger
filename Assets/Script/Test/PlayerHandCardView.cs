using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandCardView : MonoBehaviour {

	public List<GameObject> handCardObjects;
	public float handCardInterval = 100f; //数が大きいほど幅ができるよ
	public float cardAngleInterval = 5f; //数が大きいほど斜めになるよ
	public float handCardLowerRatio = 10; //数が小さいほど外側が下がるよ
	public float ResetPositionAnimationTime = 1.0f; //数が大きいほどゆっくりアニメーションするよ

	float cardBasewidth = 310;
	float screenWidth = 1080;

	void Start(){
		ResetPositionHandCard ();
	}

	//手札の位置整え
	public void ResetPositionHandCard(){

		handCardObjects = new List<GameObject>();
		for (int j = 0; j < transform.childCount; j++)
		{
			if (transform.GetChild(j).gameObject.activeSelf)
			{
				handCardObjects.Add(transform.GetChild(j).gameObject);
			}
		}

		if (cardBasewidth * handCardObjects.Count < screenWidth)
		{
			handCardInterval = cardBasewidth;
		}
		else
		{
			handCardInterval = screenWidth / (handCardObjects.Count);
		}

		float handCardAngle = cardAngleInterval*handCardObjects.Count / handCardObjects.Count;
		Vector3 baseLeftCardPosition = new Vector3(handCardInterval*(handCardObjects.Count - 1)/-2, 0, 0);
		Vector3 baseLeftCardRotate = new Vector3(0, 0, cardAngleInterval*(handCardObjects.Count-1)/2);

		if (baseLeftCardPosition.x < -0.5f * screenWidth + cardBasewidth * 0.5f)
		{
			baseLeftCardPosition = new Vector3(-0.5f * screenWidth + cardBasewidth * 0.5f, 0, 0);
		}

		int i = 0;
		foreach (GameObject obj in handCardObjects) {
			Vector3 targetPosition = new Vector3 (baseLeftCardPosition.x+handCardInterval*i, Mathf.Abs(baseLeftCardPosition.x+handCardInterval*i)/-handCardLowerRatio, 0);
			obj.GetComponent<GameCardView>().SetCardPosition(targetPosition);
			Vector3 targetRotate = new Vector3 (0, 0, baseLeftCardRotate.z-handCardAngle*i);
			StartCoroutine (UpdatePosition (targetPosition, targetRotate, ResetPositionAnimationTime, obj));
			i++;
		}
	}

	IEnumerator UpdatePosition(Vector3 targetPosition, Vector3 targetRotate, float time, GameObject obj )
	{
		float   startTime = Time.time;

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