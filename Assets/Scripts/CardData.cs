using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Custom/Card")]
public class CardData : ScriptableObject
{
    [Header("Images")]
    public Sprite cardImage;
    public Sprite cardBackground;
    [Header("Data")]
    public string cardName;
    public int cardID;
    [Header("Values")]
    public int purchaseValue;
    public int diceValue;
    public int prestigeValue;
    [Header("Abilities")]
    public CardType cardType;
    public CardAbility cardAbility;
   

    // Custom enum for card type
    public enum CardType
    {
        Orange,
        White,
        Black
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
