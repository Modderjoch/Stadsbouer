using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPrefab : MonoBehaviour
{
    [Header("Card Data")]
    public CardData cardData; // Reference to the CardData scriptable object

    [Header("Misc")]
    public GameObject removeButton;
    public Canvas canvas;
    [HideInInspector] public int player;
    [HideInInspector] public int itemPos;
    private CardData.CardType type;
    public GameObject buildingPrefab;
    public Transform particleParent;

    [HideInInspector] public DeckManager deckManager;

    [Header("Text elements")]
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI purchaseValueText;
    public TextMeshProUGUI prestigeValueText;
    public TextMeshProUGUI cardTypeText;
    public TextMeshProUGUI cardAbilityText;

    [Header("Images")]
    public Image cardArt;
    public Image panel;
    public Image dice;

    void Start()
    {
        if(cardData != null)
        {
            // Access and use the cardData properties
            string cardName = cardData.cardName;
            int cardID = cardData.cardID;
            Sprite cardImage = cardData.cardImage;
            int purchaseValue = cardData.purchaseValue;
            int diceValue = cardData.diceValue;
            int prestigeValue = cardData.prestigeValue;
            string cardType = cardData.cardType.ToString();
            string cardAbility = cardData.cardAbility.abilityDescription;
            Sprite cardBackground = cardData.cardBackground;
            Sprite diceImage = cardData.diceImage;
            int cardPos = cardData.deckPos;
            GameObject buildingModel = cardData.buildingModel;
            type = cardData.cardType;

            cardNameText.text = cardName;
            cardArt.sprite = cardImage;
            purchaseValueText.text = purchaseValue.ToString();
            prestigeValueText.text = prestigeValue.ToString();
            cardTypeText.text = cardType;
            cardAbilityText.text = cardAbility;
            panel.sprite = cardBackground;
            itemPos = cardPos;
            buildingPrefab = buildingModel;
            dice.sprite = diceImage;

            if (canvas != null) { canvas.worldCamera = Camera.main; }
        }        
    }

    public void RefreshUI()
    {
        if (cardData != null)
        {
            // Access and use the cardData properties
            string cardName = cardData.cardName;
            int cardID = cardData.cardID;
            Sprite cardImage = cardData.cardImage;
            int purchaseValue = cardData.purchaseValue;
            int diceValue = cardData.diceValue;
            int prestigeValue = cardData.prestigeValue;
            string cardType = cardData.cardType.ToString();
            string cardAbility = cardData.cardAbility.abilityDescription;
            Sprite cardBackground = cardData.cardBackground;
            Sprite diceImage = cardData.diceImage;

            cardNameText.text = cardName;
            cardArt.sprite = cardImage;
            purchaseValueText.text = purchaseValue.ToString();
            prestigeValueText.text = prestigeValue.ToString();
            cardTypeText.text = cardType;
            cardAbilityText.text = cardAbility;
            panel.sprite = cardBackground;
            dice.sprite = diceImage;
        }
    }

    public void PreviewCard()
    {
        deckManager.PreviewCard(cardData, this);
    }

    public void PlayCard()
    {
        Debug.Log("Playing card" + name);
        GameManager.Instance.PlayCard(DataManager.Instance.currentPlayerIndex, gameObject, itemPos, cardData, buildingPrefab);
    }

    public void RemoveFromDeck()
    {
        if(transform.parent.name == "CurrentDeckContainer2") { player = 1; }
        deckManager.RemoveCardFromDeck(player, itemPos, type);
        deckManager.EnableButtonOnFirst(player, true);
        Destroy(gameObject);        
    }

    public void SwitchButton(bool status)
    {
        removeButton.SetActive(status);
    }
}

