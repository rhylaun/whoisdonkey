using UnityEngine;
using System.Collections;
using System;

public class StackBoardBehavior : MonoBehaviour
{
	public float SpaceBetweenObjects = 0.1f;
	public int TotalCount = 13;

	private int _childCount = 0;
	private GameObject _context;

	void Start()
	{
		_context = new GameObject("Context");
		_context.transform.SetParent(transform);
		_context.transform.localPosition = Vector3.zero;
		_context.transform.rotation = transform.rotation;
	}

	void Update()
	{
		if (_context.transform.childCount == _childCount)
			return;

		_childCount = _context.transform.childCount;
		var childs = new Transform[_childCount];
		for (int i = 0; i < _childCount; i++)
			childs[i] = _context.transform.GetChild(i);
		RotateChilds(childs);
		RecalcChilds(childs);
	}

	void RotateChilds(Transform[] childs)
	{
		for (int i = 0; i < _childCount; i++)
		{
			childs[i].rotation = transform.rotation;
			childs[i].Rotate(Vector3.up, 180);
		}
	}

	void RecalcChilds(Transform[] childs)
	{
		var center = this.GetComponent<Renderer>().bounds.center;
		var availableWidth = this.GetComponent<Renderer>().bounds.size.x;
		var totalObjWidth = 0.0f;
		try
		{
			totalObjWidth += childs[0].GetComponentInChildren<Renderer>().bounds.size.x * TotalCount;

			var totalWidthWithSpaces = totalObjWidth + SpaceBetweenObjects * (TotalCount - 1);
			var multiplier = availableWidth / totalWidthWithSpaces;
			float objWidth = totalObjWidth * multiplier / TotalCount;
			for (int i = 0; i < _childCount; i++)
			{
				childs[i].localScale *= multiplier;
				var shift = (TotalCount - _childCount) / 2 + i;
				childs[i].position = center - transform.right * ((availableWidth - objWidth) / 2 - shift * (objWidth + SpaceBetweenObjects * multiplier));
			}

		}
		catch (Exception ex)
		{
			Console.WriteLine("Error in stack panel: " + ex.Message);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawRay(transform.position, transform.up * 2);
	}
}
