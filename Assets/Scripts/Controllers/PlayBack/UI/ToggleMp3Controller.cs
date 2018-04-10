using UnityEngine;
using UnityEngine.UI;


public class ToggleMp3Controller : BaseController
{

	void Awake() {
		GetComponent<Toggle>().isOn = model.isMp3Enable;
	}

	public void OnClick() {
		model.isMp3Enable = !model.isMp3Enable;
		EventManager.FireEvent(GlobalEvent.ReloadPlayList);
	}

}
