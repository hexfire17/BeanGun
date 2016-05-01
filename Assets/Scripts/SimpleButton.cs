using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleButton : MonoBehaviour
{
	public void Start ()
	{
		_button = GetComponent<Button> ();
		_button.onClick.AddListener (OnDown);
	}

	public bool isClicked ()
	{
		return _isClicked;
	}

	private void OnDown ()
	{
		_isClicked = true;
	}

	private void OnUp ()
	{
		_isClicked = false;
	}

	private Button _button;
	private bool _isClicked;
}
