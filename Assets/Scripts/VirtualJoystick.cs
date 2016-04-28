using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	void Start ()
	{
		FindObjectOfType<Player> ().onDeath += DisableJoystick;

		_startingPosition = _stickImage.transform.position;

		float stickRadius = _stickImage.rectTransform.sizeDelta.x * .5f;
		_radius = _backgroundImage.rectTransform.sizeDelta.x * .5f;
		_radius -= stickRadius;
	}

	private void DisableJoystick ()
	{
		gameObject.SetActive (false);
	}

	public virtual void OnDrag (PointerEventData ped)
	{
		Vector2 positionChange = _startingPosition - ped.position;
		Vector2 diffPercentVector = positionChange / _radius;

		_inputVector = diffPercentVector * -1;
		if (_inputVector.magnitude > 1f)
		{
			_inputVector = _inputVector.normalized;
		}

		_stickImage.transform.position = _startingPosition + (_inputVector * _radius);
	}

	public virtual void OnPointerDown (PointerEventData ped)
	{
		OnDrag (ped);
	}

	public virtual void OnPointerUp (PointerEventData ped)
	{
		_inputVector = Vector3.zero;
		_stickImage.transform.position = _startingPosition;
	}

	public Vector2 getInputVector ()
	{
		return _inputVector;
	}

	public Image _backgroundImage;
	public Image _stickImage;

	private float _radius;
	private Vector2 _startingPosition;
	private Vector2 _inputVector;
}
