using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Misc")]
    private DataManager dataManager;

    private void Awake()
    {
        dataManager = DataManager.Instance;

        for (int i = 0; i < dataManager.unlockedCards.Count; i++)
        {
            GameObject cardGO = Instantiate(card2D, cardSelection);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.cardData = dataManager.unlockedCards[i];
            cardScript.player = 0;
            cardScript.deckManager = this;

            GameObject cardGO2 = Instantiate(card2D, cardSelection2);
            CardPrefab cardScript2 = cardGO2.GetComponent<CardPrefab>();
            cardScript2.cardData = dataManager.unlockedCards[i];
            cardScript2.player = 1;
            cardScript2.deckManager = this;
        }
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
        if (player == 0)
        {
            dataManager.playerOneDeck.Add(card);
            InstantiateCard(0, card);
        }
        else
        {
            dataManager.playerTwoDeck.Add(card);
            InstantiateCard(1, card);
        }
    }

    private void InstantiateCard(int player, CardData card)
    {
        if(player == 0)
        {
            GameObject cardGO = Instantiate(card2D, currentDeck);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.cardData = card;
        }
        else
        {
            GameObject cardGO = Instantiate(card2D, currentDeck2);
            CardPrefab cardScript = cardGO.GetComponent<CardPrefab>();
            cardScript.cardData = card;
        }        
    }
}
