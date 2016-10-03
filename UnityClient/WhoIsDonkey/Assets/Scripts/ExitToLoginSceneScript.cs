using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitToLoginSceneScript : MonoBehaviour
{
	public float ActivationDistance = 0.8f;
	public float Timeout = 1.5f;

	private float _activationTime;
	private bool _exiting = false;

	void FixedUpdate()
	{
		var distance = (this.transform.position - Camera.main.transform.position).magnitude;
		if (distance > ActivationDistance)
			_exiting = false;

		if (distance <= ActivationDistance && !_exiting)
		{
			_exiting = true;
			_activationTime = Time.time;
			return;
		}

		if (_exiting && _activationTime + Timeout < Time.time)
			SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
	}
}
