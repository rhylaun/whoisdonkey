using Donkey.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpdaterScript : MonoBehaviour {

	public GameObject ListViewContent;
	public GameObject ListItemPrefab;

	public void RefreshPlayers()
	{
		
		var players = GameClientManager.Current.GetPlayers();
		ClearContent();
		FillContent(players);
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
