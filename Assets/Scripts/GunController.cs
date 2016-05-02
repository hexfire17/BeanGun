using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
	void Start()
	{
		if (_startingGun != null)
		{
			Debug.Log("Equipted starting gun");
			Equip(_startingGun);
		}
	}

	public void Equip(Gun gun)
	{
		if (_equippedGun != null)
		{
			Debug.Log("Equipped gun destroyed");
			Destroy(_equippedGun.gameObject);
		}
		_equippedGun = Instantiate (gun, _weaponHold.position, _weaponHold.rotation) as Gun;
		_equippedGun.transform.parent = _weaponHold;
	}

	public void Shoot()
	{
		if (_equippedGun != null)
		{
			_equippedGun.Shoot();
		}
	}

	public Transform _weaponHold;
	public Gun _startingGun;
	public Gun _equippedGun;
}
