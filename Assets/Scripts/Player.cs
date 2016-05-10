using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : LivingEntitiy
{

	// Use this for initialization
	public void Start ()
	{
		Debug.Log("Count" + Input.touchCount);
		_controller = GetComponent<PlayerController> ();
		_viewCamera = Camera.main;
		_gunController = GetComponent<GunController> ();
		_joystick.gameObject.SetActive (_isAndroid);
		_myRigidBody = GetComponent<Rigidbody> ();
		_grounded = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// stop drift!
		Vector3 currVelocity = _myRigidBody.velocity;
		Vector3 correctedVelocity = new Vector3(0, currVelocity.y, 0);
		_myRigidBody.velocity = correctedVelocity;

		// Movement
		float horizMove = 0;
		float virtMove = 0;
		if (_isAndroid)
		{
			Vector2 joystickVector = _joystick.getInputVector ();
			horizMove = joystickVector.x;
			virtMove = joystickVector.y;
		} 
		else 
		{
			horizMove = Input.GetAxis ("Horizontal");
			virtMove = Input.GetAxis ("Vertical");
		}

		Vector3 moveInput = new Vector3 (horizMove, 0, virtMove);
		Vector3 moveVelocity = moveInput.normalized * _moveSpeed;
		_controller.Move (moveVelocity);

		// Look
		Ray ray = _viewCamera.ScreenPointToRay (Input.mousePosition);
		Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
		float rayDistance;

		if (groundPlane.Raycast (ray, out rayDistance))
		{
			Vector3 point = ray.GetPoint(rayDistance);
			Debug.DrawLine(ray.origin, point, Color.red);
			_controller.LookAt(point);
		}

		// Weapppppppppppon
		if (Input.GetMouseButton (0) && !_isAndroid) {
			_gunController.Shoot ();
		}
	}

	public bool IsGrounded ()
	{
		Ray bottom = new Ray (this.transform.position, Vector3.down);
		RaycastHit hit = new RaycastHit();
		bool hitground = Physics.Raycast (bottom, out hit, 1.5f);

		return hitground;
	}
		
	PlayerController _controller;
	Camera _viewCamera;
	GunController _gunController;
	Rigidbody _myRigidBody;
	public bool _grounded;

	public VirtualJoystick _joystick;
	public bool _isAndroid;
	public float _moveSpeed = 5;
}
