using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{

    public void Exit()
    {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    public void ShowLobbyPanel()
    {

    }

    public void ShowCreateGame()
    {

    }

    public void ShowLeadersboard()
    {

    }
}
