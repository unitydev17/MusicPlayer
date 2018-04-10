using UnityEngine;
using UnityEngine.UI;


public class BaseUIController : BaseController
{

	Selectable control;


	protected virtual void Awake()
	{
		EventManager.AddListener(GlobalEvent.ActivateUIControls, OnActivateUIControls);
		EventManager.AddListener(GlobalEvent.DisableUIControls, OnDisableUIControls);
		control = gameObject.GetComponent<Selectable>();
		control.interactable = false;
	}


	void OnActivateUIControls()
	{
		control.interactable = true;
	}


	void OnDisableUIControls()
	{
		control.interactable = false;
	}

}
