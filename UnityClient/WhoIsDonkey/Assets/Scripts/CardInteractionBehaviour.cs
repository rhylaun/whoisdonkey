using UnityEngine;
using Donkey.Common;

public class CardInteractionBehaviour : MonoBehaviour
{
	public float HoverDistance = 0.2f;
	public Card CardValue = Card.One;

	public CardState State
	{
		get
		{
			switch (tag)
			{
				case "InHandCard": return CardState.InHand;
				case "SelectedCard": return CardState.Selected;
				case "DroppedCard": return CardState.Dropped;
			}
			return CardState.InHand;
		}
		set
		{
			switch (value)
			{
				case CardState.InHand:
					tag = "InHandCard";
					break;
				case CardState.Selected:
					tag = "SelectedCard";
					break;
				case CardState.Dropped:
					tag = "DroppedCard";
					break;
			}
		}
	}

	private Vector3 _savedPosition;
	private float _distance;
	private Vector3 _offset;
	private GameObject _dropArea;

	void Start()
	{
		_distance = (Camera.main.transform.position - GetHand().transform.position).magnitude;
		_dropArea = GetDropArea();
	}

	void OnMouseDown()
	{
		if (!Input.GetMouseButtonDown(0))
			return;

		var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distance);
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		_offset = this.transform.position - mousePosition;
		if (State == CardState.InHand)
			_savedPosition = this.transform.position;
	}

	void OnMouseUp()
	{
		if (!Input.GetMouseButtonUp(0))
			return;

		RaycastHit hit;
		int layerMask = 1 << 8;
		var ray = new Ray(this.transform.position, Vector3.forward);
		if (Physics.Raycast(ray, out hit, 10f, layerMask))
		{
			State = CardState.Selected;
		}
		else
		{
			State = CardState.InHand;
			ResetPosition();
		}
	}

	void OnMouseDrag()
	{
		if (_offset == null)
		{
			Debug.Log("_offset == null");
			return;
		}

		var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distance);
		var objPosition = Camera.main.ScreenToWorldPoint(mousePosition) + _offset;
		this.transform.position = objPosition;
	}

	private void ResetPosition()
	{
		transform.position = _savedPosition;
	}

	private GameObject GetHand()
	{
		return GameObject.FindGameObjectWithTag("Hand");
	}

	private GameObject GetDropArea()
	{
		return GameObject.FindGameObjectWithTag("DropArea");
	}
}

public enum CardState
{
	InHand,
	Selected,
	Dropped
}
