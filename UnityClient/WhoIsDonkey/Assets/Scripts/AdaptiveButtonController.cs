using Donkey.Client;
using Donkey.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveButtonController : MonoBehaviour
{
	public float MoveTime = 0.2f;
	public float PassTime = 3f;

	private float _mouseDownTime;
	private float _mouseUpTime;

	private Coroutine _passCoroutine;

	public void OnMouseDown()
	{
		_mouseDownTime = 0f;
		_mouseUpTime = 0f;

		var rotators = this.transform.GetComponentsInChildren<AdaptiveButtonRotator>();
		foreach (var r in rotators)
			r.enabled = true;
		_mouseDownTime = Time.realtimeSinceStartup;
		if (_passCoroutine != null)
			StopCoroutine(_passCoroutine);
		_passCoroutine = StartCoroutine(PassCheckerCoroutine());
	}

	public void OnMouseUp()
	{
		var rotators = this.transform.GetComponentsInChildren<AdaptiveButtonRotator>();
		foreach (var r in rotators)
			r.enabled = false;
		_mouseUpTime = Time.realtimeSinceStartup;
		ResolvePress();
	}

	private void ResolvePress()
	{
		if (_passCoroutine != null)
			StopCoroutine(_passCoroutine);

		var isMove = (_mouseUpTime > _mouseDownTime) && (_mouseUpTime - _mouseDownTime < MoveTime);
		if (isMove)
			MakeCardMove();

		_mouseDownTime = 0f;
		_mouseUpTime = 0f;
	}

	private IEnumerator PassCheckerCoroutine()
	{
		const float sleepTime = 0.05f;
		var startPassTime = Time.realtimeSinceStartup;

		while (Time.realtimeSinceStartup - startPassTime < PassTime)
			yield return new WaitForSeconds(sleepTime);

		var rotators = this.transform.GetComponentsInChildren<AdaptiveButtonRotator>();
		foreach (var r in rotators)
			r.enabled = false;
		MakePassMove();
	}

	private void MakeCardMove()
	{
		var selectedCards = GameObject.FindGameObjectsWithTag("SelectedCard");
		var list = new List<Card>();
		foreach (var obj in selectedCards)
			list.Add(CardHelper.GetCardValue(obj));
		GameClientManager.Current.MakeMove(list);
	}

	private void MakePassMove()
	{
		GameClientManager.Current.Pass();
	}
}
