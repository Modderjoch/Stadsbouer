using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private GameObject dataManager;

    [Header("Hands")]

    public int handSize = 10;

    [SerializeField] private GameObject playerOneHand;
    [SerializeField] private GameObject playerTwoHand;

    [SerializeField] private List<GameObject> oneHandSlots;
    [SerializeField] private List<GameObject> twoHandSlots;
    [SerializeField] private List<GameObject> oneCards;
    [SerializeField] private List<GameObject> twoCards;

    [SerializeField] private GameObject handSlotPrefab;
    [SerializeField] private GameObject cardPrefab;

    public float timeLeft;
    private float timerTime;
    public bool timerIsOn = false;

    private void Awake()
    {
        if (DataManager.Instance == null)
        {
            Instantiate(dataManager);
            DataManager.Instance.players.Add("Player 1");
            DataManager.Instance.players.Add("Player 2");
        }

        if (DataManager.Instance != null)
        {
            DataManager.Instance.currentPlayerIndex = Random.Range(0, 2);
            int i = ReturnActivePlayer();

            if (uiManager != null) { uiManager.ActivePlayerUI(); }
            if (cameraManager != null) { cameraManager.NextPlayerCamera(DataManager.Instance.currentPlayerIndex); }
        }

        timerTime = timeLeft;
        TimerSwitch();
    }

    private void Start()
    {
        SetHandPosition();
        CreateHandSlots();
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

        cameraManager.NextPlayerCamera(DataManager.Instance.currentPlayerIndex);
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
        if (!timerIsOn) { uiManager.PausePlayButton(); }
        timeLeft = timerTime;
        timerIsOn = true;
    }

    private void SetHandPosition()
    {
        playerOneHand.transform.position = new Vector3(-1.493f * handSize, playerOneHand.transform.position.y, playerOneHand.transform.position.z);
        playerTwoHand.transform.position = new Vector3(-1.493f * handSize, playerTwoHand.transform.position.y, playerTwoHand.transform.position.z);
    }

    private void CreateHandSlots()
    {
        for(int i = 0; i < handSize; i++)
        {
            float offset = 3.4f * i;
            Vector3 newPos = new Vector3(offset, 0, 0);

            oneHandSlots.Add(Instantiate(handSlotPrefab, (playerOneHand.transform.position + newPos), playerOneHand.transform.rotation, playerOneHand.transform));
            twoHandSlots.Add(Instantiate(handSlotPrefab, (playerTwoHand.transform.position + newPos), playerTwoHand.transform.rotation, playerTwoHand.transform));        
        }

        foreach (GameObject slot in oneHandSlots)
        {
            oneCards.Add(Instantiate(cardPrefab, slot.transform, false));

            for (int j = 0; j < oneCards.Count; j++)
            {
                oneCards[j].GetComponentInChildren<TextMeshProUGUI>().text = "Card number " + j;
            }
        }

        foreach (GameObject slot in twoHandSlots)
        {
            twoCards.Add(Instantiate(cardPrefab, slot.transform, false));

            for (int j = 0; j < twoCards.Count; j++)
            {
                twoCards[j].GetComponentInChildren<TextMeshProUGUI>().text = "Card number " + j;
            }
        }
    }
}