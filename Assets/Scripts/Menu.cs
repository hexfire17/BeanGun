using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	void Start ()
	{
		_menuOpen = false;
		_player = FindObjectOfType<Player> ();
	}

	public void IncrementShootSpeed ()
	{
		_player.GetComponent<GunController> ()._equippedGun._projectileVelocity++;
		_player.GetComponent<GunController> ()._equippedGun._millisBetweenShots--;
	}

	public void DecrementShootSpeed ()
	{
		_player.GetComponent<GunController> ()._equippedGun._projectileVelocity--;
		_player.GetComponent<GunController> ()._equippedGun._millisBetweenShots++;
	}

	public void IncrementDamage ()
	{
		_player.GetComponent<GunController> ()._equippedGun._gunDamage++;
	}

	public void DecrementDamage ()
	{
		_player.GetComponent<GunController> ()._equippedGun._gunDamage++;
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
			float xVal = Mathf.Lerp (77, 2000, annimatePercent);
			Vector2 modifiedVector = new Vector2 (xVal, _menuBox.anchoredPosition.y);
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
			float xVal = Mathf.Lerp (2000, 5000, annimatePercent);
			Vector2 modifiedVector = new Vector2 (xVal, _menuBox.anchoredPosition.y);
			_menuBox.anchoredPosition = modifiedVector;
			yield return null;
		}
	}

	bool _menuOpen;
	public RectTransform _menuBox;
	public Player _player;

	// SetUserPrefs
}
