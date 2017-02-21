using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour {

	void Awake ()
	{
		_spawner = FindObjectOfType<Spawner> ();
		_player = FindObjectOfType<Player> ();
		_spawner.OnNewWave += OnNewWave;
		_spawner.OnEnemyKilled += OnEnemyDeath;
		_player.OnHit += OnPlayerHit;
		_healthBarStartLength = _healthBar.sizeDelta.x;
		_logger = FindObjectOfType<Logger> ();
	}

	// Use this for initialization
	void Start () {
		FindObjectOfType<Player> ().onDeath += GameOver;
		_playerMaxHealth = _player._health;
		_healthbar.Init (_playerMaxHealth, _playerMaxHealth);
		_xpBar.Init (100, 0); //max, curr
		_camera = FindObjectOfType<Camera> ();
	}

	public void StartNewGame ()
	{
		_logger.Debug ("Starting New Game");
		SceneManager.LoadScene ("Main");
	}

	void OnNewWave(int waveNumber)
	{
		StopCoroutine ("AnnimateWaveBanner");
		_waveNumber.text = "Wave " + (waveNumber + 1);
		_waveEnemyCount.text = "Enemies:" + _spawner._waves [waveNumber]._enemeyCount;
		_wavePlayerMessage.text = _spawner._waves [waveNumber]._playerMessage;
		StartCoroutine ("AnnimateWaveBanner");
	}

	IEnumerator AnnimateWaveBanner ()
	{
		int down = 1;
		int up = -1;

		float delayTime = 1.7f;
		float downSpeed = 2.5f;
		float upSpeed = 7f;
	    float annimationRange = 0f;

		int direction = down;
		float speed = downSpeed;
		bool annimationComplete = false;
		while (!annimationComplete) {
			annimationRange += Time.deltaTime * speed * direction;
			// reverse direction once we hit here
			if (annimationRange >= 1) {
				direction = up;
				speed = upSpeed;
				yield return new WaitForSeconds(delayTime);
			}
			// exit when other direction is done
			if (annimationRange < 0) {
				annimationComplete = true;
			}

			_newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp (-150, -400, annimationRange);
			yield return null;
		}
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
		_xpBar.gameObject.SetActive (false);
	}

	void OnPlayerHit(float damage)
	{
		_healthbar.Subtract (damage);
		// shake camera
	}

	void OnEnemyDeath()
	{
		_xpBar.Add (1);
	}

	public void SetDebugText (string text)
	{
		_debugText.text = text;
	}

	public Text _debugText;
	public Image _fadePlane;
	public GameObject _gameOverUI;
	public RectTransform _newWaveBanner;
	public Text _waveNumber;
	public Text _waveEnemyCount;
	public Text _wavePlayerMessage;
	Player _player;
	public RectTransform _healthBar;
	float _healthBarStartLength;
	float _playerMaxHealth;
	public StatBar _healthbar;
	public StatBar _xpBar;

	Spawner _spawner;

	Vector3 _cameraSmoothDampVelocity;

	Logger _logger;
	Camera _camera;
}
