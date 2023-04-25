using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentCard : MonoBehaviour
{
    public Transform parent;

    public void ParentCardTo(GameObject card)
    {
        card.transform.SetParent(parent, true);
    }
}
