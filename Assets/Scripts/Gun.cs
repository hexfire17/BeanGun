using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public void Shoot()
	{
		if (Time.time > _nextShotTime) 
		{
			_nextShotTime = Time.time + _millisBetweenShots / 1000;
			Projectile projectile = Instantiate (_projectile, _muzzle.position, _muzzle.rotation) as Projectile;
			projectile.SetSpeed (_projectileVelocity);
		}
	}

	float _nextShotTime;

	public Transform _muzzle;
	public Projectile _projectile;
	public float _millisBetweenShots = 100;
	public float _projectileVelocity = 35;
}
