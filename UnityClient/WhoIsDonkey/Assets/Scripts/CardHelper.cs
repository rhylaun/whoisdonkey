using UnityEngine;
using System.Collections;
using Donkey.Common;
using System;

public static class CardHelper
{
    public static GameObject CreateCardObject(Card card)
    {
		var cardName = string.Format("Cards/card{0}prefab", (int)card);
        var result = GameObject.Instantiate(Resources.Load<GameObject>(cardName));
		var rigidBody = result.GetComponent<Rigidbody>();
		rigidBody.useGravity = false;
		rigidBody.isKinematic = true;
		return result;
    }

    public static void DropCard(GameObject card)
    {
        var dropPoint = GameObject.FindGameObjectWithTag("DropPoint");

		card.transform.localScale *= 0.5f;
		card.transform.Translate(dropPoint.transform.position -
                    card.transform.position + UnityEngine.Random.insideUnitSphere * 1.5f, Space.World);

		var rigidBody = card.GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
        rigidBody.isKinematic = false;
        card.tag = "DroppedCard";
    }

    public static Card GetCardValue(GameObject obj)
    {
        return obj.GetComponentInChildren<CardInteractionBehaviour>().CardValue;
    }
}
