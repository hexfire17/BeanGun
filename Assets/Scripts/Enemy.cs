using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntitiy {

	// Use this for initialization
	public override void Start () {
		base.Start ();
		
		if (GameObject.FindGameObjectWithTag ("Player") != null)
		{
			_pathFinder = GetComponent<NavMeshAgent> ();
			_target = GameObject.FindGameObjectWithTag ("Player").transform;
			_targetEntity = _target.GetComponent<LivingEntitiy> ();
			_targetEntity.onDeath += onTargetDeath;
			_currentState = State.Chasing;
		
			_myCollisionRadius = GetComponent<CapsuleCollider>().radius;
			_targetCollisionRadius = _target.GetComponent<CapsuleCollider>().radius;
				
			_skinMaterial = GetComponent<Renderer>().material;
		
			StartCoroutine (UpdatePath ());
		}
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
		if (squareDistanceToTarget < Mathf.Pow (_attackDistanceThreshold + _myCollisionRadius + _targetCollisionRadius, 2))
		{
			_nextAttackTime = Time.time + _timeBetweenAttacksSec;
			StartCoroutine(Attack());
		}
	}

	public override void takeHit (float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
		if (damage >= _health)
		{
			Debug.Log ("Spawning Death Effect");
			// TODO why are these not being destroyed...?
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
			Vector3 targetPosition = _target.position - directionToTarget * (_myCollisionRadius + _targetCollisionRadius + _attackDistanceThreshold/2);
			_pathFinder.SetDestination (targetPosition);
			yield return new WaitForSeconds(_refreshRateSecs);
		}
	}

	NavMeshAgent _pathFinder;
	Transform _target;
	float _refreshRateSecs = 1;
	
	float _attackDistanceThreshold = 1.5f;
	float _timeBetweenAttacksSec = 1;
	float _nextAttackTime;
	float _attackSpeed = 3;
	
	float _myCollisionRadius;
	float _targetCollisionRadius;
	
	float _damage = 1;

	public ParticleSystem _deathEffect;

	Material _skinMaterial;
	
	LivingEntitiy _targetEntity;
	
	State _currentState;
	public enum State{Idle, Chasing, Attacking}

}
