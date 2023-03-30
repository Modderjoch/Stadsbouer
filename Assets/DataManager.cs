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

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
