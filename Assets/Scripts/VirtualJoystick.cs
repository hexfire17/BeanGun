using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	void Start ()
	{
		_backgroundImage = GetComponent<Image> ();
		_stickImage = transform.GetChild (0).GetComponent<Image> ();
	}

	public virtual void OnDrag (PointerEventData ped)
	{
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
		OnDrag (ped);
	}

	public virtual void OnPointerUp (PointerEventData ped)
	{
		_inputVector = Vector3.zero;
		_stickImage.rectTransform.anchoredPosition = Vector3.zero;
	}

	public Vector2 getInputVector ()
	{
		return _inputVector;
	}

	private Image _backgroundImage;
	private Image _stickImage;

	private Vector3 _inputVector;
}
