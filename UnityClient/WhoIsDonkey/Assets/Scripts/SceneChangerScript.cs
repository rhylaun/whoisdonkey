using Donkey.Client;
using Donkey.Common;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerScript : MonoBehaviour
{
	private Coroutine _coroutine;

	public string SceneToActivate;
	public PlayerState ActivationPlayerState;

	void Start()
	{
		if (string.IsNullOrEmpty(SceneToActivate))
			throw new ArgumentException("Argument SceneToActivate is not set");

		_coroutine = StartCoroutine(CheckerCoroutine());
	}

	private IEnumerator CheckerCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
			if (GameClientManager.Current == null)
				continue;
			if (GameClientManager.Current.State != ActivationPlayerState)
				continue;

			SceneManager.LoadScene(SceneToActivate, LoadSceneMode.Single);
		}
	}
}
