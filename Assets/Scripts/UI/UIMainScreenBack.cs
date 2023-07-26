using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMainScreenBack : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	public Action swipe;

	private bool _swipeStart;
	private Vector2 _swipeStartPoint;
	private Vector2 _swipeDelta;

	public void OnPointerDown(PointerEventData eventData)
	{
		_swipeStart = true;
		_swipeStartPoint = eventData.position;
		_swipeDelta = Vector2.zero;
	}
	/*public void OnPointerMove(PointerEventData eventData)
	{
		if (_swipeStart)
		{
			if (eventData.delta != Vector2.zero)
			{
				_swipeDelta = eventData.delta;
			}
		}
	}*/
	public void OnDrag(PointerEventData eventData)
	{
		if (_swipeStart)
		{
			if (eventData.delta != Vector2.zero)
			{
				_swipeDelta = eventData.delta;
			}
		}
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		if (_swipeStart)
		{
			if (eventData.delta != Vector2.zero && Mathf.Abs(eventData.position.x - _swipeStartPoint.x) >= 10.0f)
			{
				swipe?.Invoke();
			}
		}

		_swipeStart = false;
	}
}