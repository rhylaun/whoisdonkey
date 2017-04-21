using Donkey.Client;
using Donkey.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeadersboardUpdaterScript : MonoBehaviour
{
	public GameObject ListViewContent;
	public GameObject ListItemPrefab;

	public void RefreshLeadersboard()
	{
		var scores = GameClientManager.Current.GetStatistics();
		ClearContent();
		FillContent(scores);
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

	private void FillContent(StatisticRecord[] list)
	{
		foreach (var record in list)
		{
			var newItem = GameObject.Instantiate(ListItemPrefab);

			SetTextByName(newItem, "Username", record.Name);
			SetTextByName(newItem, "Games", record.GamesPlayed.ToString());
			SetTextByName(newItem, "Donkey", record.FinishWithDonkey.ToString());
			SetTextByName(newItem, "Score", record.TotalScore.ToString());

			newItem.transform.SetParent(ListViewContent.transform);
		}
	}

	private void SetTextByName(GameObject obj, string compName, string value)
	{
		obj.transform.Find(compName).GetComponent<Text>().text = value;
	}

}
