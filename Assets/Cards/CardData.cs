using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Custom/Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int purchaseValue;
    public int diceValue;
    public int prestigeValue;
    public CardType cardType;
    public CardAbility cardAbility;

    // Custom enum for card type
    public enum CardType
    {
        Normal,
        Special
    }

    // Custom class for card ability
    [System.Serializable]
    public class CardAbility
    {
        public bool hasAbility;
        public string abilityDescription;
        // Add any additional properties or data for card ability here
    }
}
