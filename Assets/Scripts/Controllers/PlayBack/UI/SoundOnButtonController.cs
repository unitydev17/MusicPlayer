using UnityEngine;
using UnityEngine.UI;


public class SoundOnButtonController : BaseController
{

	void Awake()
	{
		EventManager.AddListener(GlobalEvent.SoundOffFromSlider, TurnOffButton);

		model.soundOnButton = gameObject;
		gameObject.SetActive(model.isSoundOnButtonEnabled);
	}


	public void OnSoundOff()
	{
		TurnOffButton();
		EventManager.FireEvent(GlobalEvent.SoundOff);
	}


	public void TurnOffButton()
	{
		model.isSoundOnButtonEnabled = false;
		model.soundOnButton.SetActive(false);
		model.soundOffButton.SetActive(true);
	}
}
