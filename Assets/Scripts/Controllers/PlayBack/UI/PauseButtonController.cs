using UnityEngine;
using UnityEngine.UI;


public class PauseButtonController : BaseUIController
{

	protected override void Awake() {
		base.Awake();
		model.pauseButton = gameObject;
		gameObject.SetActive(!model.isPlayButtonEnabled);
	}

	public void OnClick() {
		EventManager.FireEvent(GlobalEvent.Pause);
		gameObject.SetActive(false);
		model.playButton.SetActive(true);
		model.isPlayButtonEnabled = true;
	}
}
