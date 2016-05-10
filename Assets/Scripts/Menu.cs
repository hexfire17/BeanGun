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
		Debug.Log ("opening");
		Time.timeScale = 0f;
	}

	void Close ()
	{
		Debug.Log ("closing");
		Time.timeScale = 1f;
	}

	bool _menuOpen;

	// SetUserPrefs
}
