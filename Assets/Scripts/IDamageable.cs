using UnityEngine;
using System.Collections;

public interface IDamageable
{
	void takeHit(float damage, RaycastHit hit);
	
	void takeDamage(float damage);
}
