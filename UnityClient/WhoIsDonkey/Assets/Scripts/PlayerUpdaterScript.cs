using Donkey.Client;
using Donkey.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpdaterScript : MonoBehaviour
{

	public GameObject ListViewContent;
	public GameObject ListItemPrefab;

	public void RefreshPlayers()
	{
		var state = GameClientManager.Current.GetLobbyState();
		ClearContent();
		FillContent(state.Players);
	}

	public void AddAI()
	{
		var client = GameClientManager.Current;
		var botNames = client.GetServerInfo();
		GameClientManager.Current.AddAI(botNames.First());
		RefreshPlayers();
	}

	public void RemoveAI()
	{
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

			newItem.name = item.Name;
			newItem.transform.SetParent(ListViewContent.transform);
		}
	}
}
