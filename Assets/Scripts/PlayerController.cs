using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		_myRigidBody = GetComponent<Rigidbody> ();
		_startHeight = transform.position.y;
		_gunController = GetComponent<GunController> ();
		_player = GetComponent<Player> ();
		_shootStick.gameObject.SetActive (_isAndroid);
		_jumpButton.gameObject.SetActive (_isAndroid);
		_jumpButton.onClick.AddListener (JumpPressed);
		_jumpPressed = false;
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
		bool isInvoking = (_isAndroid && _jumpPressed) || Input.GetKeyDown (KeyCode.Space);
		if (isInvoking && _player.IsGrounded ())
		{
			Debug.Log ("Jump " + Time.time + ": " + _myRigidBody.velocity);
			_myRigidBody.AddForce (new Vector3(0,7,0), ForceMode.Impulse);
		}
		_jumpPressed = false;
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

	private void JumpPressed ()
	{
		_jumpPressed = true;
	}

	public bool _isAndroid;
	public VirtualJoystick _shootStick;
	Rigidbody _myRigidBody;
	Vector3 _velocity;
	float _startHeight;
	public GunController _gunController;
	Player _player;
	public Button _jumpButton;
	private bool _jumpPressed;
}
