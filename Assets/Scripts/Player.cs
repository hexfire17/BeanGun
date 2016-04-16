using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : LivingEntitiy
{

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		_controller = GetComponent<PlayerController> ();
		_viewCamera = Camera.main;
		_gunController = GetComponent<GunController> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Movement
		int zAxis = 0;
		Vector3 moveInput = new Vector3 (Input.GetAxis ("Horizontal"), zAxis, Input.GetAxis ("Vertical"));
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
		if (Input.GetMouseButton (0))
		{
			_gunController.Shoot();
		}
	}

	PlayerController _controller;
	Camera _viewCamera;
	GunController _gunController;

	public float _moveSpeed = 5;
	private bool _debug = true;
}
