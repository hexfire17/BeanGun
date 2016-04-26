using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour
{
	void Start ()
	{
		Deactivate ();
	}

	public void Activate ()
	{
		_flashHolder.SetActive (true);

		int spriteIndex = Random.Range (0, _sprites.Length);
		for (int i = 0; i < _spriteRenderers.Length; i++) 
		{
			_spriteRenderers [i].sprite = _sprites [spriteIndex];
		}

		Invoke ("Deactivate", _flashTime);
	}

	void Deactivate ()
	{
		_flashHolder.SetActive (false);
	}

	public float _flashTime;
	public SpriteRenderer[] _spriteRenderers;
	public Sprite[] _sprites;
	public GameObject _flashHolder;
}
