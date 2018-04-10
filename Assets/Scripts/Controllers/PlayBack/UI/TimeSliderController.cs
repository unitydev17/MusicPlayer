using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimeSliderController : BaseUIController, IDragHandler, IEndDragHandler {

	//Slider slider;

	protected override void Awake() {
		base.Awake();
		//slider = GetComponent<Slider>();
	}

	public void OnDrag(PointerEventData eventData) {
		EventManager.FireEvent(GlobalEvent.PlaybackDrag);
	}

	public void OnEndDrag(PointerEventData eventData) {
		EventManager.FireEvent(GlobalEvent.PlaybackDragEnd);
	}

	public void OnPointerClick(PointerEventData eventData) {
		//Debug.Log(slider.value);
		EventManager.FireEvent(GlobalEvent.PlaybackSliderClick);
	}
}
