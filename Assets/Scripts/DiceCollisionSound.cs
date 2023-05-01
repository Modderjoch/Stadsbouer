using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCollisionSound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        AudioManager.Instance.Play(gameObject.name);
    }
}
