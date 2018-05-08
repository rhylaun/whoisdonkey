using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginMenuKeybindsBehaviour : MonoBehaviour
{
	public GameObject Menu;
	public GameObject LoginForm;

	private bool firstEnter = true;

	void Update()
	{
		if (Input.anyKeyDown && firstEnter)
		{
			firstEnter = false;
			return;
		}

		if (!Input.GetKeyUp(KeyCode.Escape))
			return;

		SwapMenu();
	}

	private void SwapMenu()
	{
		var isActive = Menu.activeSelf;
		Menu.SetActive(!isActive);
		LoginForm.SetActive(isActive);
	}
}
