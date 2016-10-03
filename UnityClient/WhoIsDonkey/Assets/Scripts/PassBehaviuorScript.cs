using UnityEngine;
using System.Collections;
using Donkey.Client;

public class PassBehaviuorScript : MonoBehaviour
{

	void OnMouseDown()
	{
		var client = GameClientManager.Current;
		var result = client.Pass();
		Debug.Log("Pass : " + result);
	}
}
