using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Donkey.Client;
using Donkey.Common;
using System;

public class LoginScript : MonoBehaviour
{
	public InputField LoginInput;
	public GameObject LoginPanel;
	public GameObject MenuPanel;

	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Return))
			LoginToServer();
	}

	public void LoginToServer()
	{
		if (LoginPanel == null)
			throw new Exception("LoginPanel isnt set!");

		if (MenuPanel == null)
			throw new Exception("MenuPanel isnt set!");

		if (LoginInput == null)
            return;

		var login = LoginInput.text.Trim();
		if (string.IsNullOrEmpty(login))
            return;
		
		var auth = new AuthData(login, "");
		var client = GameClientManager.CreateNew(auth);
		Debug.Log(string.Format("Client created"));

#if (!UNITY_EDITOR)
		

		var isRegistred = client.Register();
		Debug.Log(string.Format("Client registered: {0}", isRegistred));

		var isAuth = client.Auth();
		Debug.Log(string.Format("Client authenticated: {0}", isAuth));

		if (!isAuth)
			return;
#endif

		LoginPanel.SetActive(false);
		MenuPanel.SetActive(true);
	}
}


