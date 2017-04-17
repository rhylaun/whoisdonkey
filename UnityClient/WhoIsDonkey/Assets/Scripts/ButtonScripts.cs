using UnityEngine;
using UnityEngine.UI;
using Donkey.Client;
using System.Collections.Generic;

public class ButtonScripts : MonoBehaviour
{
   
	
    public void Leave()
    {
        var result = GameClientManager.Current.Leave();
        Debug.Log(string.Format("Leaving with result: {0}", result));
        Debug.Log(string.Format("State: {0}", GameClientManager.Current.State));
    }

    public void Ready()
    {
        var result = GameClientManager.Current.StartGame();
        Debug.Log(string.Format("Starting with result: {0}", result));
        Debug.Log(string.Format("State: {0}", GameClientManager.Current.State));
    }
}
