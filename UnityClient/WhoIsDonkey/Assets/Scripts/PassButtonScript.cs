using Donkey.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassButtonScript : MonoBehaviour
{
	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var result = GameClientManager.Current.Pass();
			Debug.Log("Passing: " + result);
		}
	}
}
