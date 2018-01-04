using Donkey.Client;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpdaterScript : MonoBehaviour {

	public GameObject ListViewContent;
	public GameObject ListItemPrefab;

	public void RefreshPlayers()
	{
		
		var state = GameClientManager.Current.GetLobbyState();
		ClearContent();
		FillContent(state.Players.Select(x => x.Name).ToList());
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
			var newItem = GameObject.Instantiate(ListItemPrefab);
			var textComponent = newItem.GetComponent<Text>();
			if (textComponent == null)
				textComponent = newItem.transform.FindChild("Text").GetComponent<Text>();
			textComponent.text = itemName;
			newItem.name = itemName;
			newItem.transform.SetParent(ListViewContent.transform);
		}
	}
}
