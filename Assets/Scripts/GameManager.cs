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

    [Header("Prefabs")]
    [SerializeField] private GameObject handSlotPrefab;
    [SerializeField] private GameObject dataManagerPrefab;
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private CardData cardDataPrefab;

    [Header("Timer")]
    public float timeLeft;
    private float timerTime;
    public bool timerIsOn = false;

    [Header("Dice")]
    [SerializeField] List<Dice> dice;
    [SerializeField] GameObject[] diceSpawnpoints;
    public int numberOfThrowsLeft;

    [Header("Deck")]
    public int handSize = 10;

    [SerializeField] private GameObject handOneTransform;
    [SerializeField] private GameObject handTwoTransform;

    public List<CardData> deckOne;
    public List<CardData> deckTwo;
    public List<CardData> handOne;
    public List<CardData> handTwo;
    public List<CardData> discardOne;
    public List<CardData> discardTwo;

    public List<int> oneID;
    public List<int> twoID;

    private bool firstTime = true;
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

            for (int i = 0; i < 15; i++) { dataManager.playerOneDeck.Add(cardDataPrefab); }
            for (int i = 0; i < 15; i++) { dataManager.playerTwoDeck.Add(cardDataPrefab); }
        }

        if (DataManager.Instance != null)
        {
            dataManager = DataManager.Instance;

            dataManager.currentPlayerIndex = Random.Range(0, 2);

            if (uiManager != null) { uiManager.ActivePlayerUI(); }
            if (cameraManager != null) { cameraManager.NextPlayerCamera(dataManager.currentPlayerIndex); }
        }

        numberOfThrowsLeft = dataManager.numberOfThrows;

        timerTime = timeLeft;
        TimerSwitch();
        ShowDice(numberOfThrowsLeft);
    }

    private void Start()
    {
        deckOne = new List<CardData>(dataManager.playerOneDeck);
        deckTwo = new List<CardData>(dataManager.playerTwoDeck);

        for (int i = 0; i < deckOne.Count; i++)
        {
            Debug.Log(deckOne[i].cardID);
            deckOne[i] = Instantiate(deckOne[i]);
            deckOne[i].cardID = deckOne[i].cardID + i;
            Debug.Log(deckOne[i].cardID);
        }

        for (int i = 0; i < deckTwo.Count; i++)
        {
            Debug.Log(deckTwo[i].cardID);
            deckTwo[i] = Instantiate(deckTwo[i]);
            deckTwo[i].cardID = deckTwo[i].cardID + i;
            Debug.Log(deckTwo[i].cardID);
        }

        DrawCards(handSize);
        InstantiateCardPrefabs();        
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
        Debug.Log("Timer is switched");
        
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

    #region CardMethods
    public void ShuffleDeck()
    {
        if(ReturnActivePlayer() == 0)
        {
            for (int i = 0; i < deckOne.Count; i++)
            {
                int randomIndex = Random.Range(i, deckOne.Count);
                CardData temp = deckOne[randomIndex];
                deckOne[randomIndex] = deckOne[i];
                deckOne[i] = temp;
            }
        }
        else
        {
            for (int i = 0; i < deckTwo.Count; i++)
            {
                int randomIndex = Random.Range(i, deckTwo.Count);
                CardData temp = deckTwo[randomIndex];
                deckTwo[randomIndex] = deckTwo[i];
                deckTwo[i] = temp;
            }
        }
    }

    public void DrawCards(int numCards)
    {
        if(firstTime)
        {
            for (int i = 0; i < numCards; i++)
            {
                if (deckOne.Count > 0)
                {
                    int randomIndex = Random.Range(i, deckOne.Count);
                    CardData temp = deckOne[randomIndex];
                    deckOne[randomIndex] = deckOne[i];
                    deckOne[i] = temp;

                    CardData drawnCard = deckOne[0];
                    deckOne.RemoveAt(0);
                    handOne.Add(drawnCard);
                    oneID.Add(drawnCard.cardID);

                    uiManager.UpdateUI("Deck", "0");
                }
            }

            for (int i = 0; i < numCards; i++)
            {
                if (deckTwo.Count > 0)
                {
                    int randomIndex = Random.Range(i, deckTwo.Count);
                    CardData temp = deckTwo[randomIndex];
                    deckTwo[randomIndex] = deckTwo[i];
                    deckTwo[i] = temp;

                    CardData drawnCard = deckTwo[0];
                    deckTwo.RemoveAt(0);
                    handTwo.Add(drawnCard);
                    twoID.Add(drawnCard.cardID);

                    uiManager.UpdateUI("Deck", "1");
                }
            }
            firstTime = false;
        }
        else
        {
            if (ReturnActivePlayer() == 0)
            {
                for (int i = 0; i < numCards; i++)
                {
                    if (deckOne.Count > 0)
                    {
                        CardData drawnCard = deckOne[0];
                        deckOne.RemoveAt(0);
                        handOne.Add(drawnCard);
                        InstantiateCardPrefab(drawnCard, ReturnActivePlayer());

                        uiManager.UpdateUI("Deck", "0");
                    }
                }
            }
            else
            {
                for (int i = 0; i < numCards; i++)
                {
                    if (deckTwo.Count > 0)
                    {
                        CardData drawnCard = deckTwo[0];
                        deckTwo.RemoveAt(0);
                        handTwo.Add(drawnCard);
                        InstantiateCardPrefab(drawnCard, ReturnActivePlayer());

                        uiManager.UpdateUI("Deck", "1");
                    }
                }
            }
        }
    }

    public void DiscardCards(int numCards)
    {
        if (ReturnActivePlayer() == 0)
        {
            for (int i = handOne.Count - 1; i >= 0 && numCards > 0; i--)
            {
                discardOne.Add(handOne[i]);
                handOne.RemoveAt(i);
                var child = handOneTransform.transform.GetChild(i);
                Destroy(child.gameObject);
                numCards--;

                uiManager.UpdateUI("Discard", "0");
            }
        }
        else
        {
            for (int i = handTwo.Count - 1; i >= 0 && numCards > 0; i--)
            {
                discardTwo.Add(handTwo[i]);
                handTwo.RemoveAt(i);
                var child = handTwoTransform.transform.GetChild(i);
                Destroy(child.gameObject);
                numCards--;

                uiManager.UpdateUI("Discard", "1");
            }
        }
    }

    private void InstantiateCardPrefab(CardData card, int player)
    {
        if (player == 0)
        {
            GameObject cardGO = Instantiate(cardPrefab, handOneTransform.transform);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.cardData = card;
        }
        else
        {
            GameObject cardGO = Instantiate(cardPrefab, handTwoTransform.transform);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.cardData = card;
        }
    }

    private void InstantiateCardPrefabs()
    {
        foreach (CardData card in handOne)
        {
            GameObject cardGO = Instantiate(cardPrefab, handOneTransform.transform);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.cardData = card;

            // Set card's data in the UI using cardScript, e.g. cardScript.cardName = card.cardName;
            // Set card's position, rotation, etc. based on UI layout requirements
        }

        foreach (CardData card in handTwo)
        {
            GameObject cardGO = Instantiate(cardPrefab, handTwoTransform.transform);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.cardData = card;

            // Set card's data in the UI using cardScript, e.g. cardScript.cardName = card.cardName;
            // Set card's position, rotation, etc. based on UI layout requirements
        }
    }

    #endregion

    #region DiceMethods
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