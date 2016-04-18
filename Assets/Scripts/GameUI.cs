using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FindObjectOfType<Player> ().onDeath += GameOver;
	}

	public void StartNewGame ()
	{
		Debug.Log ("Starting New Game");
		SceneManager.LoadScene ("Main");
	}

	void GameOver ()
	{
		StartCoroutine (Fade (Color.clear, Color.black, 1));
	}

	IEnumerator Fade (Color fadeFrom, Color fadeTo, float time)
	{
		float speed = 1 / time;
		float percent = 0;

		while (percent < 1) {
			percent += Time.deltaTime * speed;
			_fadePlane.color = Color.Lerp (fadeFrom, fadeTo, percent);
			yield return null;
		}
		_gameOverUI.SetActive (true);
	}

	public Image _fadePlane;
	public GameObject _gameOverUI;
}
