using Donkey.Client;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneEndScript : MonoBehaviour
{
	public GameObject ActivateMenu;
	public string ActivateScene;

	private Coroutine _coroutine;

	void Start()
	{
		if (ActivateMenu == null)
			throw new ArgumentNullException("Argument ActivateMenu cannot be null");
		if (string.IsNullOrEmpty(ActivateScene))
			throw new ArgumentNullException("Argument ActivateScene cannot be null");

		_coroutine = StartCoroutine(CheckerCoroutine());
	}

	private IEnumerator CheckerCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
			if (GameClientManager.Current == null)
				continue;
			if (!GameClientManager.Current.CurrentGameState.GameEnded)
				continue;

			ActivateMenu.SetActive(true);
			break;
		}
	}

	public void ChangeScene()
	{
		GameClientManager.Current.Leave();
		SceneManager.LoadScene(ActivateScene, LoadSceneMode.Single);
	}
}
