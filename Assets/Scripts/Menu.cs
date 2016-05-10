using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	void Start ()
	{
		_menuOpen = false;
	}

	public void OnArrowClick ()
	{
		Debug.Log ("clicked");
		if (!_menuOpen) {
			Open ();
		}
		else {
			Close ();
		}
		_menuOpen = !_menuOpen;
	}

	void Open ()
	{
		StartCoroutine ("AnnimateOpen");
	}

	IEnumerator AnnimateOpen ()
	{
		float speed = 10f;
		float annimatePercent = 0f;
		while (annimatePercent <= 1) 
		{
			annimatePercent += Time.deltaTime * speed;
			float yVal = Mathf.Lerp (800, -1100, annimatePercent);
			Vector2 modifiedVector = new Vector2 (_menuBox.anchoredPosition.x, yVal);
			_menuBox.anchoredPosition = modifiedVector;
			yield return null;
		}

		Time.timeScale = 0f;
	}

	void Close ()
	{
		StartCoroutine ("AnnimateClose");
	}

	IEnumerator AnnimateClose ()
	{
		Time.timeScale = 1f;

		float speed = 7f;
		float annimatePercent = 0f;
		while (annimatePercent <= 1) 
		{
			annimatePercent += Time.deltaTime * speed;
			float yVal = Mathf.Lerp (-1100, -5000, annimatePercent);
			Vector2 modifiedVector = new Vector2 (_menuBox.anchoredPosition.x, yVal);
			_menuBox.anchoredPosition = modifiedVector;
			yield return null;
		}
	}

	bool _menuOpen;
	public RectTransform _menuBox;

	// SetUserPrefs
}
