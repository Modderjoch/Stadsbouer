using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [HideInInspector] public string playerOneName;
    [HideInInspector] public string playerTwoName;

    public List<string> players;

    public static DataManager Instance;

    public int currentPlayerIndex;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
