using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : LivingEntitiy
{

	// Use this for initialization
	public override void Start ()
	{
		Debug.Log("Count" + Input.touchCount);
		base.Start ();
		_controller = GetComponent<PlayerController> ();
		_viewCamera = Camera.main;
		_gunController = GetComponent<GunController> ();
		_joystick.gameObject.SetActive (_isAndroid);
		_myRigidBody = GetComponent<Rigidbody> ();
		_startHeight = transform.position.y;
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
			if (_debug) Debug.DrawLine(ray.origin, point, Color.red);
			_controller.LookAt(point);
		}

		// Weapppppppppppon
		if (Input.GetMouseButton (0) && !_isAndroid) {
			_gunController.Shoot ();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		bool bottomCollision = collision.contacts [0].normal.y > 0;
		string name = collision.gameObject.name;
		Debug.Log ("bottom collidde: " + bottomCollision);
		Debug.Log (name + " normal: " + "X: " + collision.contacts [0].normal.x + " " + "Y: " + collision.contacts [0].normal.y + " " + "Z: " + collision.contacts [0].normal.z + " ");
		if (name.Equals ("Map") || (bottomCollision && name.StartsWith ("Obsticle")))
		{
			Debug.Log ("Ontop of " + name);
			_grounded = true;
			_myRigidBody.velocity = Vector3.zero; // stop it from drifting after collision TODO do I need this anymore
		}
	}

	void OnCollisionExit(Collision collision)
	{
		string name = collision.gameObject.name;
		if (name.Equals ("Map") || name.StartsWith ("Obsticle")) {
			if (transform.position.y > collision.gameObject.transform.position.y)
			{
				Debug.Log ("Went up from: " + name);
				_grounded = false;
			}
		}
	//	Debug.Log ("E: " + collision.gameObject.name);
	}
		
	PlayerController _controller;
	Camera _viewCamera;
	GunController _gunController;
	Rigidbody _myRigidBody;
	float _startHeight;
	public bool _grounded;

	public VirtualJoystick _joystick;
	public bool _isAndroid;
	public float _moveSpeed = 5;
	private bool _debug = true;
}
