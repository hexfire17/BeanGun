using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	void Start()
	{
		Debug.Log ("Spawner started");

		_map = FindObjectOfType<MapGenerator> ();
		NextWave();	
	}
	
	void Update()
	{
		if (_enemiesRemainingToSpawn > 0 && Time.time > _nextSpawnTime)
		{
			Debug.Log ("Spawning Enemy");
			_enemiesRemainingToSpawn--;
			StartCoroutine (SpawnEnemy ());
		}
	}

	IEnumerator SpawnEnemy()
	{
		_nextSpawnTime = Time.time + _currentWave._timeBetweenWaves;

		float spawnDelay = 1;
		float tileFlashSpeed = 4;

		Transform tile = _map.getRandomTile ();
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
	
    [System.Serializable]
	public class Wave
	{
		public float _timeBetweenWaves;
		public int _enemeyCount;	
	}
}
