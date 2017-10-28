using Donkey.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTokensScript : MonoBehaviour
{
	private string _currentPlayer;
	private Text _textField;

	void Start()
	{
		_currentPlayer = GameClientManager.Current.CurrentGameState.ActivePlayerName;
		_textField = this.GetComponent<Text>();
		_textField.text = _currentPlayer;
	}

	void Update()
	{
		var player = GameClientManager.Current.CurrentGameState.ActivePlayerName;
		if (player == _currentPlayer)
			return;
		
		_currentPlayer = player;
		_textField.text = player;
		Debug.Log("Rotated to " + _currentPlayer);
	}
}
