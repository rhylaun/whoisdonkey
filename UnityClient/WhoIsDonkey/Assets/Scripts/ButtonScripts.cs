using UnityEngine;
using UnityEngine.UI;
using Donkey.Client;
using System.Collections.Generic;

public class ButtonScripts : MonoBehaviour
{
    public GameObject Content;
    public GameObject ListItemPrefab;

    public void RefreshLobbies()
    {
        var lobbies = GameClientManager.Current.GetLobbies();

        ClearContent();
        FillContent(lobbies);
    }

    public void RefreshPlayers()
    {
        var players = GameClientManager.Current.GetPlayers();
        ClearContent();
        FillContent(players);
    }

    private void ClearContent()
    {
        var count = Content.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            var toDelete = Content.transform.GetChild(0);
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
            newItem.transform.SetParent(Content.transform);
        }
    }

    public void JoinLobby()
    {
        var lobbyName = this.transform.FindChild("Text").GetComponent<Text>().text;
        var isLobbyJoined = GameClientManager.Current.JoinLobby(lobbyName);
        Debug.Log(string.Format("Lobby '{0}' joined: {1}", lobbyName, isLobbyJoined));
        Debug.Log(string.Format("State: {0}", GameClientManager.Current.State));
    }

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
