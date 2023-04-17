using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variables
    [Header("Managers")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private InputManager inputManager;

    [Header("Hands")]

    public int handSize = 10;

    [SerializeField] private GameObject playerOneHand;
    [SerializeField] private GameObject playerTwoHand;

    [SerializeField] private List<GameObject> oneHandSlots;
    [SerializeField] private List<GameObject> twoHandSlots;
    [SerializeField] private List<GameObject> oneCards;
    [SerializeField] private List<GameObject> twoCards;

    [Header("Prefabs")]
    [SerializeField] private GameObject handSlotPrefab;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject dataManagerPrefab;
    [SerializeField] private GameObject dicePrefab;

    [Header("Timer")]
    public float timeLeft;
    private float timerTime;
    public bool timerIsOn = false;

    [Header("Dice")]
    [SerializeField] List<Dice> dice;
    [SerializeField] GameObject[] diceSpawnpoints;
    public int numberOfThrowsLeft;

    #endregion

    #region StartupMethods
    private void Awake()
    {
        if (DataManager.Instance == null)
        {
            Instantiate(dataManagerPrefab);
            dataManager = DataManager.Instance;
            dataManager.players.Add("Player 1");
            dataManager.players.Add("Player 2");
            dataManager.numberOfThrows = 1;
        }

        if (DataManager.Instance != null)
        {
            dataManager = DataManager.Instance;

            dataManager.currentPlayerIndex = Random.Range(0, 2);

            if (uiManager != null) { uiManager.ActivePlayerUI(); }
            if (cameraManager != null) { cameraManager.NextPlayerCamera(dataManager.currentPlayerIndex); }
        }

        numberOfThrowsLeft = dataManager.numberOfThrows;

        ShowDice(numberOfThrowsLeft);
        uiManager.UpdateUI("Feedback", "Press SPACE to throw the dice.");
        timerTime = timeLeft;
        TimerSwitch();
    }

    private void Start()
    {
        SetHandPosition();
        CreateHandSlots();
    }
#endregion

    #region PlayerMethods
    public void NextPlayer()
    {
        if(dataManager.currentPlayerIndex >= dataManager.players.Count - 1)
        {
            dataManager.currentPlayerIndex--;
        }
        else
        {
            dataManager.currentPlayerIndex++;
        }

        cameraManager.NextPlayerCamera(dataManager.currentPlayerIndex);
        ResetTimer();
        if (uiManager != null) { uiManager.ActivePlayerUI(); }

        dataManager.numberOfThrows = numberOfThrowsLeft;
        ShowDice(numberOfThrowsLeft);
    }

    public void AddScore(int scoreToAdd)
    {
        switch (ReturnActivePlayer())
        {
            case 0:
                dataManager.playerOneScore += scoreToAdd;
                break;
            case 1:
                dataManager.playerTwoScore += scoreToAdd;
                break;
            default:
                Debug.Log("No active player found");
                break;
        }

        uiManager.UpdateUI("Score", (ReturnActivePlayer().ToString()));
    }

    private int ReturnActivePlayer()
    {
        return dataManager.currentPlayerIndex;
    }
    #endregion

    #region TimerMethods
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

        CheckDice();
    }

    private void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        uiManager.UpdateUI("Timer", (string.Format("{0:00}:{1:00}", minutes, seconds)));
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

    private void ResetTimer()
    {
        if (!timerIsOn) { uiManager.PausePlayButton(); }
        timeLeft = timerTime;
        timerIsOn = true;
    }
    #endregion

    #region HandMethods
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
    #endregion

    #region DiceMethods
    //public void SpawnDice(int diceCount)
    //{
    //    if(diceCount <= 1 && currentDiceCount == 0)
    //    {
    //        var newDice = Instantiate(dicePrefab, diceSpawnpoints[0].transform);
    //        dice.Insert(0, newDice.GetComponent<Dice>());

    //        inputManager.dice.Insert(0, newDice.GetComponent<Dice>());

    //        Debug.Log("Added dice");
    //        currentDiceCount++;
    //    }
    //    else
    //    {
    //        for(int i = 0; i < diceCount; i++)
    //        {
    //            Debug.Log("amount of dices we spawn: " + diceCount);
    //            var newDice = Instantiate(dicePrefab, diceSpawnpoints[i].transform);
    //            dice.Insert(i, newDice.GetComponent<Dice>());

    //            inputManager.dice.Insert(i, newDice.GetComponent<Dice>());

    //            DiceSide[] diceSides;

    //            diceSides = dice[i].GetComponentsInChildren<DiceSide>();
    //            foreach(DiceSide d in diceSides)
    //            {
    //                d.diceNumber = i;
    //            }
                
    //            currentDiceCount++;
    //            //inputManager.RefreshDice();

    //            Debug.Log("Added dice" + "current dice in the scene: " + currentDiceCount);
    //        }
    //    }
    //}

    //public void DestroyDice()
    //{
    //    foreach(Dice d in dice)
    //    {
    //        if(d != null)
    //        Destroy(d.gameObject);
    //    }

    //    currentDiceCount = 0;
    //    inputManager.dice.Clear();
    //    dice.Clear();
    //}

    public void ShowDice(int diceCount)
    {
        for(int i = 0; i < diceCount; i++)
        {
            dice[i].gameObject.SetActive(true);
        }
    }

    public void HideDice(int diceCount)
    {
        for (int i = 0; i < diceCount; i++)
        {
            if(dice[i].GetComponent<Rigidbody>().velocity.magnitude < 0.01)
            {
                dice[i].gameObject.SetActive(false);
            }
        }
    }

    private void CheckDice()
    {
        if (dice.Count != 0 && dice[1] != null && dice[1].gameObject.activeSelf)
        {
            if (dice[0].GetComponent<Rigidbody>().velocity.magnitude < 0.01 && dice[1].GetComponent<Rigidbody>().velocity.magnitude < 0.01 && dataManager.diceIsCounted)
            {
                dataManager.diceResult[2] = dataManager.diceResult[0] + dataManager.diceResult[1];
                uiManager.UpdateUI("Dice", ("Result is: " + dataManager.diceResult[2]));
                dataManager.diceIsCounted = false;
                if(dataManager.numberOfThrows == 0) 
                { 
                    StartCoroutine(RemoveAfterSeconds(1, dice[0].gameObject)); 
                    StartCoroutine(RemoveAfterSeconds(1, dice[1].gameObject)); 
                }
            }
        }
        else
        {
            if(dice.Count != 0 && dice[0] != null && dice[0].gameObject.activeSelf)
            {
                if (dice[0].GetComponent<Rigidbody>().velocity.magnitude < 0.01 && dataManager.diceIsCounted)
                {
                    Debug.Log(dataManager.numberOfThrows);
                    if (dataManager.numberOfThrows == 0) 
                    { 
                        StartCoroutine(RemoveAfterSeconds(1, dice[0].gameObject)); 
                    }
                    uiManager.UpdateUI("Dice", ("Result is: " + dataManager.diceResult[0]));
                    dataManager.diceIsCounted = false;
                }
            }
        }
    }

    public void ChangeNumberOfThrows(int newNumber) { numberOfThrowsLeft = newNumber; }

    #endregion

    private IEnumerator RemoveAfterSeconds(int seconds, GameObject obj)
    {
        yield return new WaitForSeconds(seconds);
        uiManager.UpdateUI("Feedback", "");
        obj.SetActive(false);
    }
}