using System.Collections.Generic;
using UnityEngine;


public class PlayerModel
{
	
	public bool isMp3Enable;
	public LinkedList<PlayListItemModel> playListItems;
	public PlayListItemModel itemToPlay;
	public GameObject itemToPlayObj;
	public AudioModel audioModel;
	public float playbackTime;
	public bool isSoundOff;

	public bool isPlayButtonEnabled;
	public GameObject playButton;
	public GameObject pauseButton;

	public bool isSoundOnButtonEnabled;
	public GameObject soundOnButton;
	public GameObject soundOffButton;


	public PlayerModel()
	{
		isMp3Enable = true;
		playListItems = new LinkedList<PlayListItemModel>();
		isPlayButtonEnabled = true;
		isSoundOnButtonEnabled = true;
		isSoundOff = false;
	}


	public string GetExtension()
	{
		return isMp3Enable ? Util.WAV_MP3 : Util.WAV;
	}


	public void AddPlayListItem(PlayListItemModel item)
	{
		lock (playListItems) {
			playListItems.AddLast(item);
		}
	}


	public void RemovePlayListItems()
	{
		lock (playListItems) {
			playListItems.Clear();
		}
	}


	public void SetItemToPlay(int id)
	{
		foreach (PlayListItemModel item in playListItems) {
			if (id == item.id) {
				itemToPlay = item;
			}
		}
	}


	public AudioClip AudioModelToClip()
	{
		AudioClip audioClip = AudioClip.Create(audioModel.name, audioModel.lengthSamples, audioModel.channel, audioModel.frequency, audioModel.stream);
		audioClip.SetData(audioModel.data, 0);
		return audioClip;
	}
}
