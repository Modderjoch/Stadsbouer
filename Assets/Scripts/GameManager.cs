using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variables
    [Header("Managers")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraManager cameraManager;
    private DataManager dataManager;
    [SerializeField] private InputManager inputManager;
    private AudioManager audioManager;

    [Header("Prefabs")]
    [SerializeField] private GameObject dataManagerPrefab;
    [SerializeField] private GameObject audioManagerPrefab;
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private CardData cardDataPrefab;
    [SerializeField] private GameObject buildingBasePrefab;

    [Header("Played Cards")]
    public Transform playingAreaOne;
    public Transform playingAreaTwo;

    [Header("Buildings")]
    public Transform buildAreaOne;
    public Transform buildAreaTwo;

    [Header("Timer")]
    public float timeLeft;
    private float timerTime;
    [HideInInspector] public bool timerIsOn = false;

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
    public List<CardData> playOne;
    public List<CardData> playTwo;

    public List<int> oneID;
    public List<int> twoID;

    private bool firstTime = true;
    private int numberOfCards;

    [Header("User Interface")]
    [SerializeField] private TextMeshProUGUI[] playerOneUpdateUI;
    [SerializeField] private TextMeshProUGUI[] playerTwoUpdateUI;
    [SerializeField] private Canvas canvas;

    [Header("Misc")]
    [SerializeField] private GameObject debugPanel;
    public static GameManager Instance;
    public ParticleSystem placeCardParticle;

    #endregion

    #region StartupMethods
    private void Awake()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;

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

            if (uiManager != null)
            {
                uiManager.ActivePlayerUI(); 
                uiManager.UpdateUI("Money", "0"); 
                uiManager.UpdateUI("Money", "1");
                uiManager.UpdateUI("Score", "0");
                uiManager.UpdateUI("Score", "1");
            }
            if (cameraManager != null) { cameraManager.NextPlayerCamera(dataManager.currentPlayerIndex); }
        }

        if(AudioManager.Instance == null) 
        {
            Instantiate(audioManagerPrefab); 
            audioManager = AudioManager.Instance;
        } 
        else 
        { 
            audioManager = AudioManager.Instance; 
        }

        Instance = this;
        numberOfThrowsLeft = dataManager.numberOfThrows;
        numberOfCards = dataManager.numberOfCards;

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
            deckOne[i] = Instantiate(deckOne[i]);
            deckOne[i].cardID = deckOne[i].cardID + i;
        }

        for (int i = 0; i < deckTwo.Count; i++)
        {
            deckTwo[i] = Instantiate(deckTwo[i]);
            deckTwo[i].cardID = deckTwo[i].cardID + i;
        }

        DrawCards(handSize);
        InstantiateCardPrefabs();        
    }
    #endregion

    #region PlayerMethods
    public void NextPlayer()
    {
        dataManager.diceResult[0] = 0;
        dataManager.diceResult[1] = 0;
        dataManager.diceResult[2] = 0;

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
        dataManager.numberOfCards = numberOfCards;
        ShowDice(numberOfThrowsLeft);
    }

    public void AddScore(int scoreToAdd)
    {
        switch (ReturnActivePlayer())
        {
            case 0:
                dataManager.playerOneScore += scoreToAdd;
                playerOneUpdateUI[1].text = string.Format("<color=green>+{0}</color>" ,scoreToAdd.ToString());
                StartCoroutine(RemoveAfterSeconds(2, playerOneUpdateUI[1].gameObject));
                break;
            case 1:
                dataManager.playerTwoScore += scoreToAdd;
                playerTwoUpdateUI[1].text = string.Format("<color=green>+{0}</color>", scoreToAdd.ToString());
                StartCoroutine(RemoveAfterSeconds(2, playerTwoUpdateUI[1].gameObject));
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

        //CheckDice();
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
                    drawnCard.deckPos = i;
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
                    drawnCard.deckPos = i;
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

                playerOneUpdateUI[2].text = string.Format("<color=red>-{0}</color>", numCards.ToString());
                StartCoroutine(RemoveAfterSeconds(2, playerOneUpdateUI[2].gameObject));
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

                playerTwoUpdateUI[2].text = string.Format("<color=red>-{0}</color>", numCards.ToString());
                StartCoroutine(RemoveAfterSeconds(2, playerTwoUpdateUI[2].gameObject));
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

            playerOneUpdateUI[3].text = string.Format("+" + numCards.ToString());
            StartCoroutine(RemoveAfterSeconds(2, playerOneUpdateUI[3].gameObject));
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

            playerTwoUpdateUI[3].text = string.Format("+" + numCards.ToString());
            StartCoroutine(RemoveAfterSeconds(2, playerTwoUpdateUI[3].gameObject));
        }
    }

    private void InstantiateCardPrefab(CardData card, int player)
    {
        if (player == 0)
        {
            GameObject cardGO = Instantiate(cardPrefab, handOneTransform.transform);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.cardData = card;
            cardScript.itemPos = handOne.Count - 1;
        }
        else
        {
            GameObject cardGO = Instantiate(cardPrefab, handTwoTransform.transform);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.cardData = card;
            cardScript.itemPos = handTwo.Count - 1;
        }
    }

    private void InstantiateCardPrefabs()
    {
        for (int i = 0; i < handOne.Count; i++)
        {
            CardData card = handOne[i];
            GameObject cardGO = Instantiate(cardPrefab, handOneTransform.transform);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.cardData = card;
            for (int j = 0; j < handOne.Count - 1; j++) { cardScript.itemPos = j; }
        }

        for (int i = 0; i < handTwo.Count; i++)
        {
            CardData card = handTwo[i];
            GameObject cardGO = Instantiate(cardPrefab, handTwoTransform.transform);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.cardData = card;
            for (int j = 0; j < handTwo.Count - 1; j++) { cardScript.itemPos = j; }
        }
    }

    public void PlayCard(int player, GameObject card, int cardPos, CardData cardData, GameObject building)
    {
        if(player == 0 && dataManager.numberOfCards > 0)
        {
            if(dataManager.playerOneMoney - cardData.purchaseValue >= 0)
            {
                //UI Stuff
                dataManager.playerOneMoney -= cardData.purchaseValue;
                dataManager.playerOneScore += cardData.prestigeValue;
                uiManager.UpdateUI("Money", "0");
                uiManager.UpdateUI("Score", "0");
                playerOneUpdateUI[0].text = string.Format("-" + cardData.purchaseValue.ToString());
                StartCoroutine(RemoveAfterSeconds(2, playerOneUpdateUI[0].gameObject));
                playerOneUpdateUI[1].text = string.Format("+" + cardData.prestigeValue.ToString());
                StartCoroutine(RemoveAfterSeconds(2, playerOneUpdateUI[1].gameObject));
                audioManager.Play("paying");

                //Card logic
                Destroy(card.GetComponent<MouseHover>());
                card.GetComponent<EventTrigger>().triggers.Clear();
                card.transform.SetParent(playingAreaOne, false);
                Instantiate(placeCardParticle, card.GetComponent<CardPrefab>().particleParent);
                card.AddComponent<Rigidbody>();
                playOne.Add(cardData);
                Debug.Log("Tried to remove at: " + cardPos);
                handOne.RemoveAt(cardPos);
                dataManager.numberOfCards--;

                Debug.Log("Building");
                //Building logic
                GameObject go = Instantiate(buildingBasePrefab, buildAreaOne);
                Transform buildingParent = go.transform.GetChild(1).transform;
                Instantiate(building, buildingParent);
            }
            else
            {
                Debug.Log("Not enough money");
            }
        }
        else
        {
            if(dataManager.numberOfCards > 0)
            {
                if(dataManager.playerTwoMoney - cardData.purchaseValue >= 0)
                {
                    //UI Stuff
                    dataManager.playerTwoMoney -= cardData.purchaseValue;
                    dataManager.playerTwoScore += cardData.prestigeValue;
                    uiManager.UpdateUI("Money", "1");
                    uiManager.UpdateUI("Score", "1");
                    playerTwoUpdateUI[0].text = string.Format("-" + cardData.purchaseValue.ToString());
                    StartCoroutine(RemoveAfterSeconds(2, playerTwoUpdateUI[0].gameObject));
                    playerTwoUpdateUI[1].text = string.Format("+" + cardData.prestigeValue.ToString());
                    StartCoroutine(RemoveAfterSeconds(2, playerTwoUpdateUI[1].gameObject));
                    audioManager.Play("paying");

                    //Card logic
                    Destroy(card.GetComponent<MouseHover>());
                    card.GetComponent<EventTrigger>().triggers.Clear();                    
                    card.transform.SetParent(playingAreaTwo, false);
                    Instantiate(placeCardParticle, card.GetComponent<CardPrefab>().particleParent);
                    card.AddComponent<Rigidbody>();
                    playTwo.Add(cardData);
                    handTwo.RemoveAt(cardPos);
                    dataManager.numberOfCards--;

                    //Building logic
                    GameObject go = Instantiate(buildingBasePrefab, buildAreaTwo);
                    Transform buildingParent = go.transform.GetChild(1).transform;
                    Instantiate(building, buildingParent);
                }
                else
                {
                    Debug.Log("Not enough money");
                }                
            }            
        }
    }

    public void CompareResultToCards(int player, int result)
    {
        Debug.Log("Player threw a " + result + " it was player: " + player);
        if(player == 0)
        {
            int combineResult = 0;

            for (int i = 0; i < playOne.Count; i++)
            {
                if(playOne[i].diceValue == result)
                {
                    Debug.Log(playOne[i].cardName + " was just activated");
                    playOne[i].cardAbility.Ability("self", playOne[i].cardAbility.abilityAmount);
                    combineResult += playOne[i].cardAbility.abilityAmount;
                    uiManager.UpdateUI("Money", "0");
                    audioManager.Play("money");
                }
            }

            if(combineResult > 0)
            {
                playerOneUpdateUI[0].text = string.Format("+{0}", combineResult);
                StartCoroutine(RemoveAfterSeconds(2, playerOneUpdateUI[0].gameObject));
            }
        }
        else
        {
            int combineResult = 0;

            for (int i = 0; i < playTwo.Count; i++)
            {
                if (playTwo[i].diceValue == result)
                {
                    Debug.Log(playTwo[i].cardName + "Card was just activated");
                    playTwo[i].cardAbility.Ability("self", playTwo[i].cardAbility.abilityAmount);
                    combineResult += playTwo[i].cardAbility.abilityAmount;
                    uiManager.UpdateUI("Money", "1");                    
                    audioManager.Play("money");
                }
            }

            if(combineResult > 0)
            {
                playerTwoUpdateUI[0].text = string.Format("+{0}", combineResult);
                StartCoroutine(RemoveAfterSeconds(2, playerTwoUpdateUI[0].gameObject));
            }            
        }
    }

    public void ChangeNumberOfCards(int newNumber) { numberOfCards = newNumber; }

    #endregion

    #region DiceMethods
    public void ShowDice(int diceCount)
    {
        if(diceCount > dice.Count)
        {
            dice[0].gameObject.SetActive(true);
            dice[1].gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < diceCount; i++)
            {
                dice[i].gameObject.SetActive(true);
            }
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

    public void StartDiceCheck()
    {
        Debug.Log("DiceCheckk");
        if (dataManager.diceIsCounted)
        {
            InvokeRepeating("CheckDice", 1.5f, .1f);
        }
    }

    private void CheckDice()
    {
        Debug.Log("checking dice result");
        if (dice.Count != 0 && dice[1] != null && dice[1].gameObject.activeSelf)
        {
            if (dice[0].GetComponent<Rigidbody>().velocity.magnitude < 0.01 && dice[1].GetComponent<Rigidbody>().velocity.magnitude < 0.01 && dataManager.diceIsCounted)
            {
                dataManager.diceResult[2] = dataManager.diceResult[0] + dataManager.diceResult[1];
                uiManager.UpdateUI("Dice", ("Result is: " + dataManager.diceResult[2]));
                Debug.Log("Result is: " + dataManager.diceResult[2]);

                CompareResultToCards(ReturnActivePlayer(), dataManager.diceResult[2]);

                CancelInvoke();
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
                    Debug.Log("Result is: " + dataManager.diceResult[0]);

                    CompareResultToCards(ReturnActivePlayer(), dataManager.diceResult[0]);

                    dataManager.diceResult[0] = 0;
                    CancelInvoke();
                    dataManager.diceIsCounted = false;
                }
            }
        }
    }

    public void ChangeNumberOfThrows(int newNumber) { numberOfThrowsLeft = newNumber; }
    public void AddMoney() { 
        if(ReturnActivePlayer() == 0) 
        {    
            dataManager.playerOneMoney += 3;
            uiManager.UpdateUI("Money", "0");
        } 
        else 
        {
            dataManager.playerTwoMoney += 3;
            uiManager.UpdateUI("Money", "1");
        } 
    }
    #endregion

    private IEnumerator RemoveAfterSeconds(int seconds, GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }

    public void DebugPanel()
    {
        if (debugPanel.activeSelf)
        {
            debugPanel.SetActive(false);
        }
        else
        {
            debugPanel.SetActive(true);
        }
    }
}