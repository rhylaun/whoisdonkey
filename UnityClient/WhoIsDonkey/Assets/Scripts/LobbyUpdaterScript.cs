using UnityEngine;
using UnityEngine.UI;
using Donkey.Client;
using System.Collections.Generic;
using Donkey.Common;

public class LobbyUpdaterScript : MonoBehaviour
{
	public GameObject ListViewContent;
	public GameObject LobbyPrefab;

	public void RefreshLobbies()
	{
		var lobbies = GameClientManager.Current.GetLobbies();
		ClearContent();
		FillContent(lobbies);
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

	private void FillContent(List<string> list)
	{
		foreach (var itemName in list)
		{
			var newItem = GameObject.Instantiate(LobbyPrefab);
			var textComponent = newItem.GetComponent<Text>();
			if (textComponent == null)
				textComponent = newItem.transform.FindChild("Text").GetComponent<Text>();
			textComponent.text = itemName;
			newItem.name = itemName;
			newItem.transform.SetParent(ListViewContent.transform);
		}
	}
}
