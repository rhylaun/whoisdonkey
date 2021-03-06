﻿using Donkey.Client;
using Donkey.Common;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpdaterScript : MonoBehaviour
{

	public GameObject ListViewContent;
	public GameObject ListItemPrefab;
	public GameObject AiDropdown;

	public void RefreshPlayers()
	{
		var state = GameClientManager.Current.GetLobbyState();
		ClearContent();
		FillContent(state.Players);
	}

	public void AddAI()
	{
		var dropdown = AiDropdown.GetComponent<Dropdown>();
		var aiName = dropdown.captionText.text;
		GameClientManager.Current.AddAI(aiName);
		RefreshPlayers();
	}

	public void RemoveAI(string aiName)
	{
		GameClientManager.Current.RemoveAI(aiName);
		RefreshPlayers();
	}

	private void ClearContent()
	{
		var count = ListViewContent.transform.childCount;
		for (int i = 0; i < count; i++)
		{
			var toDelete = ListViewContent.transform.GetChild(0);
			toDelete.transform.SetParent(null, false);
			Destroy(toDelete.gameObject);
		}
	}

	private void FillContent(List<PlayerDescription> list)
	{
		foreach (var item in list)
		{
			var newItem = GameObject.Instantiate(ListItemPrefab);

			var userNameComponent = newItem.transform.FindChild("UserName").GetComponent<Text>();
			userNameComponent.text = item.Name;

			var userTypeComponent = newItem.transform.FindChild("PlayerType").GetComponent<Text>();
			userTypeComponent.text = item.PlayerType == PlayerType.Human ? "Human" : "AI";

			var button = newItem.transform.FindChild("RemovePlayerButton");
			if (item.PlayerType == PlayerType.Human)
			{
				button.gameObject.SetActive(false);
			}
			if (item.PlayerType == PlayerType.AI)
			{
				var btnComponent = button.GetComponent<Button>();
				btnComponent.onClick.AddListener(() => RemoveAI(item.Name));
			}

			newItem.name = item.Name;
			newItem.transform.SetParent(ListViewContent.transform);
		}
	}
}
