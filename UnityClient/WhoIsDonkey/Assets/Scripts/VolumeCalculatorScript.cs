using UnityEngine;
using UnityEngine.UI;

public class VolumeCalculatorScript : MonoBehaviour
{
	public GameObject MasterSlider;
	public GameObject MusicSlider;
	public GameObject EffectSlider;

	public AudioSource MusicSource;
	public AudioSource EffectSource;

	private Slider _musicSlider;
	private Slider _masterSlider;
	private Slider _effectSlider;

	public void Start()
	{
		_musicSlider = MusicSlider.GetComponent<Slider>();
		_masterSlider = MasterSlider.GetComponent<Slider>();
		_effectSlider = EffectSlider.GetComponent<Slider>();
	}

	public void AdjustMasterVolume()
	{
		MusicSource.volume = _musicSlider.value * _masterSlider.value;
		EffectSource.volume = _effectSlider.value * _masterSlider.value;
	}

	public void AdjustMusicVolume()
	{
		MusicSource.volume = _musicSlider.value * _masterSlider.value;
	}

	public void AdjustEffectVolume()
	{
		EffectSource.volume = _effectSlider.value * _masterSlider.value;
	}
}
