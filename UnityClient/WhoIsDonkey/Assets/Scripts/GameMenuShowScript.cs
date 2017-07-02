using Donkey.Client;
using UnityEngine;

public class GameMenuShowScript : MonoBehaviour
{
	public GameObject GameMenu;

	void Update()
	{
		if (!Input.GetKeyDown(KeyCode.Escape))
			return;
		if (GameClientManager.Current.CurrentGameState.GameEnded)
			return;

		GameMenu.SetActive(!GameMenu.activeSelf);
	}
}
