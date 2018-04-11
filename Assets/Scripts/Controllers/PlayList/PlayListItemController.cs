using UnityEngine;
using UnityEngine.UI;


public class PlayListItemController : BaseController
{
	Button button;


	void Awake()
	{
		EventManager.AddListener(GlobalEvent.UpdatePlayListItems, UpdateItem);

		button = gameObject.GetComponent<Button>();
	}


	public void OnClick()
	{
		// Disable current button
		button.interactable = false;

		// Enable previous button
		if (model.itemToPlayObj != null) {
			model.itemToPlayObj.GetComponent<Button>().interactable = true;
		}

		// Store current button
		model.SetCurrentItem(gameObject);
		EventManager.FireEvent(GlobalEvent.SelectPlayListFile);
	}


	public void UpdateItem()
	{
		if (model.itemToPlay.id == gameObject.GetInstanceID()) {
			OnClick();
		}
	}
}
