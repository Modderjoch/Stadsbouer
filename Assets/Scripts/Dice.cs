using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 diceVelocity;

    [SerializeField] private GameObject[] sides;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Roll()
    {
        diceVelocity = rb.velocity;

        float dirX = Random.Range(-650, 650);
        float dirY = Random.Range(-650, 650);
        float dirZ = Random.Range(-650, 650);
        transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        transform.rotation = new Quaternion(transform.rotation.x + (Random.Range(0, 4) * 90), transform.rotation.y + (Random.Range(0, 4) * 90), transform.rotation.z + (Random.Range(0, 4) * 90), 0);
        rb.AddForce(Vector3.up * Random.Range(400, 700));
        rb.AddTorque(dirX, dirY, dirZ);
    }

    public void Result(int result, int diceNr)
    {
        DataManager.Instance.diceResult[diceNr] = result;
        Debug.Log("You've thrown a " + result);
    }
}
