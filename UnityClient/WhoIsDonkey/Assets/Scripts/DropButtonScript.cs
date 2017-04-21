using Donkey.Client;
using Donkey.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropButtonScript : MonoBehaviour
{
	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var selectedCards = GameObject.FindGameObjectsWithTag("SelectedCard");
			var list = new List<Card>();
			foreach (var obj in selectedCards)
				list.Add(CardHelper.GetCardValue(obj));
			var result = GameClientManager.Current.MakeMove(list);
			Debug.Log("Dropping cards: " + result);
		}
	}
}
