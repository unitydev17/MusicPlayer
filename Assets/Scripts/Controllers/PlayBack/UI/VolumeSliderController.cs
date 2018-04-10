using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class VolumeSliderController : BaseController
{

	public AudioSource audioSource;

	private Slider slider;


	void Awake()
	{
		slider = GetComponent<Slider>();
		audioSource.volume = 0.5f;
		slider.value = 0.5f;

		GetComponent<Slider>().onValueChanged.AddListener((float value) => {
			audioSource.volume = slider.value;
			CheckVolume();
		});
			
		EventManager.AddListener(GlobalEvent.SoundOff, OnSoundOff);
	}


	void CheckVolume()
	{
		if (!model.isSoundOff && audioSource.volume == 0) {
			model.isSoundOff = true;
			EventManager.FireEvent(GlobalEvent.SoundOffFromSlider);

		} else if (model.isSoundOff) {
			model.isSoundOff = false;
			EventManager.FireEvent(GlobalEvent.SoundOn);
		}
	}


	void OnSoundOff()
	{
		slider.value = 0;
	}

}
