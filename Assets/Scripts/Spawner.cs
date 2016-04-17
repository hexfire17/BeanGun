using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	void Start()
	{
		Debug.Log ("Spawner started");

		_playerEntity = FindObjectOfType<Player> ();
		_playerTransform = _playerEntity.transform;

		_nextCampCheckTime = _campCheckInterval + Time.time;
		_previousCampPosition = _playerTransform.position;

		_map = FindObjectOfType<MapGenerator> ();
		NextWave();	
	}
	
	void Update()
	{
		if (Time.time > _nextCampCheckTime)
		{
			_nextCampCheckTime = Time.time + _campCheckInterval;
			_isCamping = (Vector3.Distance (_playerTransform.position, _previousCampPosition) < _campThresholdDistance);
			_previousCampPosition = _playerTransform.position;
		}

		if (_enemiesRemainingToSpawn > 0 && Time.time > _nextSpawnTime)
		{
			_enemiesRemainingToSpawn--;
			StartCoroutine (SpawnEnemy ());
		}
	}

	IEnumerator SpawnEnemy()
	{
		Debug.Log ("Spawning enemy, remaining: " + _enemiesRemainingToSpawn);
		_nextSpawnTime = Time.time + _currentWave._timeBetweenWaves;

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
		spawnedEnemy.onDeath += OnEnemyDeath;
	}
	
	void NextWave()
	{
		if (_currentWaveNumber < _waves.Length)
		{
	  		_currentWave = _waves[_currentWaveNumber];
			_currentWaveNumber++;
			_enemiesRemainingToSpawn = _currentWave._enemeyCount;
			_enemiesRemaining = _currentWave._enemeyCount;
		}
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
	 
	int _enemiesRemaining;
	
	Wave _currentWave;
	int _currentWaveNumber;
	
	int _enemiesRemainingToSpawn;
	float _nextSpawnTime;
	
	public Wave[] _waves;
	public Enemy _enemy;

	MapGenerator _map;

	LivingEntitiy _playerEntity;
	Transform _playerTransform;

	float _campCheckInterval = 2;
	float _campThresholdDistance = 1.5f;
	float _nextCampCheckTime;
	Vector3 _previousCampPosition;
	bool _isCamping;
	
    [System.Serializable]
	public class Wave
	{
		public float _timeBetweenWaves;
		public int _enemeyCount;	
	}
}
