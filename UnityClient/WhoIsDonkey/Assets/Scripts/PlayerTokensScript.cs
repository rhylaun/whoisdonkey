using Donkey.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTokensScript : MonoBehaviour
{
	public GameObject PlayerTokenPrefab;
	public float Radius = 1.0f;
	public float Inclination = 30;

	private List<GameObject> _tokens = new List<GameObject>();
	private Dictionary<string, GameObject> _nameToToken = new Dictionary<string, GameObject>();
	private GameObject _holder;
	private string _currentPlayer;

	void Start()
	{
		CreatePlayerTokens();
		SpreadTokens();
		_holder.transform.Rotate(Vector3.right, Inclination);
		_currentPlayer = GameClientManager.Current.CurrentGameState.ActivePlayerName;
		RotateToToken(_nameToToken[_currentPlayer]);
	}

	void Update()
	{
		var player = GameClientManager.Current.CurrentGameState.ActivePlayerName;
		if (player == _currentPlayer)
			return;

		var token = _nameToToken[player];
		RotateToToken(token);
		_currentPlayer = player;
		Debug.Log("Rotated to " + _currentPlayer);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, Radius);
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
			_tokens.Add(token);
			_nameToToken.Add(player, token);
		}

	}

	private void SpreadTokens()
	{
		var center = this.transform.position;
		var anglePart = 360 / _tokens.Count;
		for (int i = 0; i < _tokens.Count; i++)
		{
			var transform = _tokens[i].transform;
			transform.position = center + Vector3.down * Radius;
			transform.RotateAround(center, Vector3.forward, anglePart * i);
		}
	}

	private void RotateToToken(GameObject token)
	{
		var angle = token.transform.rotation.eulerAngles.z;

		StartCoroutine(RotateCoroutine(angle));
	}

	private IEnumerator RotateCoroutine(float angle)
	{
		var count = 20;
		for (int i = 0; i < count; i++)
		{
			yield return new WaitForSeconds(1 / count);
			_holder.transform.Rotate(Vector3.forward, -angle / count, Space.Self);
		}
	}

}
