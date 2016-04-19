using UnityEngine;
using System.Collections;

public interface IDamageable
{
	void takeHit (float damage, Vector3 hitPoint, Vector3 hitDirection);

	void takeDamage(float damage);
}
