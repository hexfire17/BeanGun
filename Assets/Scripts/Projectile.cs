using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public void Start()
	{
		GameObject.Destroy(gameObject, _ttlSecs);	
		Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, _collisionMask);
		if (initialCollisions.Length > 0)
		{
			onHitObject(initialCollisions[0], transform.position);
		}
	}
	
	// Use this for initialization
	public void SetSpeed (float speed)
	{
		_speed = speed;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float moveDistance = Time.deltaTime * _speed;
		CheckCollision (moveDistance);
		transform.Translate (Vector3.forward * moveDistance);
	}

	void CheckCollision(float moveDistance)
	{
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, moveDistance + _skinWidth, _collisionMask, QueryTriggerInteraction.Collide))
		{
			onHitObject(hit.collider, hit.point);
		}
	}
	
	void onHitObject(Collider c, Vector3 hitPoint)
	{
		IDamageable damageableObject = c.GetComponent<IDamageable> ();
		if (damageableObject != null)
		{
			damageableObject.takeHit(_damage, hitPoint, transform.forward);
			GameObject.Destroy(gameObject);
		}
		Debug.Log("Hit: " + c.gameObject.name);
	}

	public LayerMask _collisionMask;
	float _speed = 10;
	float _damage = 1;
	float _ttlSecs = 3;
	float _skinWidth = .1f;
}
