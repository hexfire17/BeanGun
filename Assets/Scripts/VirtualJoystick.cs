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

		Vector3 stickRadius = _stickImage.rectTransform.sizeDelta * .5f;
		_radius = (_backgroundImage.rectTransform.sizeDelta * .5f);
		_radius -= stickRadius;
	}

	private void DisableJoystick ()
	{
		gameObject.SetActive (false);
	}

	public virtual void OnDrag (PointerEventData ped)
	{
		Vector2 position;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_backgroundImage.rectTransform, ped.position, ped.pressEventCamera, out position))
		{
			position.x = (position.x / _backgroundImage.rectTransform.sizeDelta.x);
			position.y = (position.y / _backgroundImage.rectTransform.sizeDelta.y);

			_inputVector = new Vector3 (position.x * 2 + 1, position.y * 2 - 1, 0);
			if (_inputVector.magnitude > 1f)
			{
				_inputVector = _inputVector.normalized;
			}

			_stickImage.transform.position = _startingPosition + Vector3.Scale (_inputVector, _radius);
		}
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

	private Vector3 _startingPosition;
	private Vector3 _radius;
	private Vector3 _inputVector;
}
