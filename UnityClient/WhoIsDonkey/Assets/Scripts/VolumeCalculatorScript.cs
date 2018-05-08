using System;
using System.IO;
using System.Text;
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

	private readonly string SettingsPath = "settings.stg";

	public void Start()
	{
		_musicSlider = MusicSlider.GetComponent<Slider>();
		_masterSlider = MasterSlider.GetComponent<Slider>();
		_effectSlider = EffectSlider.GetComponent<Slider>();
		Load();
		AdjustMasterVolume();
	}

	public void AdjustMasterVolume()
	{
		MusicSource.volume = _musicSlider.value * _masterSlider.value;
		EffectSource.volume = _effectSlider.value * _masterSlider.value;
		Save();
	}

	public void AdjustMusicVolume()
	{
		MusicSource.volume = _musicSlider.value * _masterSlider.value;
		Save();
	}

	public void AdjustEffectVolume()
	{
		EffectSource.volume = _effectSlider.value * _masterSlider.value;
		Save();
	}

	private void Save()
	{
		var filePath = Path.Combine(Environment.CurrentDirectory, SettingsPath);

		var sb = new StringBuilder();
		sb.AppendLine(_masterSlider.value.ToString());
		sb.AppendLine(_musicSlider.value.ToString());
		sb.AppendLine(_effectSlider.value.ToString());

		File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
	}

	private void Load()
	{
		var filePath = Path.Combine(Environment.CurrentDirectory, SettingsPath);

		if (!File.Exists(filePath))
			return;
		try
		{
			var lines = File.ReadAllLines(filePath, Encoding.UTF8);
			_masterSlider.value = float.Parse(lines[0]);
			_musicSlider.value = float.Parse(lines[1]);
			_effectSlider.value = float.Parse(lines[2]);
		}
		catch { }
	}
}
