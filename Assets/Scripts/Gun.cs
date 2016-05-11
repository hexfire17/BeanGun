using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	void Start ()
	{
		_muzzleFlash = GetComponent <MuzzleFlash> ();
		_currBurst = 0;
	}

	// TODO LateUpdate does it later

	public void Update ()
	{
		// animate recoil
		float timeToReturn = .1f;
		transform.localPosition = Vector3.SmoothDamp (transform.localPosition, Vector3.zero, ref _recoilSmoothDampVelocity, timeToReturn);

		// reduce recoil over time
		float recoilReturn = .1f;
		_recoilAngle = Mathf.SmoothDamp (_recoilAngle, 0f, ref _recoilAngleSmoothDampVelocity, recoilReturn);
		transform.localEulerAngles = Vector3.left * _recoilAngle;
	}

	public void Shoot()
	{
		if (Time.time > _nextShotTime)
		{
			for (int i = 0; i < _projectileSpawns.Length; i++) 
			{
				Transform currentSpawn = _projectileSpawns [i];
				Projectile projectile = Instantiate (_projectile, currentSpawn.position, currentSpawn.rotation) as Projectile;
				projectile._speed = _projectileVelocity;

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
			transform.localPosition -= Vector3.forward * .2f;
			_recoilAngle += _recoilIncrement;
			_recoilAngle = Mathf.Clamp (_recoilAngle, 0, _maxRecoilAngle);
		}
    }

	public enum FireMode {Auto, Burst};
	public FireMode _fireMode;

	int _maxBurt = 3;
	int _currBurst;
	float _timeBetweenBurst = .5f;

	float _nextShotTime;
	MuzzleFlash _muzzleFlash;

	// recoil
	Vector3 _recoilSmoothDampVelocity;
	float _recoilAngleSmoothDampVelocity;


	float _recoilAngle;

	[Header("TestHeader")]
	public Transform[] _projectileSpawns;
	public Projectile _projectile;
	public float _millisBetweenShots = 100;
	public float _projectileVelocity = 35;
	[Header("TestHeader2")]
	public Transform _shell;
	public Transform _shellEjectionPoint;

	public float _maxRecoilAngle = 15;
	public float _recoilIncrement = 5;
	public float _maxRecoilRandomDeviation = 0; // TODO use this!
}
