﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class StatBar : MonoBehaviour {

	public void Start ()
	{
		_valueRectStartLength = _fill.sizeDelta.x;
	}

	public void Add(float points)
	{
		float percentLost = points / _maxValue;
		float sizeMinus = _valueRectStartLength * percentLost;
		_fill.sizeDelta -= new Vector2 (sizeMinus, 0);
		_fill.anchoredPosition -= new Vector2 (sizeMinus, 0) * .5f;
	}

	public RectTransform _fill;
	public RectTransform _background;
	public float _maxValue;
	float _valueRectStartLength;
}
