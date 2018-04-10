using UnityEngine;
using UnityEngine.UI;


public class PlayButtonController : BaseUIController
{

	protected override void Awake() {
		base.Awake();
		EventManager.AddListener(GlobalEvent.Stop, OnStop);
		EventManager.AddListener(GlobalEvent.PlayJustLoaded, OnClick);

		model.playButton = gameObject;
		gameObject.SetActive(model.isPlayButtonEnabled);
	}


	void OnStop() {
		model.isPlayButtonEnabled = true;
		model.pauseButton.SetActive(false);
		model.playButton.SetActive(true);
	}


	public void OnClick() {
		EventManager.FireEvent(GlobalEvent.Play);
		gameObject.SetActive(false);
		model.pauseButton.SetActive(true);
		model.isPlayButtonEnabled = false;
	}
}
