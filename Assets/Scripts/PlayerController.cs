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
		_player = GetComponent<Player> ();
	}

	public void Move (Vector3 velocity)
	{
		_velocity = velocity;
	}

	public void FixedUpdate ()
	{
		Vector3 move = (_myRigidBody.position + _velocity * Time.fixedDeltaTime);
		_myRigidBody.MovePosition (move);
	}

	public void Update ()
	{
		// jump
		if (Input.GetKeyDown (KeyCode.Space) && _player._grounded) {
			Debug.Log ("Jump " + Time.time + ": " + _myRigidBody.velocity);
			_myRigidBody.AddForce (new Vector3(0,7,0), ForceMode.Impulse);
		}
	}

	public void LookAt(Vector3 point)
	{
		Vector3 lookPosition;
		lookPosition = new Vector3 (point.x, transform.position.y, point.z);
		if (_isAndroid)
		{
			Vector2 shootInput = _shootStick.getInputVector ();
			Vector3 playerPosition = transform.position; // maybe dont look if dont need to
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
	Player _player;
}
