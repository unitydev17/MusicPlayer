using UnityEngine;
using UnityEngine.UI;


public class StopButtonController : BaseUIController
{
	public void OnClick() {
		EventManager.FireEvent(GlobalEvent.Stop);
	}
}
