using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header("Player properties")]
    public string playerOneName;
    public string playerTwoName;

    public int playerOneScore;
    public int playerTwoScore;

    public List<string> players;

    public static DataManager Instance;

    public int currentPlayerIndex;

    [Header("Dice")]
    public int[] diceResult = new int[3];
    public bool diceIsCounted = false;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
