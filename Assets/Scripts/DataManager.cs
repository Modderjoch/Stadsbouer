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

    public int playerOneMoney;
    public int playerTwoMoney;
    public int playerOneTotalMoney;
    public int playerTwoTotalMoney;

    public List<string> players;
    public string winCondition;

    public static DataManager Instance;

    public int currentPlayerIndex;

    [Header("Card properties")]
    public List<CardData> playerOneDeck;
    public List<CardData> playerTwoDeck;

    public List<CardData> unlockedCards;

    [Header("Dice")]
    public List<int> diceResult = new List<int>(2);
    public bool diceIsCounted = false;
    public int numberOfThrows = 1;
    public int numberOfCards = 1;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
