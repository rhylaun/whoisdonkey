using UnityEngine;
using System.Collections;

public class FlikerLightBehavior : MonoBehaviour
{
	public float MaxIntensity = 0.5f;
	public float StartIntensity = 0.0f;
	public float Speed = 0.1f;
	public float Pause = 0.0f;

	private Light _light;
	private int _direction = 1;
	private bool _pauseMod = false;
	private float _pauseTime = 0.0f;

	void Start()
	{
		_light = this.GetComponent<Light>();
		if (_light == null) return;

		_light.intensity = StartIntensity;
	}

	void Update()
	{
		if (_light == null) return;

		if (Pause > float.Epsilon && _pauseMod)
		{
			_pauseTime += Time.deltaTime;
			if (_pauseTime > Pause)
				_pauseMod = false;
			return;
		}

		if (CheckDirection())
		{
			_light.intensity = 0.0f;
			_pauseMod = true;
			_pauseTime = 0.0f;
		}

		_light.intensity += Speed * _direction;
	}

	private bool CheckDirection()
	{
		if (_light == null) return false;

		if (_light.intensity < float.Epsilon)
		{
			_direction = 1;
			return true;
		}

		if (_light.intensity > MaxIntensity) _direction = -1;
		return false;
		
	}
}
