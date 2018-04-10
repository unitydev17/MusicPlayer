using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using System.Linq;


public class PlayListController : BaseController
{

	public GameObject playListContainer;
	public GameObject playListItemPrefab;
	public GameObject blockingPanel;

	private string[] filenames;
	private string path;


	void Awake()
	{
		EventManager.AddListener(GlobalEvent.ReloadPlayList, ReloadPlayList);
		EventManager.AddListener(GlobalEvent.Next, PlayNext);
		EventManager.AddListener(GlobalEvent.DisableUIControls, OnStartWaiting);
		EventManager.AddListener(GlobalEvent.ActivateUIControls, OnStopWaiting);
		SetPath();
	}


	void SetPath()
	{
		DirectoryInfo info = new DirectoryInfo(Application.dataPath);
		path = info.Parent.FullName + "\\Tracks";
	}


	void Start()
	{
		Validate();
		ReloadPlayList();
	}


	void Validate()
	{
		if (!playListContainer) {
			throw new UnityException("Playlist container not defined");
		}

		if (!playListItemPrefab) {
			throw new UnityException("Playlist item prefab not defined");
		}
	}


	void LoadPlayList()
	{
		try {
			filenames = GetFileNames(path, model.GetExtension());
		} catch (SystemException e) {
			throw new UnityException(e.Message);
		}
		foreach (string filename in filenames) {
			AddPlayListItem(filename);
		}
	}


	void ReloadPlayList()
	{
		RemoveItems();
		LoadPlayList();
	}


	void RemoveItems()
	{
		EventManager.RemoveListeners(GlobalEvent.UpdatePlayListItems);

		Transform[] childs = playListContainer.transform.GetComponentsInChildren<Transform>();
		foreach (Transform child in childs) {
			if (!child.Equals(playListContainer.transform)) {
				Destroy(child.gameObject);
			}
		}

		model.RemovePlayListItems();
	}


	private string[] GetFileNames(string path, string filter)
	{
		string[] entries = filter.Split('|');
		return entries.SelectMany(entry => Directory.GetFiles(path, entry)).ToArray();
	}


	public void AddPlayListItem(string filepath)
	{
		GameObject item = Instantiate(playListItemPrefab);
		item.GetComponentInChildren<Text>().text = Path.GetFileName(filepath);
		item.transform.SetParent(playListContainer.transform);
		item.transform.localScale = Vector3.one;

		PlayListItemModel itemModel = new PlayListItemModel(item.GetInstanceID(), filepath);
		model.AddPlayListItem(itemModel);
	}


	public void PlayNext()
	{
		bool isCurrentTrackFound = false;

		lock (model.playListItems) {
			foreach (PlayListItemModel item in model.playListItems) {
				if (item.id == model.itemToPlay.id) {
					isCurrentTrackFound = true;
					continue;
				}
			
				if (isCurrentTrackFound) {
					model.itemToPlayObj.GetComponent<Button>().interactable = true;
					model.itemToPlay = item;
					EventManager.FireEvent(GlobalEvent.UpdatePlayListItems);
					break;
				}
			}
		}
		EventManager.FireEvent(GlobalEvent.Stop);
	}


	void OnStartWaiting()
	{
		blockingPanel.SetActive(true);
	}


	void OnStopWaiting()
	{
		blockingPanel.SetActive(false);
	}
}
