using Donkey.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameJoinerScript : MonoBehaviour
{
	public GameObject LobbyPanel;
	public GameObject GamePanel;
	public GameObject MenuPanel;
	public GameObject PlayerUpdater;

	public void JoinLobby(string lobbyName)
	{
		if (LobbyPanel == null)
			throw new Exception("LobbyPanel isnt set!");
		if (GamePanel == null)
			throw new Exception("GamePanel isnt set!");
		if (MenuPanel == null)
			throw new Exception("MainPanel isnt set!");

		var isLobbyJoined = GameClientManager.Current.JoinLobby(lobbyName);
		Debug.Log(string.Format("Lobby '{0}' joined: {1}", lobbyName, isLobbyJoined));
		Debug.Log(string.Format("State: {0}", GameClientManager.Current.State));

		if (!isLobbyJoined)
			return;

		LobbyPanel.SetActive(false);
		GamePanel.SetActive(true);
		var updater = PlayerUpdater.GetComponent<PlayerUpdaterScript>();
		updater.RefreshPlayers();
	}

	public void CreateAndJoinLobby()
	{
		if (LobbyPanel == null)
			throw new Exception("LobbyPanel isnt set!");
		if (GamePanel == null)
			throw new Exception("GamePanel isnt set!");
		if (MenuPanel == null)
			throw new Exception("MainPanel isnt set!");

		var lobbyName = GameClientManager.Current.AuthData.Login;
		var isCreated = GameClientManager.Current.CreateLobby(lobbyName);

		if (!isCreated)
			return;

		MenuPanel.SetActive(false);
		GamePanel.SetActive(true);
		var updater = PlayerUpdater.GetComponent<PlayerUpdaterScript>();
		updater.RefreshPlayers();
	}

	public void ReadyForGame()
	{
		var readyResult = GameClientManager.Current.StartGame();
		Debug.Log(string.Format("Ready result : {0}", readyResult));
	}
}
