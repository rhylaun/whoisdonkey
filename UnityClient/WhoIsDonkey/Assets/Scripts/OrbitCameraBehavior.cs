using UnityEngine;
using System;

public class OrbitCameraBehavior : MonoBehaviour
{
	public bool Enabled = true;
	public float OrbitRadius = 20.0f;
	public float AngleSpeed = 20.0f;
	public float GoToOrbitSpeed = 10.0f;
	public GameObject Target;

	private Vector3 _axisVector = new Vector3(0.1f, 0.8f, 0.2f);
	private float _orbitSpeed;

	void Start()
	{
		_orbitSpeed = GoToOrbitSpeed;
	}

	void LateUpdate()
	{
		if (!Enabled) return;

		var subVector = Target.transform.position - this.transform.position;
		int direction = Math.Sign(subVector.magnitude - OrbitRadius);

		if (_orbitSpeed > 0.01f)
		{
			if (subVector.magnitude < OrbitRadius)
				_orbitSpeed = _orbitSpeed / 2;

			subVector.Normalize();
			this.transform.position += direction * subVector * _orbitSpeed * Time.deltaTime;
		}

		this.transform.RotateAround(Target.transform.position, _axisVector, AngleSpeed * Time.deltaTime);
		this.transform.LookAt(Target.transform);
	}
}
