using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    public float timeLeft;
    private float timerTime;
    public bool timerIsOn = false;

    private void Start()
    {
        if (DataManager.Instance.players != null)
        {
            DataManager.Instance.currentPlayerIndex = Random.Range(0, 2);
            int i = ReturnActivePlayer();
            GameObject.Find("StartingPlayer").GetComponent<TextMeshProUGUI>().text = "The starting player is: " + DataManager.Instance.players[i];

            if(uiManager != null) { uiManager.ActivePlayerUI(); }
        }

        timerTime = timeLeft;
    }

    public void NextPlayer()
    {
        if(DataManager.Instance.currentPlayerIndex >= DataManager.Instance.players.Count - 1)
        {
            DataManager.Instance.currentPlayerIndex--;
        }
        else
        {
            DataManager.Instance.currentPlayerIndex++;
        }

        ResetTimer();
        if (uiManager != null) { uiManager.ActivePlayerUI(); }
    }

    public void AddScore(int scoreToAdd)
    {
        switch (ReturnActivePlayer())
        {
            case 0:
                DataManager.Instance.playerOneScore += scoreToAdd;
                break;
            case 1:
                DataManager.Instance.playerTwoScore += scoreToAdd;
                break;
            default:
                Debug.Log("No active player found");
                break;
        }

        uiManager.UpdateScoreUI(ReturnActivePlayer());
    }

    private int ReturnActivePlayer()
    {
        Debug.Log("Current player is nr: " + DataManager.Instance.currentPlayerIndex);
        return DataManager.Instance.currentPlayerIndex;
    }

    public void TimerSwitch()
    {
        if (timerIsOn)
        {
            timerIsOn = false;
        }
        else
        {
            timerIsOn = true;
        }
    }

    private void FixedUpdate()
    {
        if (timerIsOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }
            else
            {
                Debug.Log("You took too long, your turn was force skipped");
                timeLeft = 0;
                timerIsOn = false;
                NextPlayer();
            }
        }
    }

    private void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        uiManager.UpdateTimerUI(string.Format("{0:00}:{1:00}", minutes, seconds));
    }

    private void ResetTimer()
    {
        timeLeft = timerTime;
        timerIsOn = true;
    }
}