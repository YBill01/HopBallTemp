using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInputComponent : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	//public event Action<int> swipeEvent;


	private bool _swipeStart;
	//private Vector2 _swipePrevPosition;
	//private Vector2 _swipePosition;
	private Vector2 _swipeDelta;

	

	public void OnPointerDown(PointerEventData eventData)
	{
		_swipeStart = true;

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
			if (_swipeDelta != Vector2.zero)
			{
				GameEvents.inputSwipe?.Invoke(_swipeDelta.x < 0 ? -1 : 1);

				//Debug.Log(_swipeDelta.x < 0 ? "left" : "right");
			}
		}

		_swipeStart = false;
	}

	
}