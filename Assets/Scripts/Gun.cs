using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	void Start ()
	{
		_muzzleFlash = GetComponent <MuzzleFlash> ();
	}

	public void Shoot()
	{
		if (Time.time > _nextShotTime) 
		{
			_nextShotTime = Time.time + _millisBetweenShots / 1000;
			Projectile projectile = Instantiate (_projectile, _muzzle.position, _muzzle.rotation) as Projectile;
			projectile.SetSpeed (_projectileVelocity);

			_muzzleFlash.Activate ();
			Instantiate (_shell, _shellEjectionPoint.position, _shellEjectionPoint.rotation);
		}
	}

	float _nextShotTime;
	MuzzleFlash _muzzleFlash;

	public Transform _muzzle;
	public Projectile _projectile;
	public float _millisBetweenShots = 100;
	public float _projectileVelocity = 35;
	public Transform _shell;
	public Transform _shellEjectionPoint;
}
