﻿using Donkey.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTokensScript : MonoBehaviour
{
	public GameObject PlayerTokenPrefab;
	public float Radius = 1.0f;
	public float Inclination = 30;

	private List<GameObject> _tokens = new List<GameObject>();
	private GameObject _holder;

	void Start ()
	{
		CreatePlayerTokens();
		SpreadTokens();
		
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

		_holder.transform.Rotate(Vector3.right, Inclination);
	}

	void Update ()
	{
		
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, Radius);
	}
}
