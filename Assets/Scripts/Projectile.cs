using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public void Start()
	{
		GameObject.Destroy(gameObject, _ttlSecs);	
		Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, _collisionMask);
		if (initialCollisions.Length > 0)
		{
			OnHitObject(initialCollisions[0], transform.position);
		}
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
			OnHitObject(hit.collider, hit.point);
		}
	}
	
	void OnHitObject(Collider c, Vector3 hitPoint)
	{
		IDamageable damageableObject = c.GetComponent<IDamageable> ();
		if (damageableObject != null)
		{
			damageableObject.takeHit(_damage, hitPoint, transform.forward);
		}
		GameObject.Destroy(gameObject);
	}

	public LayerMask _collisionMask;

	public float _speed { get; set; }
	public float _damage { get; set; }
	public float _ttlSecs = 3;
	float _skinWidth = .1f;
}
