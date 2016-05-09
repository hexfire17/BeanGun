using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntitiy {

	void Awake ()
	{
		if (GameObject.FindGameObjectWithTag ("Player") != null)
		{
			_pathFinder = GetComponent<NavMeshAgent> ();
			_target = GameObject.FindGameObjectWithTag ("Player").transform;
			_targetEntity = _target.GetComponent<LivingEntitiy> ();

			_myCollisionRadius = GetComponent<CapsuleCollider>().radius;
			_targetCollisionRadius = _target.GetComponent<CapsuleCollider>().radius;

			_skinMaterial = GetComponent<Renderer>().material;
		}
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();
		
		if (_target != null)
		{
			_targetEntity.onDeath += onTargetDeath;
			_currentState = State.Chasing;
			StartCoroutine (UpdatePath ());
		}
	}

	public void setCharacteristics (float speed, int damage, float attackDistance, int health, Color color)
	{
		_pathFinder.speed = speed;
		_startingHealth = health;
		_damage = damage;
		_attackDistance = attackDistance;
		GetComponent <Renderer> ().material.color = color;
	}

	void onTargetDeath()
	{
		_currentState = State.Idle;	
	}
	
	void Update ()
	{
		if (_target == null || Time.time < _nextAttackTime)
		{
			return;	
		}
		
		float squareDistanceToTarget = (_target.position - transform.position).sqrMagnitude;
		if (squareDistanceToTarget < Mathf.Pow (_attackDistance + _myCollisionRadius + _targetCollisionRadius, 2))
		{
			_nextAttackTime = Time.time + _timeBetweenAttacksSec;
			StartCoroutine(Attack());
		}
	}

	public override void takeHit (float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
		Instantiate (_bloodEffect, hitPoint, Quaternion.FromToRotation (Vector3.forward, hitDirection));
		if (damage >= _health)
		{
			Debug.Log ("Spawning Death Effect");
			_deathEffect.GetComponent<Renderer> ().material = _skinMaterial;
			Instantiate (_deathEffect, hitPoint, Quaternion.FromToRotation (Vector3.forward, hitDirection));
		}
		base.takeHit (damage, hitPoint, hitDirection);
	}
	
	IEnumerator Attack()
	{
		_currentState = State.Attacking;
		Color originalColor = _skinMaterial.color;
		_skinMaterial.color = Color.red;
		Vector3 originalPosition = transform.position;
		Vector3 directionToTarget = (_target.position - transform.position).normalized;
		Vector3 targetPosition = _target.position - directionToTarget * (_myCollisionRadius);
		
		bool appliedDamage = false;
		float percent = 0;
		while (percent <= 1)
		{
			if (!appliedDamage && percent >= .5)
			{
				appliedDamage = true;
				_targetEntity.takeDamage(_damage);
			}
			
			percent += Time.deltaTime * _attackSpeed;
			float interpalation = (-Mathf.Pow(percent,2) + percent) * 4; // -4x^2 + 4x parabola
			transform.position = Vector3.Lerp(originalPosition, targetPosition, interpalation);
			
			yield return null;
		}
		
		_skinMaterial.color = originalColor;
		_currentState = State.Chasing;
	}

	IEnumerator UpdatePath()
	{
		if (_target == null || _currentState != State.Chasing)
		{
			yield break;
		}
		
		while (_target != null && _isAlive)
		{
			Vector3 directionToTarget = (_target.position - transform.position).normalized;
			Vector3 targetPosition = _target.position - directionToTarget * (_myCollisionRadius + _targetCollisionRadius + _attackDistance/2);
			_pathFinder.SetDestination (targetPosition);
			yield return new WaitForSeconds(_refreshRateSecs);
		}
	}

	NavMeshAgent _pathFinder;
	Transform _target;
	float _refreshRateSecs = 1;
	
	float _attackDistance = 1f;
	float _timeBetweenAttacksSec = 1;
	float _nextAttackTime;
	float _attackSpeed = 3;
	
	float _myCollisionRadius;
	float _targetCollisionRadius;
	
	float _damage = 1;

	public ParticleSystem _deathEffect;
	public ParticleSystem _bloodEffect;

	Material _skinMaterial;
	
	LivingEntitiy _targetEntity;
	
	State _currentState;
	public enum State{Idle, Chasing, Attacking}

}
