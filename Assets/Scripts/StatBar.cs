using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class StatBar : MonoBehaviour {

	public void Start ()
	{
		_valueRectStartLength = _fill.sizeDelta.x;
		Debug.Log ("StartSize: " + _valueRectStartLength);
	}

	public void Add(float points)
	{
		Debug.Log ("PointAdd: " + points);
		float percentLost = points / _maxValue;
		Debug.Log ("Plost: " + percentLost);
		float sizeMinus = _valueRectStartLength * percentLost;
		Debug.Log ("SizeMinus: " + sizeMinus);
		_fill.sizeDelta -= new Vector2 (sizeMinus, 0);
		_fill.anchoredPosition -= new Vector2 (sizeMinus, 0) * .5f;
	}

	public void Subtract(float points)
	{
		Debug.Log ("PointSub: " + points);
		float percentLost = points / _maxValue;
		Debug.Log ("Plost: " + percentLost);
		float sizeMinus = _valueRectStartLength * percentLost;
		Debug.Log ("SizeMinus: " + sizeMinus);
		_fill.sizeDelta -= new Vector2 (sizeMinus, 0);
		_fill.anchoredPosition -= new Vector2 (sizeMinus, 0) * .5f;
	}

	public RectTransform _fill;
	public RectTransform _background;
	public float _maxValue { set; get;}
	float _valueRectStartLength;
}
