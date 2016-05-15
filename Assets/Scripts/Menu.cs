using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	void Start ()
	{
		_menuOpen = false;
		_player = FindObjectOfType<Player> ();

		_shootSpeedBar.Init (20,10);
		_moveSpeedBar.Init (20, 3);
		_damageBar.Init (20, 1);
	}

	public void IncrementShootSpeed ()
	{
		_player.GetComponent<GunController> ()._equippedGun._projectileVelocity++;
		_player.GetComponent<GunController> ()._equippedGun._millisBetweenShots-= 3;
		_shootSpeedBar.Add (1);
	}

	public void DecrementShootSpeed ()
	{
		_player.GetComponent<GunController> ()._equippedGun._projectileVelocity--;
		_player.GetComponent<GunController> ()._equippedGun._millisBetweenShots+= 3;
		_shootSpeedBar.Subtract (1);
	}

	public void IncrementDamage ()
	{
		_player.GetComponent<GunController> ()._equippedGun._gunDamage++;
		_damageBar.Add (1);
	}

	public void DecrementDamage ()
	{
		_player.GetComponent<GunController> ()._equippedGun._gunDamage++;
		_damageBar.Subtract (1);
	}

	public void IncrementMoveSpeed ()
	{
		_moveSpeedBar.Add (1);
		_player._moveSpeed = _player._moveSpeed + 1;
	}

	public void DecrementMoveSpeed ()
	{
		_moveSpeedBar.Subtract (1);
		_player._moveSpeed = _player._moveSpeed - 1;
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
	Player _player;

	public StatBar _shootSpeedBar;
	public StatBar _damageBar;
	public StatBar _moveSpeedBar;

	// SetUserPrefs
}
