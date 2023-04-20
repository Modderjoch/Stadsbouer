using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPrefab : MonoBehaviour
{
    [Header("Card Data")]
    public CardData cardData; // Reference to the CardData scriptable object

    [Header("Text elements")]
    [SerializeField] public TextMeshProUGUI cardNameText;
    [SerializeField] public Image cardArt;
    [SerializeField] public TextMeshProUGUI purchaseValueText;
    [SerializeField] public TextMeshProUGUI diceValueText;
    [SerializeField] public TextMeshProUGUI prestigeValueText;
    [SerializeField] public TextMeshProUGUI cardTypeText;
    [SerializeField] public TextMeshProUGUI cardAbilityText;

    void Start()
    {

        Debug.Log("Initiating card details");
        // Access and use the cardData properties
        string cardName = cardData.cardName;
        Sprite cardImage = cardData.cardImage;
        int purchaseValue = cardData.purchaseValue;
        int diceValue = cardData.diceValue;
        int prestigeValue = cardData.prestigeValue;
        string cardType = cardData.cardType.ToString();
        string cardAbility = cardData.cardAbility.abilityDescription;

        cardNameText.text = cardName;
        cardArt.sprite = cardImage;
        purchaseValueText.text = purchaseValue.ToString();
        diceValueText.text = diceValue.ToString();
        prestigeValueText.text = prestigeValue.ToString();
        cardTypeText.text = cardType;
        cardAbilityText.text = cardAbility;
}
}

