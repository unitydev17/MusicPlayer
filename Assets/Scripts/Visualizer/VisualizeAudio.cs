using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VisualizeAudio : MonoBehaviour
{

	const int QTY = 1024;

	public AudioSource source;
	public float adj1 = 1f;
	public float adj2 = 110f;

	float[] samples = new float[QTY];
	int channel;
	Image[] bars;
	bool isPlaying;


	void Awake()
	{
		EventManager.AddListener(GlobalEvent.Pause, () => {
			isPlaying = false;
		});

		EventManager.AddListener(GlobalEvent.Play, () => {
			isPlaying = true;
		});

		EventManager.AddListener(GlobalEvent.Stop, () => {
			isPlaying = false;
			ResetBars();
		});
	}



	void Start()
	{
		bars = transform.GetComponentsInChildren<Image>();
		channel = 0;
		isPlaying = false;
	}


	void Update()
	{
		if (isPlaying) {
			source.GetSpectrumData(samples, channel, FFTWindow.BlackmanHarris);

			int delta = QTY / bars.Length;
			float sum = 0;
			int idxSample = 0;
			int idx = 0;
			for (int i = 0; i < samples.Length; i++) {
				if (idx++ < (delta - 1)) {
					sum += samples[i] * samples[i];
				} else {
					float rms = Mathf.Sqrt(sum / (float)delta);
					float dbvalue = 20f * Mathf.Log10(rms / 0.1f);
					if (dbvalue < -160) {
						dbvalue = -160;
					}
					bars[idxSample++].fillAmount = adj1 + dbvalue / adj2;
					sum = 0;
					idx = 0;
				}
			}
		}
	}


	void ResetBars()
	{
		foreach (Image bar in bars) {
			bar.fillAmount = 0;
		}
	}
}
