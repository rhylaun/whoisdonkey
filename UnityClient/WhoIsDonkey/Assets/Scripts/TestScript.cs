using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour
{
    public GameObject Target;
    public GameObject Prefab;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var crdObj = CardHelper.CreateCardObject(Donkey.Common.Card.Donkey);
            CardHelper.DropCard(crdObj);
        }
    }
}
