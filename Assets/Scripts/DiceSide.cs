using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    public int sideNumber;
    public int diceNumber;

    public bool groundCollision = false;

    private Dice dice;

    private void Start()
    {
        dice = GetComponentInParent<Dice>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            groundCollision = true;
            dice.Result(sideNumber, diceNumber);
        }
        else
        {
            groundCollision = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Ground")
        {
            groundCollision = false;
        }
    }
}

