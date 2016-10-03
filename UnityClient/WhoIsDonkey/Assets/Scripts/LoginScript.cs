﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Donkey.Client;
using Donkey.Common;

public class LoginScript : MonoBehaviour
{
	public InputField LoginInput;
	public string SceneToLoad;

	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Return))
			OnClick();
	}

	public void OnClick()
	{
		if (LoginInput == null) return;

		var login = LoginInput.text.Trim();
		if (string.IsNullOrEmpty(login)) return;
		
		var auth = new AuthData(login, "");
		var client = GameClientManager.CreateNew(auth);

		Debug.Log(string.Format("Client created"));

		var isRegistred = client.Register();
		Debug.Log(string.Format("Client registered: {0}", isRegistred));

		var isAuth = client.Auth();
		Debug.Log(string.Format("Client authenticated: {0}", isAuth));
		if (isAuth)
			SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Single);
	}
}

