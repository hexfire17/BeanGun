using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	void Start()
	{
		Debug.Log ("Spawner started");

		_playerDead = false;
		_player = FindObjectOfType<Player> ();
		_playerTransform = _player.transform;

		_nextCampCheckTime = _campCheckInterval + Time.time;
		_previousCampPosition = _playerTransform.position;

		_map = FindObjectOfType<MapGenerator> ();
		_player.onDeath += OnPlayerDeath;
		NextWave();	
	}
	
	void Update()
	{
		if (_playerDead) {return;}

		if (Time.time > _nextCampCheckTime)
		{
			_nextCampCheckTime = Time.time + _campCheckInterval;
			_isCamping = (Vector3.Distance (_playerTransform.position, _previousCampPosition) < _campThresholdDistance);
			_previousCampPosition = _playerTransform.position;
		}

		if (_enemiesRemainingToSpawn > 0 && Time.time > _nextSpawnTime)
		{
			_enemiesRemainingToSpawn--;
			StartCoroutine ("SpawnEnemy");
		}

		if (_devMode && Input.GetKeyDown (KeyCode.Return)) {
			StopCoroutine ("SpawnEnemy");
			foreach (Enemy enemy in FindObjectsOfType<Enemy> ()) {
				GameObject.Destroy (enemy.gameObject);
			}
			NextWave ();
		}
	}

	IEnumerator SpawnEnemy()
	{
		Debug.Log ("Spawning enemy, remaining: " + _enemiesRemainingToSpawn);
		_nextSpawnTime = Time.time + _currentWave._timeBetweenSpawns;

		float spawnDelay = 1;
		float tileFlashSpeed = 4;

		Transform tile = _map.getRandomTile ();
		if (_isCamping) {
			Debug.Log ("Using camper tile");
			tile = _map.getTile (_playerTransform.position);
		}

		Material tileMaterial = tile.GetComponent<Renderer> ().material;
		Color tileColor = tileMaterial.color;
		Color flashColor = Color.red;
		float spawnTimer = 0;
		while (spawnTimer < spawnDelay)
		{
			tileMaterial.color = Color.Lerp (tileColor, flashColor, Mathf.PingPong (spawnTimer * tileFlashSpeed, 1));
			spawnTimer += Time.deltaTime;
			yield return null; // TODO figure out why
		}
		tileMaterial.color = tileColor;


		Enemy spawnedEnemy = Instantiate(_enemy, tile.position + Vector3.up, Quaternion.identity) as Enemy;
		spawnedEnemy.setCharacteristics (_currentWave._enemySpeed, _currentWave._enemyDamage, _currentWave._enemyAttackDistance, _currentWave._enemyHealth, _currentWave._enemyColor);
		spawnedEnemy.onDeath += OnEnemyDeath;
	}
	
	void NextWave()
	{
		if (OnNewWave != null) {
			OnNewWave (_currentWaveNumber);
		}

		if (_currentWaveNumber < _waves.Length)
		{
	  		_currentWave = _waves[_currentWaveNumber];
			_currentWaveNumber++;
			_enemiesRemainingToSpawn = _currentWave._enemeyCount;
			_enemiesRemaining = _currentWave._enemeyCount;
		}
		ResetPlayerPosition (); 
	}

	void ResetPlayerPosition ()
	{
		_player.transform.position = _map.getTile (Vector3.zero).position + Vector3.up;
		_player._grounded = true;
	}
	
	void OnEnemyDeath()
	{
		_enemiesRemaining--;
		if (_enemiesRemaining == 0)
		{
			NextWave();	
		}
		Debug.Log ("Enemy Died");
	}

	void OnPlayerDeath()
	{
		_playerDead = true;
	}

	public event System.Action<int> OnNewWave;
	 
	int _enemiesRemaining;
	
	Wave _currentWave;
	int _currentWaveNumber;
	
	int _enemiesRemainingToSpawn;
	float _nextSpawnTime;
	
	public Wave[] _waves;
	public Enemy _enemy;

	MapGenerator _map;

	Player _player;
	Transform _playerTransform;

	float _campCheckInterval = 2;
	float _campThresholdDistance = 1.5f;
	float _nextCampCheckTime;
	Vector3 _previousCampPosition;
	bool _isCamping;
	bool _playerDead;

	public bool _devMode;
	
    [System.Serializable]
	public class Wave
	{
		public float _timeBetweenSpawns;
		public int _enemeyCount;	
		public float _enemySpeed;
		public int _enemyHealth;
		public int _enemyDamage;
		public float _enemyAttackDistance;
		public Color _enemyColor;
	}
}
