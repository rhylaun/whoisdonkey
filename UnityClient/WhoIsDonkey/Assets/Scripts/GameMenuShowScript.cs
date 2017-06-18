using UnityEngine;

public class GameMenuShowScript : MonoBehaviour
{
	public GameObject GameMenu;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameMenu.SetActive(!GameMenu.activeSelf);
		}
	}
}
