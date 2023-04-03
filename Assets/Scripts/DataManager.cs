using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public string playerOneName;
    public string playerTwoName;

    public int playerOneScore;
    public int playerTwoScore;

    public List<string> players;

    public static DataManager Instance;

    public int currentPlayerIndex;

    public int[] diceResult = new int[3];

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(diceResult[1] != 0)
        {
            diceResult[2] = diceResult[0] + diceResult[1]; 
        }
    }
}
