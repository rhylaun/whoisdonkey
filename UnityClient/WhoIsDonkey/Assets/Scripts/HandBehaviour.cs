using UnityEngine;
using System.Collections;
using Donkey.Client;
using Donkey.Common;
using System.Collections.Generic;

public class HandBehaviour : MonoBehaviour
{
    private readonly List<GameObject> _handList = new List<GameObject>();

    void FixedUpdate()
    {
		var client = GameClientManager.Current;
        if (client.State != PlayerState.Game)
			return;

        var cards = client.GetCards();
		if (cards.Count == _handList.Count)
			return;

		ClearHand();
        foreach (var card in cards)
        {
			var cardObj = CardHelper.CreateCardObject(card);
			_handList.Add(cardObj);
            cardObj.transform.SetParent(transform.GetChild(0));
        }
    }

    private void ClearHand()
    {
        foreach (var cObj in _handList)
        {
            cObj.transform.SetParent(null, false);
            Destroy(cObj);
        }
        _handList.Clear();
    }
}
