using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		_myRigidBody = GetComponent<Rigidbody> ();
		_startHeight = transform.position.y;
		_gunController = GetComponent<GunController> ();
	}

	public void Move (Vector3 velocity)
	{
		_velocity = velocity;
	}

	public void FixedUpdate ()
	{
		_myRigidBody.velocity = Vector3.zero; // stop it from drifting after collision
		transform.position = new Vector3(transform.position.x, _startHeight, transform.position.z); // stop from floating over shit
		_myRigidBody.MovePosition (_myRigidBody.position + _velocity * Time.fixedDeltaTime);
	}

	public void LookAt(Vector3 point)
	{
		Vector3 lookPosition;
		lookPosition = new Vector3 (point.x, transform.position.y, point.z);
		if (_isAndroid)
		{
			Vector2 shootInput = _shootStick.getInputVector ();
			Vector3 playerPosition = transform.position;
			lookPosition = new Vector3 (playerPosition.x + shootInput.x, playerPosition.y, playerPosition.z + shootInput.y);
			if (!lookPosition.Equals (playerPosition)) {
				_gunController.Shoot ();
			}
		}
		transform.LookAt (lookPosition);
	}

	public bool _isAndroid;
	public VirtualJoystick _shootStick;
	Rigidbody _myRigidBody;
	Vector3 _velocity;
	float _startHeight;
	public GunController _gunController;
}
