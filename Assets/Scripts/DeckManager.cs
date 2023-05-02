using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckManager : MonoBehaviour
{
    [Header("Parents")]
    [SerializeField] private Transform cardSelection;
    [SerializeField] private Transform currentDeck;
    [SerializeField] private Transform cardSelection2;
    [SerializeField] private Transform currentDeck2;

    [Header("Prefabs")]
    [SerializeField] private GameObject card2D;

    [Header("Preview")]
    [SerializeField] private GameObject cardPreview;
    [SerializeField] private GameObject cardPreview2;

    [Header("Restrictions")]
    private int type1_One;
    private int type1_Two;
    private int type2_One;
    private int type2_Two;
    private int type3_One;
    private int type3_Two;
    public int type1_limit;
    public int type2_limit;
    public int type3_limit;

    public TextMeshProUGUI restrictionsOne;
    public TextMeshProUGUI restrictionsTwo;

    public GameObject[] warning;

    [Header("Misc")]
    private DataManager dataManager;
    private UIManager uiManager;

    private void Awake()
    {
        dataManager = DataManager.Instance;
        uiManager = GetComponent<UIManager>();

        for (int i = 0; i < dataManager.unlockedCards.Count; i++)
        {
            GameObject cardGO = Instantiate(card2D, cardSelection);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.cardData = dataManager.unlockedCards[i];
            cardScript.player = 0;
            cardScript.deckManager = this;
            cardScript.SwitchButton(false);

            GameObject cardGO2 = Instantiate(card2D, cardSelection2);
            CardPrefab cardScript2 = cardGO2.GetComponent<CardPrefab>();
            cardScript2.cardData = dataManager.unlockedCards[i];
            cardScript2.player = 1;
            cardScript2.deckManager = this;
            cardScript2.SwitchButton(false);
        }

        restrictionsOne.text = string.Format("<color=orange>{0}/{1}</color>\n<color=green>{2}/{3}</color>\n{4}/{5}", type1_One, type1_limit, type2_One, type2_limit, type3_One, type3_limit);
        restrictionsTwo.text = string.Format("<color=orange>{0}/{1}</color>\n<color=green>{2}/{3}</color>\n{4}/{5}", type1_Two, type1_limit, type2_Two, type2_limit, type3_Two, type3_limit);
    }

    public void PreviewCard(CardData card, CardPrefab cardPrfb)
    {
        var cardPreviewData = cardPreview.GetComponent<CardPrefab>();
        var cardPreviewData2 = cardPreview2.GetComponent<CardPrefab>();

        // Create a new instance of CardData
        CardData cardDataCopy = ScriptableObject.CreateInstance<CardData>();

        // Assign the values of the original card to the new instance
        cardDataCopy.cardImage = card.cardImage;
        cardDataCopy.cardBackground = card.cardBackground;
        cardDataCopy.cardName = card.cardName;
        cardDataCopy.cardType = card.cardType;
        cardDataCopy.cardAbility = card.cardAbility;
        cardDataCopy.cardID = card.cardID;
        cardDataCopy.prestigeValue = card.prestigeValue;
        cardDataCopy.purchaseValue = card.purchaseValue;
        cardDataCopy.diceValue = card.diceValue;
        cardDataCopy.diceImage = card.diceImage;
        cardDataCopy.buildingModel = card.buildingModel;

        // Assign the new instance of CardData to cardPreviewData
        if(cardPrfb.player == 0)
        {
            cardPreviewData.cardData = cardDataCopy;
            cardPreviewData.RefreshUI();
        }
        else
        {
            cardPreviewData2.cardData = cardDataCopy;
            cardPreviewData2.RefreshUI();
        }
        //cardPreviewData.cardData = cardDataCopy;

        // Refresh the UI of cardPreviewData
        //cardPreviewData.RefreshUI();
    }


    public void AddCardToPlayer(int player)
    {
        if(player == 0)
        {
            AddCardToDeck(player, cardPreview.GetComponent<CardPrefab>().cardData);
        }
        else
        {
            AddCardToDeck(player, cardPreview2.GetComponent<CardPrefab>().cardData);
        }        
    }

    public void AddCardToDeck(int player, CardData card)
    {
        CardData cardCopy = Instantiate(card);

        if (CheckDeckLimits(player, cardCopy.cardType))
        {
            if (player == 0)
            {
                cardCopy.deckPos = dataManager.playerOneDeck.Count;
                dataManager.playerOneDeck.Add(cardCopy);
                InstantiateCard(0, cardCopy);
                UpdateRestrictions(player, cardCopy.cardType, false);
            }
            else
            {
                cardCopy.deckPos = dataManager.playerTwoDeck.Count;
                dataManager.playerTwoDeck.Add(cardCopy);
                InstantiateCard(1, cardCopy);
                UpdateRestrictions(player, cardCopy.cardType, false);
            }
        }
        else
        {
            GameObject go = null;
            Debug.Log("Limit reached for type: " + cardCopy.cardType);
            switch (cardCopy.cardType)
            {
                case CardData.CardType.Take:
                    if (player == 0) { go = warning[0]; } else { go = warning[3]; }
                    break;
                case CardData.CardType.Gain:
                    if (player == 0) { go = warning[1]; } else { go = warning[4]; }
                    break;
                case CardData.CardType.Profit:
                    if (player == 0) { go = warning[2]; } else { go = warning[5]; }
                    break;
            }

            AudioManager.Instance.Play("limit");
            StartCoroutine(GetComponent<UIManager>().PopUp(1, go));
        }        
    }

    private bool CheckDeckLimits(int player, CardData.CardType type)
    {
        if(player == 0)
        {
            switch (type)
            {
                case CardData.CardType.Take:
                    if(type1_One == type1_limit) { return false; }
                    break;
                case CardData.CardType.Gain:
                    if (type2_One == type2_limit) { return false; }
                    break;
                case CardData.CardType.Profit:
                    if (type3_One == type3_limit) { return false; }
                    break;
            }

            return true;
        }
        else
        {
            switch (type)
            {
                case CardData.CardType.Take:
                    if (type1_Two == type1_limit) { return false; }
                    break;
                case CardData.CardType.Gain:
                    if (type2_Two == type2_limit) { return false; }
                    break;
                case CardData.CardType.Profit:
                    if (type3_Two == type3_limit) { return false; }
                    break;
            }

            return true;
        }
    }

    public void RemoveCardFromDeck(int player, int itemPos, CardData.CardType type)
    {
        if (player == 0)
        {
            EnableButtonOnFirst(0, true);
            dataManager.playerOneDeck.RemoveAt(itemPos);
            UpdateRestrictions(player, type, true);
        }
        else
        {
            EnableButtonOnFirst(1, true);
            dataManager.playerTwoDeck.RemoveAt(itemPos);
            UpdateRestrictions(player, type, true);
        }
    }

    private void InstantiateCard(int player, CardData card)
    {
        if(player == 0)
        {
            GameObject cardGO = Instantiate(card2D, currentDeck);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.deckManager = this;
            cardScript.cardData = card;
            EnableButtonOnFirst(player, false);
        }
        else
        {
            GameObject cardGO = Instantiate(card2D, currentDeck2);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.deckManager = this;
            cardScript.cardData = card;
            EnableButtonOnFirst(player, false);
        }        
    }

    public void UpdateRestrictions(int player, CardData.CardType type, bool remove)
    {
        if(player == 0)
        {
            switch (type)
            {
                case CardData.CardType.Take:
                    if (remove) { type1_One--; } else { type1_One++; }
                    break;
                case CardData.CardType.Gain:
                    if (remove) { type2_One--; } else { type2_One++; }
                    break;
                case CardData.CardType.Profit:
                    if (remove) { type3_One--; } else { type3_One++; }
                    break;
            }

            restrictionsOne.text = string.Format("<color=orange>{0}/{1}</color>\n<color=green>{2}/{3}</color>\n{4}/{5}", type1_One, type1_limit, type2_One, type2_limit, type3_One, type3_limit);
        }
        else
        {
            switch (type)
            {
                case CardData.CardType.Take:
                    if (remove) { type1_Two--; } else { type1_Two++; }
                    break;
                case CardData.CardType.Gain:
                    if (remove) { type2_Two--; } else { type2_Two++; }
                    break;
                case CardData.CardType.Profit:
                    if (remove) { type3_Two--; } else { type3_Two++; }
                    break;
            }

            restrictionsTwo.text = string.Format("<color=orange>{0}/{1}</color>\n<color=green>{2}/{3}</color>\n{4}/{5}", type1_Two, type1_limit, type2_Two, type2_limit, type3_Two, type3_limit);
        }
    }

    public void EnableButtonOnFirst(int player, bool remove)
    {
        if(player == 0)
        {
            for (int i = 0; i < currentDeck.childCount; i++)
            {
                currentDeck.GetComponentsInChildren<CardPrefab>()[i].SwitchButton(false);
            }

            int childCount = (remove) ? childCount = currentDeck.childCount - 2 : childCount = currentDeck.childCount - 1;
            var lastChild = currentDeck.transform.GetChild(childCount);
            lastChild.GetComponent<CardPrefab>().SwitchButton(true);
        }
        else
        {
            for (int i = 0; i < currentDeck2.childCount; i++)
            {
                currentDeck2.GetComponentsInChildren<CardPrefab>()[i].SwitchButton(false);
            }

            int childCount = (remove) ? childCount = currentDeck2.childCount - 2 : childCount = currentDeck2.childCount - 1;
            var lastChild = currentDeck2.transform.GetChild(childCount);
            lastChild.GetComponent<CardPrefab>().SwitchButton(true);
        }
    }
}
