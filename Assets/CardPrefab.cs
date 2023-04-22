using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPrefab : MonoBehaviour
{
    [Header("Card Data")]
    public CardData cardData; // Reference to the CardData scriptable object

    public int player;

    [HideInInspector] public DeckManager deckManager;

    [Header("Text elements")]
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI purchaseValueText;
    public TextMeshProUGUI diceValueText;
    public TextMeshProUGUI prestigeValueText;
    public TextMeshProUGUI cardTypeText;
    public TextMeshProUGUI cardAbilityText;

    [Header("Images")]
    public Image cardArt;
    public Image panel;

    void Start()
    {
        if(cardData != null)
        {
            Debug.Log("Initiating card details");
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

            cardNameText.text = cardName;
            cardArt.sprite = cardImage;
            purchaseValueText.text = purchaseValue.ToString();
            diceValueText.text = diceValue.ToString();
            prestigeValueText.text = prestigeValue.ToString();
            cardTypeText.text = cardType;
            cardAbilityText.text = cardAbility;
            panel.sprite = cardBackground;
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

            cardNameText.text = cardName;
            cardArt.sprite = cardImage;
            purchaseValueText.text = purchaseValue.ToString();
            diceValueText.text = diceValue.ToString();
            prestigeValueText.text = prestigeValue.ToString();
            cardTypeText.text = cardType;
            cardAbilityText.text = cardAbility;
            panel.sprite = cardBackground;
        }
    }

    public void PreviewCard()
    {
        deckManager.PreviewCard(cardData, this);
    }
}

