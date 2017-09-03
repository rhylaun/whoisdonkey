using Donkey.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTokensScript : MonoBehaviour
{
	public GameObject PlayerTokenPrefab;
	public float Radius = 1.0f;

	private List<GameObject> _tokens = new List<GameObject>();
	private Dictionary<string, GameObject> _nameToToken = new Dictionary<string, GameObject>();
	private GameObject _holder;
	private string _currentPlayer;

	void Start()
	{
		CreatePlayerTokens();
		_currentPlayer = GameClientManager.Current.CurrentGameState.ActivePlayerName;
		SetCurrentPlayerVisible(_nameToToken[_currentPlayer]);
	}

	void Update()
	{
		var player = GameClientManager.Current.CurrentGameState.ActivePlayerName;
		if (player == _currentPlayer)
			return;

		var token = _nameToToken[player];
		SetCurrentPlayerVisible(token);
		_currentPlayer = player;
		Debug.Log("Rotated to " + _currentPlayer);
	}

	private void CreatePlayerTokens()
	{
		var players = GameClientManager.Current.GetPlayers();
		_holder = new GameObject("TokenHolder");
		_holder.transform.SetParent(this.transform);
		_holder.transform.position = this.transform.position;


		foreach (var player in players)
		{
			var token = GameObject.Instantiate(PlayerTokenPrefab);
			token.transform.Find("Username").GetComponent<TextMesh>().text = player;
			token.transform.SetParent(_holder.transform);
			token.transform.position = _holder.transform.position;
			_tokens.Add(token);
			_nameToToken.Add(player, token);
			token.SetActive(false);
		}
	}

	private void SetCurrentPlayerVisible(GameObject token)
	{
		foreach (var t in _tokens)
			t.SetActive(false);
		token.SetActive(true);
	}
}
