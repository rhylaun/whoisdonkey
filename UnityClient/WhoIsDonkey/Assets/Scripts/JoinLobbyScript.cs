using Donkey.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyScript : MonoBehaviour
{
	public void JoinLobby()
	{
		var lobbyName = this.transform.FindChild("Text").GetComponent<Text>().text;
		var gameJoiner = GameObject.FindGameObjectWithTag("GameJoiner");

		var joinScript = gameJoiner.GetComponent<GameJoinerScript>();
		joinScript.JoinLobby(lobbyName);
	}
}
