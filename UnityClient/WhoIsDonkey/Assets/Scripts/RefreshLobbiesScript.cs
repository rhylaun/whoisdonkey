using UnityEngine;
using UnityEngine.UI;
using Donkey.Client;
using System.Collections.Generic;

public class RefreshLobbiesScript : MonoBehaviour
{
	public GameObject Content;
	public Button ListItemPrefab;

	public void OnClick()
	{
		var lobbies = GameClientManager.Current.GetLobbies();

		ClearContent();
		FillContent(lobbies);
	}

	private void ClearContent()
	{
		var count = Content.transform.childCount;
		for (int i = 0; i < count; i++)
		{
			var toDelete = Content.transform.GetChild(0);
			toDelete.transform.parent = null;
			//toDelete.gameObject.SetActive(false);
			GameObject.Destroy(toDelete);
		}
	}

	private void FillContent(List<string> list)
	{
		foreach (var itemName in list)
		{
			var newItem = GameObject.Instantiate(ListItemPrefab);
			newItem.transform.FindChild("Text").GetComponent<Text>().text = itemName;
			newItem.name = itemName;
			newItem.transform.SetParent(Content.transform);
		}
	}
}
