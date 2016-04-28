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
	}
	
	// Update is called once per frame
	void Update ()
	{
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
		if (Input.GetMouseButton (0)) {
			_gunController.Shoot ();
		}
	}
		
	PlayerController _controller;
	Camera _viewCamera;
	GunController _gunController;

	public VirtualJoystick _joystick;
	public bool _isAndroid;
	public float _moveSpeed = 5;
	private bool _debug = true;
}
