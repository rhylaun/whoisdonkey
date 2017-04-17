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
	public GameObject PlayerUpdater;

	public void JoinLobby(string lobbyName)
	{
		if (LobbyPanel == null)
			throw new Exception("LobbyPanel isnt set!");
		if (GamePanel == null)
			throw new Exception("GamePanel isnt set!");

#if (!UNITY_EDITOR)
		var isLobbyJoined = GameClientManager.Current.JoinLobby(lobbyName);
		Debug.Log(string.Format("Lobby '{0}' joined: {1}", lobbyName, isLobbyJoined));
		Debug.Log(string.Format("State: {0}", GameClientManager.Current.State));

		if (!isLobbyJoined)
			return;
#endif

		LobbyPanel.SetActive(false);
		GamePanel.SetActive(true);
		var updater = PlayerUpdater.GetComponent<PlayerUpdaterScript>();
		updater.RefreshPlayers();
	}
}
