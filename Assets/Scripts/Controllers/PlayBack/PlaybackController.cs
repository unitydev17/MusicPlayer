using System.Collections;
using System.IO;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;


public class PlaybackController : BaseController
{
	enum State
	{
		Idle,
		Play,
		Pause,
		TimeSliderDrag,
		TimeSliderDragPaused,
	}


	public Slider timeSlider;
	public AudioSource audioSource;
	public Text timeText;
	public Text infoText;


	float duration;
	float timeSliderValue;
	Coroutine playCoroutine;
	State state;
	volatile bool clipLoadInProgress;
	bool disableNextClip;	// used to prevent next clip loading while loading already is in progress
	AudioModel audioModel;


	#region subscribe for the events

	void Awake()
	{
		EventManager.AddListener(GlobalEvent.SelectPlayListFile, PrepareClip);
		EventManager.AddListener(GlobalEvent.PlaybackDrag, OnSliderDrag);
		EventManager.AddListener(GlobalEvent.PlaybackDragEnd, OnSliderDragEnd);
		EventManager.AddListener(GlobalEvent.Play, OnPlay);
		EventManager.AddListener(GlobalEvent.Pause, OnPause);
		EventManager.AddListener(GlobalEvent.Stop, OnStop);

		timeSlider.onValueChanged.AddListener((float value) => {
			timeSliderValue = value;
		});
	}

	#endregion


	void Start()
	{
		ResetTimeSlider();
	}


	void ResetTimeSlider()
	{
		timeSlider.value = 0;
	}


	void PrepareClip()
	{
		EventManager.FireEvent(GlobalEvent.DisableUIControls);
		clipLoadInProgress = true;
		disableNextClip = true;
		Thread t1 = new Thread(LoadClip);
		t1.Start();
		StartCoroutine(WaitClipLoading());
	}


	IEnumerator WaitClipLoading()
	{
		while (clipLoadInProgress) {
			yield return null;
		}

		if (model.audioModel != null) {

			//stop playback
			//set audio clip after load
			audioSource.Stop();
			DestroyImmediate(audioSource.clip);
			audioSource.clip = model.AudioModelToClip();

			// reset values
			duration = audioSource.clip.length;
			audioSource.time = 0;
			timeSliderValue = 0;
			state = State.Idle;
			infoText.text = Util.GetFileDescription(model.itemToPlay.path);

		}
		EventManager.FireEvent(GlobalEvent.ActivateUIControls);
		EventManager.FireEvent(GlobalEvent.PlayJustLoaded);
	}


	// Called in separate thread. Must not contain unity object invokation/creation
	void LoadClip()
	{
		var path = model.itemToPlay.path;

		if (File.Exists(path)) {
			byte[] data = File.ReadAllBytes(path);

			if (model.audioModel != null) {
				model.audioModel.data = null;
			}

			model.audioModel = Util.isMP3(path) ? NAudioConverter.FromMp3DataModel(ref data) : OpenWavParser.ByteArrayToAudioClipModel(data);
		} else {
			throw new UnityException("File not exists: " + path);
		}
		clipLoadInProgress = false;
	}


	public void OnSliderDrag()
	{
		if (State.Play == state) {
			audioSource.Pause();
			StopCoroutine(playCoroutine);
			state = State.TimeSliderDrag;
		}

		if (State.Pause == state) {
			state = State.TimeSliderDragPaused;
		}

		if (State.TimeSliderDrag == state || State.TimeSliderDragPaused == state) {
			UpdateDragTimeInfo();
		}
	}


	public void OnSliderDragEnd()
	{
		if (State.TimeSliderDrag == state) {
			OnPlay();
		}

		if (State.TimeSliderDragPaused == state) {
			state = State.Pause;
		}
	}


	public void OnPlay()
	{
		disableNextClip = false;

		state = State.Play;
		audioSource.Play();
		audioSource.time = timeSliderValue * duration;

		if (playCoroutine != null) {
			StopCoroutine(playCoroutine);
		}
		playCoroutine = StartCoroutine(TimeControl());
	}


	public void OnPause()
	{
		state = State.Pause;
		audioSource.Pause();
		StopCoroutine(playCoroutine);
	}


	public void OnStop()
	{
		audioSource.Stop();
		state = State.Idle;
		ResetTimeSlider();
		StopCoroutine(playCoroutine);
	}


	IEnumerator TimeControl()
	{
		while (audioSource.isPlaying) {
			timeSlider.value = audioSource.time / duration;
			UpdateTimeInfo();
			yield return null;
		}

		state = State.Idle;
		ResetTimeSlider();

		if (!disableNextClip) {
			EventManager.FireEvent(GlobalEvent.Next);
		}
	}


	void UpdateTimeInfo()
	{
		
		timeText.text = Util.GetTimeRange(audioSource.time, duration);
	}


	void UpdateDragTimeInfo()
	{
		timeText.text = Util.GetTimeRange(timeSliderValue * duration, duration);
	}

}
