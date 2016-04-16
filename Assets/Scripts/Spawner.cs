using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	void Start()
	{
		NextWave();	
	}
	
	void Update()
	{
		if (_enemiesRemainingToSpawn > 0 && Time.time > _nextSpawnTime)
		{
			_enemiesRemainingToSpawn--;
			_nextSpawnTime = Time.time + _currentWave._timeBetweenWaves;
			
			Enemy spawnedEnemy = Instantiate(_enemy, Vector3.zero, Quaternion.identity) as Enemy;
			spawnedEnemy.onDeath += OnEnemyDeath;
		}
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
	
    [System.Serializable]
	public class Wave
	{
		public float _timeBetweenWaves;
		public int _enemeyCount;	
	}
}
