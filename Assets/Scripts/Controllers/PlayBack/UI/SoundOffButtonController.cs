using UnityEngine;
using UnityEngine.UI;


public class SoundOffButtonController : BaseController
{

	void Awake()
	{
		EventManager.AddListener(GlobalEvent.SoundOn, OnSoundOn);

		model.soundOffButton = gameObject;
		gameObject.SetActive(!model.isSoundOnButtonEnabled);
	}


	public void OnSoundOn()
	{
		model.isSoundOnButtonEnabled = true;
		model.soundOnButton.SetActive(true);
		model.soundOffButton.SetActive(false);
	}
		
}
