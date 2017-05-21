using UnityEngine;
using System.Collections;
using Donkey.Client;
using Donkey.Common;
using System.Collections.Generic;

public class DropPointBehaviour : MonoBehaviour
{
	private GameObject _dropPoint;
	private Coroutine _coroutine;

	public int CheckInterval = 1;

	void Start()
	{
		_dropPoint = GameObject.FindGameObjectWithTag("DropPoint");
		_coroutine = StartCoroutine(CheckerCoroutine());
	}

	void Update()
	{
		if (_dropPoint == null) return;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			var selectedCards = GameObject.FindGameObjectsWithTag("SelectedCard");
			var list = new List<Card>();
			foreach (var obj in selectedCards)
				list.Add(CardHelper.GetCardValue(obj));
			GameClientManager.Current.MakeMove(list);
		}
	}

	private IEnumerator CheckerCoroutine()
	{
		int currentStep = 0;
		var history = new GameHistory();
		while (true)
		{
			yield return new WaitForSeconds(CheckInterval);
			if (GameClientManager.Current == null)
				continue;
			if (GameClientManager.Current.State != PlayerState.Game)
				continue;

			if (history.Count <= currentStep)
			{
				var gameMoves = GameClientManager.Current.GetHistory(currentStep);
				for (int i = 0; i < gameMoves.Length; i++)
					history.Add(gameMoves[i]);
			}

			if (currentStep <= GameClientManager.Current.CurrentGameStep)
			{
				var moves = history.ToArray(currentStep);
				PlayMove(moves[0]);
				currentStep++;
			}
		}
	}

	private void PlayMove(GameMove move)
	{
		switch (move.MoveType)
		{
			case MoveType.Clear:
				ClearBoard();
				break;
			case MoveType.Pass:
				break;
			case MoveType.Drop:
				DropCards(move.Cards);
				break;
			case MoveType.Take:
				break;
		}
	}

	private void DropCards(List<Card> list)
	{
		Debug.Log("Dropping cards");
		foreach (var card in list)
		{
			var cardObj = CardHelper.CreateCardObject(card);
			CardHelper.DropCard(cardObj);
			SetAproxRotattion(cardObj);
		}
	}

	private void ClearBoard()
	{
		Debug.Log("Clearing board");
		var droppedCards = GameObject.FindGameObjectsWithTag("DroppedCard");
		for (int i = 0; i < droppedCards.Length; i++)
		{
			droppedCards[i].transform.SetParent(null, false);
			Destroy(droppedCards[i]);
		}
	}

	private void SetAproxRotattion(GameObject cardObj)
	{
		cardObj.transform.LookAt(Camera.main.transform);
		cardObj.transform.Rotate(90, 0, 0);
		cardObj.transform.Rotate(0, Random.value * 90 - 45, 0);
	}

}
