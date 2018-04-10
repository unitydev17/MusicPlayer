using UnityEngine;
using UnityEngine.UI;


public class PlayListItemController : BaseController
{
	Button button;

	void Awake() {
		EventManager.AddListener(GlobalEvent.UpdatePlayListItems, UpdateItem);

		button = gameObject.GetComponent<Button>();
	}

	public void OnClick() {
		button.interactable = false;

		if (model.itemToPlayObj != null) {
			model.itemToPlayObj.GetComponent<Button>().interactable = true;
		}

		model.SetItemToPlay(transform.gameObject.GetInstanceID());
		model.itemToPlayObj = gameObject;
		EventManager.FireEvent(GlobalEvent.SelectPlayListFile);
	}

	public void UpdateItem() {
		if (model.itemToPlay.id == gameObject.GetInstanceID()) {
			OnClick();
		}
	}
}
