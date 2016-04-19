using UnityEngine;
using System.Collections;

public class LivingEntitiy : MonoBehaviour, IDamageable {
	
	public virtual void Start()
	{
		_health = _startingHealth;	
	}
	
	public virtual void takeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
		takeDamage(damage);
	}
	
	public virtual void takeDamage(float damage)
	{
		_health -= damage;
		Debug.Log (this.name + " health: " + _health);
		if (_health <= 0 && _isAlive)
		{
			Die();	
		}	
	}

	[ContextMenu("Kill")] // TODO why doesn't this work...? supposed to be able to right click on script
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
