using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		_myRigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Move (Vector3 velocity)
	{
		_velocity = velocity;
	}

	public void FixedUpdate ()
	{
		_myRigidBody.MovePosition (_myRigidBody.position + _velocity * Time.fixedDeltaTime);
	}

	public void LookAt(Vector3 point)
	{
		Vector3 heightCorrectedPoint = new Vector3 (point.x, transform.position.y, point.z);
		transform.LookAt (heightCorrectedPoint);
	}

	Rigidbody _myRigidBody;
	Vector3 _velocity;
}
