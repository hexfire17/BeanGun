using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	void Start ()
	{
		_muzzleFlash = GetComponent <MuzzleFlash> ();
		_currBurst = 0;
	}

	public void Shoot()
	{
		if (Time.time > _nextShotTime)
		{
			for (int i = 0; i < _projectileSpawns.Length; i++) 
			{
				Transform currentSpawn = _projectileSpawns [i];
				Projectile projectile = Instantiate (_projectile, currentSpawn.position, currentSpawn.rotation) as Projectile;
				projectile.SetSpeed (_projectileVelocity);

				// Configure next shots
				_nextShotTime = Time.time + _millisBetweenShots / 1000;
				if (_fireMode == FireMode.Burst) {
					_currBurst++;
					if (_currBurst >= _maxBurt) {
						_currBurst = 0;
						_nextShotTime += _timeBetweenBurst;
					}
				}
			}
			// only do this for one muzzle TODO check if this is crazy to have a lot
			_muzzleFlash.Activate ();
			Instantiate (_shell, _shellEjectionPoint.position, _shellEjectionPoint.rotation);
		}
    }

	public enum FireMode {Auto, Burst};
	public FireMode _fireMode;

	int _maxBurt = 3;
	int _currBurst;
	float _timeBetweenBurst = .5f;

	float _nextShotTime;
	MuzzleFlash _muzzleFlash;

	public Transform[] _projectileSpawns;
	public Projectile _projectile;
	public float _millisBetweenShots = 100;
	public float _projectileVelocity = 35;
	public Transform _shell;
	public Transform _shellEjectionPoint;
}
