using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		_myRigidBody = GetComponent<Rigidbody> ();
		_gunController = GetComponent<GunController> ();
		_player = GetComponent<Player> ();
		_shootStick.gameObject.SetActive (_isAndroid);
		_isPhoneShaking = false;
		_accel = Input.acceleration.y;
		_nextJumpTime = 0;
		_myRigidBody.transform.forward = _gunController._equippedGun._projectileSpawns [0].transform.forward;
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
		CheckAccel ();
		// jump
		bool isInvoking = (_isAndroid && _isPhoneShaking) || Input.GetKeyDown (KeyCode.Space);
		if (isInvoking && _player.IsGrounded () && Time.time > _nextJumpTime)
		{
			_myRigidBody.velocity.Scale (new Vector3 (1, 0, 1)); // TODO is this even helping...?
			Debug.Log ("Jump " + Time.time + ": " + _myRigidBody.velocity);
			_myRigidBody.AddForce (new Vector3(0,_jumpHeight,0), ForceMode.Impulse);
			_nextJumpTime = Time.time + _delayBetweenJumps;
		}
	}

	private void CheckAccel ()
	{
    	float currentAccel = Input.acceleration.y;
		float accelDiff = Mathf.Abs (_accel - currentAccel);
		_isPhoneShaking = accelDiff >= _accelThreshold;
		_accel = currentAccel;
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

		// air time
		if (!_player.IsGrounded ()) {
			if (_lockObject != null && !_lockObject.position.Equals(Vector3.zero)) {
				Debug.Log ("Locked On: " + _lockObject.position);
				lookPosition = _lockObject.position;
				Debug.DrawLine (_myRigidBody.transform.position, _lockObject.position, Color.blue);
			} else {
				_lockObject = null;
				Transform muzzleTransform = _gunController._equippedGun._projectileSpawns [0].transform;
				Vector3 muzzlePosition = muzzleTransform.position;
				Vector3 muzzleDirection = muzzleTransform.forward;
				Vector3 groundedMuzzlePosition = new Vector3 (muzzlePosition.x, 1.1f, muzzlePosition.z);
				Ray ray = new Ray (groundedMuzzlePosition, muzzleDirection);
				RaycastHit hit = new RaycastHit ();
				bool isHit = Physics.Raycast (ray, out hit, 100f, LayerMask.GetMask ("Enemy"));
				if (isHit) {
					Debug.Log ("P: " + muzzlePosition);
					Debug.Log ("HIT!");
					_lockObject = hit.transform;
					Debug.DrawLine (ray.origin, hit.point, Color.red);
					lookPosition = _lockObject.position;
				}
			}
		} else {
			_lockObject = null;
		}

		transform.LookAt (lookPosition);
	}

	bool _isPhoneShaking;
	float _accel;
	float _accelThreshold = .45f;
	float _delayBetweenJumps = .5f; // fix low double jump bug
	float _nextJumpTime;

	public bool _isAndroid;
	public VirtualJoystick _shootStick;
	Rigidbody _myRigidBody;
	Vector3 _velocity;
	public GunController _gunController;
	Player _player;
	public Button _jumpButton;
	public int _jumpHeight;
	private Transform _lockObject;
}
