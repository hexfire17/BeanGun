using UnityEngine;
using System.Collections;

public class LivingEntitiy : MonoBehaviour, IDamageable {
	
	public virtual void Start()
	{
		_health = _startingHealth;	
	}
	
		public void takeHit(float damage, RaycastHit hit)
	{
		// do some stuff with hit later
		takeDamage(damage);
	}
	
	public void takeDamage(float damage)
	{
		_health -= damage;
		if (_health <= 0 && _isAlive)
		{
			Die();	
		}	
	}

	
	void Die()
	{
		Debug.Log("Dead: " + gameObject.name);
		_isAlive = false;	
		GameObject.Destroy(gameObject);
		
		if (onDeath != null)
		{
			onDeath();	
		}
	}
	
	public event System.Action onDeath;
	
	public float _startingHealth;
	
	protected bool _isAlive = true;
	protected float _health;
}
