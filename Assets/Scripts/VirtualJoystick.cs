using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	void Start ()
	{
		Debug.Log ("bg image: " + _backgroundImage.name);
		Debug.Log ("stick image: " + _stickImage.name);
	}

	public virtual void OnDrag (PointerEventData ped)
	{
		Debug.Log ("Pointer Drag");

		Vector2 position;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_backgroundImage.rectTransform, ped.position, ped.pressEventCamera, out position))
		{
			position.x = (position.x / _backgroundImage.rectTransform.sizeDelta.x);
			position.y = (position.x / _backgroundImage.rectTransform.sizeDelta.y);

			_inputVector = new Vector3 (position.x * 2 + 1, 0, position.y * 2 - 1);
			if (_inputVector.magnitude > 1f)
			{
				_inputVector = _inputVector.normalized;
			}

			_stickImage.rectTransform.anchorMax = new Vector3 (_inputVector.x * (_backgroundImage.rectTransform.sizeDelta.x / 2),
				_backgroundImage.rectTransform.sizeDelta.y / 2);
			Debug.Log(position);
		}
	}

	public virtual void OnPointerDown (PointerEventData ped)
	{
		Debug.Log ("Pointer Down");
		OnDrag (ped);
	}

	public virtual void OnPointerUp (PointerEventData ped)
	{
		Debug.Log ("Pointer Up");
		_inputVector = Vector3.zero;
		_stickImage.rectTransform.anchoredPosition = Vector3.zero;
	}

	public Vector2 getInputVector ()
	{
		return _inputVector;
	}

	public Image _backgroundImage;
	public Image _stickImage;

	private Vector3 _inputVector;
}
