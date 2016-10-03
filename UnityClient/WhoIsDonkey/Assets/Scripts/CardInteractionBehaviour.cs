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
            switch(tag)
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
    void OnMouseEnter()
    {
        if (State == CardState.InHand)
        {
            _savedPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, Camera.current.transform.position, HoverDistance);
        }
    }

    void OnMouseExit()
    {
        if (State == CardState.InHand)
        {
            transform.position = _savedPosition;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && State == CardState.InHand)
        {
            transform.position += Vector3.up * 0.5f;
            State = CardState.Selected;
            return;
        }

        if (Input.GetMouseButtonDown(1) && State == CardState.Selected)
        {
            transform.position -= Vector3.up * 0.5f;
            State = CardState.InHand;
        }
    }

    private GameObject GetHand()
    {
        return GameObject.FindGameObjectWithTag("Hand");
    }
}

public enum CardState
{
    InHand,
    Selected,
    Dropped
}
