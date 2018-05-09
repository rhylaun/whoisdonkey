using Donkey.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AiDropdownBehaviour : MonoBehaviour
{
	void Start()
	{
		var client = GameClientManager.Current;
		var aiNames = client.GetServerInfo();
		var dropdown = this.GetComponentInParent<Dropdown>();
		dropdown.AddOptions(aiNames);
	}
}
