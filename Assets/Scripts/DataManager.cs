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
    public int numberOfThrows = 1;

    [Header("Dice")]
    public List<int> diceResult = new List<int>(2);
    public bool diceIsCounted = false;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
